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
using System.Linq;

using GLib;

using Docky.Services;
using Docky.Items;
using Docky.Menus;

namespace Bookmarks
{
	public class BookmarksItemProvider : AbstractDockItemProvider
	{
		class NonRemovableItem : FileDockItem
		{
			public NonRemovableItem (string uri, string name, string icon) : base (uri)
			{
				if (!string.IsNullOrEmpty (icon))
				    Icon = icon;
				    
				if (string.IsNullOrEmpty (name))
					HoverText = OwnedFile.Basename;
				else 
					HoverText = name;
			}
			
			public override IEnumerable<Docky.Menus.MenuItem> GetMenuItems ()
			{
				yield return new MenuItem ("Open", "gtk-open", (o, a) => Open ());
			}
		}
		
		NonRemovableItem computer, home;
		
		List<AbstractDockItem> items;
		
		File BookmarksFile {
			get {
				string path = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), ".gtk-bookmarks");
				
				return FileFactory.NewForPath (path);
			}
		}
		
		public BookmarksItemProvider ()
		{
			items = new List<AbstractDockItem> ();

			computer = new NonRemovableItem ("computer://", "Computer", "computer");
			home = new NonRemovableItem (string.Format ("file://{0}",
			    Environment.GetFolderPath (Environment.SpecialFolder.Personal)), null, null);
		
			UpdateItems (BookmarksFile);
			
			GType.Init ();
			
			FileMonitor watcher = FileMonitor.File (BookmarksFile, FileMonitorFlags.None, null);
			
			watcher.Changed += WatcherChanged;
		}

		void WatcherChanged (object o, ChangedArgs args)
		{
			// FIXME: bug in current GIO / GLib-sharp, should be able to use args.File
			File f = (File) FileAdapter.GetObject (args.Args[0] as GLib.Object);
			
			if (args.EventType == FileMonitorEvent.ChangesDoneHint)
				Gtk.Application.Invoke ( delegate {
					UpdateItems (f);
				});
		}
		
		void UpdateItems (File file)
		{
			List<AbstractDockItem> old = items;
			items = new List<AbstractDockItem> ();
			
			Log<BookmarksItemProvider>.Debug ("Updating bookmarks.");
			
			if (file.QueryExists (null)) {
				using (DataInputStream stream = new DataInputStream (file.Read (null))) {
					ulong length;
					string line, name, uri;
					while ((line = stream.ReadLine (out length, null)) != null) {
						uri = line.Split (' ').First ();
						File bookmark = FileFactory.NewForUri (uri);
						name = line.Substring (uri.Length).Trim ();
						if (old.Cast<BookmarkDockItem> ().Any (fdi => fdi.Uri == uri)) {
							BookmarkDockItem item = old.Cast<BookmarkDockItem> ().First (fdi => fdi.Uri == uri);
							old.Remove (item);
							items.Add (item);
						} else if (bookmark.Uri.Scheme == "file" && !bookmark.Exists) {
							Log<BookmarksItemProvider>.Warn ("Bookmark path '{0}' does not exist, please fix the bookmarks file",
							    bookmark.Uri.ToString ());
							continue;
						} else {
							BookmarkDockItem item = BookmarkDockItem.NewFromUri (bookmark.Uri.ToString (), name);
							if (item != null) {
								item.Owner = this;
								items.Add (item);
							}
						}
					}
				}
			}
			
			foreach (AbstractDockItem item in old)
				item.Dispose ();
			
			OnItemsChanged (items, old);
		}

		#region IDockItemProvider implementation
		
		public override string Name {
			get {
				return "Bookmark Items";
			}
		}
		
		public override string Icon { get { return "folder-home"; } }
		
		public override bool ItemCanBeRemoved (AbstractDockItem item)
		{
			return item is BookmarkDockItem;
		}
		
		public override bool RemoveItem (AbstractDockItem item)
		{
			if (!ItemCanBeRemoved (item))
				return false;
			
			BookmarkDockItem bookmark = item as BookmarkDockItem;
			
			if (!bookmark.OwnedFile.Exists)
				return false;
			
			File tempFile = FileFactory.NewForPath (System.IO.Path.GetTempFileName());
			
			using (DataInputStream reader = new DataInputStream (BookmarksFile.Read (null))) {
				using (DataOutputStream writer = new DataOutputStream (tempFile.AppendTo (FileCreateFlags.None, null))) {
					string line;
					ulong length;
					while ((line = reader.ReadLine (out length, null)) != null) {
						if (!line.Contains (bookmark.Uri))
							writer.PutString (string.Format ("{0}\n", line), null);
						else {
							items.Remove (bookmark);
							OnItemsChanged (null, (bookmark as AbstractDockItem).AsSingle ());
							Log<BookmarksItemProvider>.Debug ("Removing '{0}'", bookmark.HoverText);
						}
					}
				}
			}
			
			if (tempFile.Exists)
				tempFile.Move (BookmarksFile, FileCopyFlags.Overwrite, null, null);

			return true;
		}
		
		public override bool Separated {
			get {
				return true;
			}
		}
		
		public override IEnumerable<AbstractDockItem> Items {
			get {
				yield return computer;
				yield return home;
				foreach (AbstractDockItem item in items)
					yield return item;
			}
		}
		
		public override void Dispose ()
		{
			foreach (AbstractDockItem item in items)
				item.Dispose ();
		}
		
		#endregion
	}
}
