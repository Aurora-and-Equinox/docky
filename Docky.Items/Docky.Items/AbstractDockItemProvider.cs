//  
//  Copyright (C) 2009 Robert Dyer
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
using System.ComponentModel;
using System.Linq;
using System.Text;

using Docky;
using Docky.Menus;
using Docky.CairoHelper;
using Docky.Services;

namespace Docky.Items
{
	public abstract class AbstractDockItemProvider
	{
		public event EventHandler<ItemsChangedArgs> ItemsChanged;
		
		public abstract string Name { get; }
		
		public abstract string Icon { get; }
		
		public virtual bool Separated {
			get { return false; }
		}
		
		IEnumerable<AbstractDockItem> items;
		public IEnumerable<AbstractDockItem> Items {
			get { return items; }
			protected set {
				IEnumerable<AbstractDockItem> added = value.Where (adi => !items.Contains (adi)).ToArray ();
				IEnumerable<AbstractDockItem> removed = items.Where (adi => !value.Contains (adi)).ToArray ();
				
				int position = items.Any () ? items.Max (adi => adi.Position) + 1 : 0;
				foreach (AbstractDockItem item in added) {
					item.AddTime = DateTime.UtcNow;
					item.Position = position;
					position++;
				}
				
				items = value.ToArray ();
				foreach (AbstractDockItem item in items)
					item.Owner = this;
				
				OnItemsChanged (added, removed);
			}
		}
		
		protected AbstractDockItemProvider ()
		{
			items = Enumerable.Empty<AbstractDockItem> ();
		}
		
		public bool CanAcceptDrop (string uri)
		{
			try {
				return OnCanAcceptDrop (uri);
			} catch (Exception e) {
				Log<AbstractDockItem>.Error (e.Message);
				Log<AbstractDockItemProvider>.Debug (e.StackTrace);
			}
			return false;
		}
		
		protected virtual bool OnCanAcceptDrop (string uri)
		{
			return false;
		}
		
		public bool AcceptDrop (string uri, int position)
		{
			AbstractDockItem newItem = null;
			try {
				newItem = OnAcceptDrop (uri);
			} catch (Exception e) {
				Log<AbstractDockItem>.Error (e.Message);
				Log<AbstractDockItemProvider>.Debug (e.StackTrace);
			}
			
			if (newItem != null) {
				foreach (AbstractDockItem item in Items.Where (adi => adi.Position >= position)) {
					item.Position++;
				}
				newItem.Position = position;
			}
			
			OnItemsChanged (null, null);
			
			return newItem != null;
		}
		
		protected virtual AbstractDockItem OnAcceptDrop (string uri)
		{
			return null;
		}
		
		public virtual bool ItemCanBeRemoved (AbstractDockItem item)
		{
			return false;
		}
		
		public virtual bool RemoveItem (AbstractDockItem item)
		{
			return false;
		}
		
		public virtual MenuList GetMenuItems (AbstractDockItem item)
		{
			return item.GetMenuItems ();
		}
		
		void OnItemsChanged (IEnumerable<AbstractDockItem> added, IEnumerable<AbstractDockItem> removed)
		{
			if (ItemsChanged != null)
				ItemsChanged (this, new ItemsChangedArgs (added, removed));
		}
		
		public abstract void Dispose ();
		
		public virtual void AddedToDock ()
		{
		}
	}
}
