//  
//  Copyright (C) 2009 Jason Smith, Chris Szikszoy
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using GLib;

using Docky.Items;
using Docky.Menus;

namespace Docky.DBus
{
	public struct ItemTuple
	{
		public string Name;
		public string Icon;
		public string Title;
		
		public ItemTuple (string name, string icon, string title)
		{
			Name = name;
			Icon = icon;
			Title = title;
		}
	}
	
	public class DockyDBusItem : IDockyDBusItem, IDisposable
	{
		uint timer;
		Dictionary<uint, RemoteMenuEntry> items;
		Dictionary<uint, DateTime> update_time;
		
		List<uint> known_ids;
		
		AbstractDockItem owner;
		
		public DockyDBusItem (AbstractDockItem item)
		{
			owner = item;
			known_ids = new List<uint> ();
			items = new Dictionary<uint, RemoteMenuEntry> ();
			update_time = new Dictionary<uint, DateTime> ();
			
			timer = GLib.Timeout.Add (4 * 60 * 1000, delegate {
				TriggerConfirmation ();
				return true;
			});
		}
		
		public void TriggerConfirmation ()
		{
			if (ItemConfirmationNeeded != null)
				ItemConfirmationNeeded ();
			
			GLib.Timeout.Add (30 * 1000, delegate {
				foreach (uint i in update_time
					.Where (kvp => (DateTime.UtcNow - kvp.Value).TotalMinutes > 1)
					.Select (kvp => kvp.Key))
					RemoveItem (i);
				
				return false;
			});
		}
		
		uint GetRandomID ()
		{
			Random rand = new Random ();
			
			uint number;
			
			do {
				//FIXME should we ever get 100,000 items in here, I hope we crash, though we will likely get an infinite loop
				number = (uint) rand.Next (0, 100000);
			} while (known_ids.BinarySearch (number) >= 0);
			
			known_ids.Add (number);
			known_ids.Sort ();
			
			return number;
		}
		
		#region IDockyDBusMenus implementation
		public event MenuItemActivatedHandler MenuItemActivated;
		
		public event Action ItemConfirmationNeeded;
		
		public string Name {
			get {
				return owner.ShortName;
			}
		}
		
		public string BadgeText {
			get {
				return owner.BadgeText;
			}
			set {
				owner.SetRemoteBadgeText (value);
			}
		}
		
		public string Text {
			get {
				return owner.HoverText;
			}
			set {
				owner.SetRemoteText (value);
			}
		}
		
		public string Icon {
			get {
				if (CanSetIcon)
					return (owner as IconDockItem).Icon;
				return "custom";
			}
			set {
				if (!CanSetIcon)
					return;
				
				(owner as IconDockItem).SetRemoteIcon (value);
			}
		}

		public bool CanSetIcon {
			get {
				return owner is IconDockItem;
			}
		}
		
		public bool OwnsDesktopFile {
			get {
				return owner is ApplicationDockItem;
			}
		}
		
		public bool OwnsUri {
			get {
				return owner is FileDockItem;
			}
		}
		
		public bool Attention { 
			get { return (owner.State & ItemState.Urgent) == ItemState.Urgent; }
		}
		
		public bool Wait {
			get { return (owner.State & ItemState.Wait) == ItemState.Wait; }
		}
		
		public string DesktopFile {
			get {
				if (owner is ApplicationDockItem)
					return (owner as ApplicationDockItem).OwnedItem.Path;
				return "";
			}
		}
		
		public string Uri {
			get {
				if (owner is FileDockItem)
					return (owner as FileDockItem).Uri;
				return "";
			}
		}
		
		public uint[] Items {
			get {
				return items.Keys.ToArray ();
			}
		}
		
		public uint AddMenuItem (string name, string icon, string title)
		{
			uint number = GetRandomID ();
			
			RemoteMenuEntry rem = new RemoteMenuEntry (number, name, icon, title);
			rem.Clicked += HandleActivated;
			
			AddToList (rem, number);
			
			return number;
		}
		
