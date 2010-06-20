//  
//  Copyright (C) 2009 Jason Smith, Robert Dyer
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Cairo;
using Gdk;
using Mono.Unix;
using Wnck;

using Docky.Menus;
using Docky.Services;
using Docky.Windowing;

namespace Docky.Items
{
	public class ApplicationDockItem : WnckDockItem
	{
		public static ApplicationDockItem NewFromUri (string uri)
		{
			try {
				string filename = Gnome.Vfs.Global.GetLocalPathFromUri (uri);
				if (!File.Exists (filename))
					return null;
				
				DesktopItem desktopItem = WindowMatcher.Default.DesktopItemForDesktopFile (filename);
				
				if (desktopItem == null)
					desktopItem = new DesktopItem (filename);

				return new ApplicationDockItem (desktopItem);
				
			} catch (Exception e) {
				Log<ApplicationDockItem>.Error (e.Message);
				Log<ApplicationDockItem>.Debug (e.StackTrace);
			}

			return null;
		}
		
		bool can_manage_windows;
		IEnumerable<string> mimes;
		
		public DesktopItem OwnedItem { get; private set; }
	
		public override string ShortName {
			get {
				// FIXME
				return HoverText;
			}
		}
		
		public ApplicationDockItem (DesktopItem item)
		{
			OwnedItem = item;
			WindowMatcher.Default.RegisterDesktopItem (OwnedItem);
			can_manage_windows = true;
			
			UpdateInfo ();
			UpdateWindows ();
			
			OwnedItem.HasChanged += HandleOwnedItemHasChanged;
			
			Wnck.Screen.Default.WindowOpened += WnckScreenDefaultWindowOpened;
			Wnck.Screen.Default.WindowClosed += WnckScreenDefaultWindowClosed;
			Wnck.Screen.Default.ActiveWindowChanged += WnckScreenDefaultWindowChanged;
		}

		void HandleOwnedItemHasChanged (object sender, EventArgs e)
		{
			UpdateInfo ();
		}
		
		void UpdateInfo ()
		{
			if (OwnedItem.HasAttribute ("Icon"))
				Icon = OwnedItem.GetString (("Icon"));
			
			if (OwnedItem.HasAttribute ("X-GNOME-FullName")) {
				HoverText = OwnedItem.GetString (("X-GNOME-FullName"));
			} else if (OwnedItem.HasAttribute ("Name")) {
				HoverText = OwnedItem.GetString (("Name"));
			} else {
				HoverText = System.IO.Path.GetFileNameWithoutExtension (OwnedItem.Path);
			}
			
			if (OwnedItem.HasAttribute ("MimeType")) {
				mimes = OwnedItem.GetStrings ("MimeType");
			} else {
				mimes = Enumerable.Empty<string> ();
			}
			
			if (OwnedItem.HasAttribute ("X-Docky-NoMatch") && OwnedItem.GetBool ("X-Docky-NoMatch")) {
				can_manage_windows = false;
			}
		}
		
		public override string UniqueID ()
		{
			return OwnedItem.Path;
		}
		
		void WnckScreenDefaultWindowClosed (object o, WindowClosedArgs args)
		{
			UpdateWindows ();
			OnPaintNeeded ();
		}

		void WnckScreenDefaultWindowOpened (object o, WindowOpenedArgs args)
		{
			UpdateWindows ();
			OnPaintNeeded ();
		}
		
		void WnckScreenDefaultWindowChanged (object o, ActiveWindowChangedArgs args)
		{
			UpdateWindows ();
			OnPaintNeeded ();
		}

		void UpdateWindows ()
		{
			if (can_manage_windows)
				Windows = WindowMatcher.Default.WindowsForDesktopItem (OwnedItem);
			else
				Windows = Enumerable.Empty<Wnck.Window> ();
		}
		
		public void RecollectWindows () 
		{
			UpdateWindows ();
			OnPaintNeeded ();
		}
		
		protected override MenuList OnGetMenuItems ()
		{
			MenuList list = base.OnGetMenuItems ();
			if (ManagedWindows.Any ())
				list[MenuListContainer.Actions].Insert (0, new MenuItem (Catalog.GetString ("New _Window"), RunIcon, (o, a) => Launch ()));
			else
				list[MenuListContainer.Actions].Insert (0, new MenuItem (Catalog.GetString ("_Open"), RunIcon, (o, a) => Launch ()));

			return list;
		}
		
		protected override bool OnCanAcceptDrop (IEnumerable<string> uris)
		{
			if (uris == null)
				return false;
			
			foreach (string uri in uris) {
				string mime = GLib.FileFactory.NewForUri (uri).QueryInfo<string> ("standard::content-type");
				if (mimes.Any (m => GLib.Content.TypeIsA (mime, m) || GLib.Content.TypeEquals (mime, m)))
					return true;
			}
			
			return base.OnCanAcceptDrop (uris);
		}
		
		protected override bool OnAcceptDrop (IEnumerable<string> uris)
		{
			LaunchWithFiles (uris);
			return true;
		}


		protected override ClickAnimation OnClicked (uint button, ModifierType mod, double xPercent, double yPercent)
		{
			if ((!ManagedWindows.Any () && button == 1) || button == 2 || 
				(button == 1 && (mod & ModifierType.ControlMask) == ModifierType.ControlMask)) {
				Launch ();
				return ClickAnimation.Bounce;
			}
			return base.OnClicked (button, mod, xPercent, yPercent);
		}
		
		void Launch ()
		{
			LaunchWithFiles (Enumerable.Empty<string> ());
		}
		
		public void LaunchWithFiles (IEnumerable<string> files)
		{
			OwnedItem.Launch (files);
		}
		
		public override void Dispose ()
		{
			if (OwnedItem != null)
				OwnedItem.HasChanged -= HandleOwnedItemHasChanged;

			Wnck.Screen.Default.WindowOpened -= WnckScreenDefaultWindowOpened;
			Wnck.Screen.Default.WindowClosed -= WnckScreenDefaultWindowClosed;
			Wnck.Screen.Default.ActiveWindowChanged -= WnckScreenDefaultWindowChanged;
			
			base.Dispose ();
		}
	}
}
