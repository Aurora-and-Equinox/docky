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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Docky.Items;

namespace Clock
{


	public class ClockItemProvider : IDockItemProvider
	{

		#region IDockItemProvider implementation
		public event EventHandler<ItemsChangedArgs> ItemsChanged;
		
		public bool ItemCanBeRemoved (AbstractDockItem item)
		{
			return false;
		}
		
		public bool RemoveItem (AbstractDockItem item)
		{
			return false;
		}
		
		public string Name {
			get {
				return "Clock";
			}
		}
		
		public bool Separated {
			get {
				return false;
			}
		}
		
		public IEnumerable<AbstractDockItem> Items {
			get {
				yield return clock;
			}
		}
		#endregion

		ClockDockItem clock;
		
		public ClockItemProvider ()
		{
			clock = new ClockDockItem ();
			clock.Owner = this;
		}

		#region IDisposable implementation
		public void Dispose ()
		{
			clock.Dispose ();
		}
		#endregion

	}
}