		public uint AddFileMenuItem (string uri, string title)
		{			
			uint number = GetRandomID ();
			
			RemoteFileMenuEntry rem = new RemoteFileMenuEntry (number, FileFactory.NewForUri (uri), title);
			
			AddToList (rem, number);
			
			return number;
		}
		
		private void AddToList (RemoteMenuEntry entry, uint id)
		{
			items[id] = entry;
			update_time[id] = DateTime.UtcNow;
			
			//TODO Insert items into list... this is stupid but whatever fix later
			foreach (MenuItem item in items.Values)
				owner.RemoteMenuItems.Remove (item);
			
			MenuListContainer _container = MenuListContainer.Footer + 1;
			var groupedItems = items.Values
				.GroupBy (rmi => rmi.Title)
				.OrderBy (g => g.Key);
			
			foreach (var itemGroup in groupedItems) {
				MenuListContainer container;
				
				switch (itemGroup.Key.ToLower ()) {
				case "actions":
					container = MenuListContainer.Actions;
					break;
				case "relateditems":
					container = MenuListContainer.RelatedItems;
					break;
				case "windows":
					container = MenuListContainer.Windows;
					break;
				case "header":
					container = MenuListContainer.Header;
					break;
				case "footer":
					container = MenuListContainer.Footer;
					break;
				default:
					container = _container;
					owner.RemoteMenuItems.SetContainerTitle (container, itemGroup.Key);
					break;
				}
				
				foreach (MenuItem item in itemGroup.OrderBy (i => i.Text)) {
					owner.RemoteMenuItems[container].Add (item);
				}
				_container++;
			}
		}
		
		public void RemoveItem (uint item)
		{
			
			if (items.ContainsKey (item)) {
				RemoteMenuEntry entry = items[item];
				entry.Clicked -= HandleActivated;
				
				items.Remove (item);
				
				owner.RemoteMenuItems.Remove (entry);
			}
			
			known_ids.Remove (item);
		}
		
		public void ConfirmItem (uint item)
		{
			update_time[item] = DateTime.UtcNow;
		}
		
		public void SetAttention ()
		{
			owner.State |= ItemState.Urgent;
		}
		
		public void UnsetAttention ()
		{
			owner.State &= ~ItemState.Urgent;
		}

		public void SetWaiting ()
		{
			owner.State |= ItemState.Wait;
		}
		
		public void UnsetWaiting ()
		{
			owner.State &= ~ItemState.Wait;
		}
		
		public void ResetBadgeText ()
		{
			owner.SetRemoteBadgeText ("");
		}
		
		public void ResetText ()
		{
			owner.SetRemoteText ("");
		}
		
		public void ResetIcon ()
		{
			if (!CanSetIcon)
				return;
			
			(owner as IconDockItem).SetRemoteIcon ("");
		}
		
		public ItemTuple GetItem (uint item)
		{
			if (!items.ContainsKey (item))
				return new ItemTuple ("", "", "");
			
			RemoteMenuEntry entry = items[item];
			return new ItemTuple (entry.Text, entry.Icon, entry.Title);
		}
		
		#endregion
			
		void HandleActivated (object sender, EventArgs args)
		{
			if (!(sender is RemoteMenuEntry))
				return;
			
			if (MenuItemActivated != null)
				MenuItemActivated ((sender as RemoteMenuEntry).ID);
		}
		
		#region IDisposable implementation
		public void Dispose ()
		{
			if (timer > 0)
				GLib.Source.Remove (timer);
			
			known_ids.Clear ();
			update_time.Clear ();
			
			foreach (RemoteMenuEntry m in items.Values) {
				m.Clicked -= HandleActivated;
				m.Dispose ();
			}
			items.Clear ();
			
			owner = null;
		}
		#endregion
	}
}
