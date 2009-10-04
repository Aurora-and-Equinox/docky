//  
//  Copyright (C) 2009 Jason Smith
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
using System.Threading;

using Cairo;
using Gdk;
using Wnck;

using Docky.Menus;
using Docky.Services;
using Docky.Windowing;
using Docky.Zeitgeist;

namespace Docky.Items
{

	public class ApplicationDockItem : WnckDockItem
	{
		public static ApplicationDockItem NewFromUri (string uri)
		{
			Gnome.DesktopItem desktopItem;
			string filename = Gnome.Vfs.Global.GetLocalPathFromUri (uri);
			
			try {
				desktopItem = Gnome.DesktopItem.NewFromFile (filename, 0);
			} catch (Exception e) {
				Console.Error.WriteLine (e.Message);
				return null;
			}
			
			if (desktopItem == null)
				return null;
			
			return new ApplicationDockItem (desktopItem);
		}
		
		Gnome.DesktopItem desktop_item;
		string path;
		ZeitgeistResult[] related_uris;
		object related_lock;
		
		uint timer;
	
		public override string ShortName {
			get {
				return HoverText; // fixme
			}
		}
		
		private ApplicationDockItem (Gnome.DesktopItem item)
		{
			related_lock = new Object ();
			related_uris = new ZeitgeistResult[0];
			
			desktop_item = item;
			if (item.AttrExists ("Icon"))
				Icon = item.GetString ("Icon");
			
			if (item.AttrExists ("Name")) {
				try {
					HoverText = item.GetLocalestring ("Name");
				} catch {
					HoverText = item.GetString ("Name");
				}
			} else {
				HoverText = System.IO.Path.GetFileNameWithoutExtension (path);
			}
			
			path = Gnome.Vfs.Global.GetLocalPathFromUri (item.Location);
			
			UpdateWindows ();
			UpdateRelated ();
			
			timer = GLib.Timeout.Add (10 * 60 * 1000, delegate {
				UpdateRelated ();
				return true;
			});
			
			Wnck.Screen.Default.WindowOpened += WnckScreenDefaultWindowOpened;
			Wnck.Screen.Default.WindowClosed += WnckScreenDefaultWindowClosed;
		}

		public override string UniqueID ()
		{
			return path;
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
		
		void UpdateWindows ()
		{
			Windows = WindowMatcher.Default.WindowsForDesktopFile (path);
		}
		
		void UpdateRelated ()
		{
			if (desktop_item.AttrExists ("MimeType")) {
				string[] mimes = desktop_item.GetString ("MimeType").Split (';');
				Thread th = new Thread ((ThreadStart) delegate {
					Zeitgeist.ZeitgeistFilter filter = new Zeitgeist.ZeitgeistFilter ();
					filter.MimeTypes.AddRange (mimes);
					
					ZeitgeistResult[] uris = ZeitgeistProxy.Default.FindEvents (
						DateTime.Now.AddDays (-31), 
						DateTime.Now, 
						4, 
						false, 
						"mostused",
						filter.AsSingle ())
						.ToArray ();
					
					lock (related_lock) {
						related_uris = uris;
					}
				});
				th.Priority = ThreadPriority.BelowNormal;
				th.Start ();
			}
		}
		
		public override IEnumerable<MenuItem> GetMenuItems ()
		{
			if (ManagedWindows.Any ())
				yield return new MenuItem ("New Instance", RunIcon, (o, a) => Launch ());
			else
				yield return new MenuItem ("Open", RunIcon, (o, a) => Launch ());
			
			foreach (MenuItem item in base.GetMenuItems ()) {
				yield return item;
			}
			
			if (related_uris.Any ()) {
				yield return new SeparatorMenuItem ();
				
				lock (related_lock) {
					foreach (ZeitgeistResult result in related_uris) {
						RelatedFileMenuItem item = new RelatedFileMenuItem (result.Uri);
						if (!string.IsNullOrEmpty (result.Text))
							item.Text = result.Text;
						item.Clicked += ItemClicked;
						yield return item;
					}
				}
			}
		}

		void ItemClicked (object sender, EventArgs e)
		{
			RelatedFileMenuItem item = sender as RelatedFileMenuItem;
			if (item == null)
				return;
			
			LaunchWithFiles (item.Uri.AsSingle ());
		}

		protected override ClickAnimation OnClicked (uint button, ModifierType mod, double xPercent, double yPercent)
		{
			if ((!ManagedWindows.Any () && button == 1) || button == 2) {
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
			if (files.Any ()) {
				GLib.List glist = new GLib.List (files.ToArray () as object[], typeof(string), false, true);
				desktop_item.Launch (glist, Gnome.DesktopItemLaunchFlags.OnlyOne);
				glist.Dispose ();
			} else {
				desktop_item.Launch (null, 0);
			}
		}
		
		public override void Dispose ()
		{
			GLib.Source.Remove (timer);
			base.Dispose ();
		}
	}
}
