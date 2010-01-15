//  
//  Copyright (C) 2010 Chris Szikszoy
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

namespace DockyHelper
{

	public class BaseMenuItem
	{
		public event EventHandler Activated;
		public uint Handle { get; internal set; }
		public string Title { get; protected set; }
		
		internal IDockyItem Owner { get; set; }

		internal void Init ()
		{
			Owner.MenuItemActivated += delegate(uint menuHandle) {
				if (menuHandle == Handle) {
					OnActivated ();
				}
			};
			
			Owner.ItemConfirmationNeeded += delegate {
				Owner.ConfirmItem (Handle);
			};
		}

		void OnActivated ()
		{
			if (Activated != null)
				Activated (this, EventArgs.Empty);
		}
	}
}

