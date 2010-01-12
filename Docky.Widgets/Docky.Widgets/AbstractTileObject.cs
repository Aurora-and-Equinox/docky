//  
//  Copyright (C) 2009 Chris Szikszoy
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

using Mono.Unix;

namespace Docky.Widgets
{

	public abstract class AbstractTileObject
	{
		/// <summary>
		/// Triggered when the icon for this tile is updated
		/// </summary>
		public event EventHandler IconUpdated;
		/// <summary>
		/// Triggered when any text field for this tile is updated
		/// </summary>
		public event EventHandler TextUpdated;
		/// <summary>
		/// Triggered when a button is updated (Show changed, added, or removed)
		/// </summary>
		public event EventHandler ButtonsUpdated;

		void OnIconUpdated ()
		{
			if (IconUpdated != null)
				IconUpdated (this, EventArgs.Empty);
		}
		
		void OnTextUpdated ()
		{
			if (TextUpdated != null)
				TextUpdated (this, EventArgs.Empty);
		}
		
		void OnButtonsUpdated ()
		{
			if (ButtonsUpdated != null)
				ButtonsUpdated (this, EventArgs.Empty);
		}
		
		public AbstractTileObject ()
		{
			ExtraButtons = new List<Gtk.Button> ();
		}

		/// <summary>
		/// The icon for the tile
		/// </summary>
		string icon;
		public virtual string Icon {
			get {
				if (icon == null)
					icon = "";
				return icon;
			}
			protected set {
				if (icon == value)
					return;
				// if we set an icon, clear the forced pixbuf
				if (ForcePixbuf != null)
					ForcePixbuf = null;
				icon = value;
				OnIconUpdated ();
			}
		}
		
		/// <summary>
		/// The hue shift to be applied to the icon
		/// </summary>
		int? shift;
		public virtual int HueShift {
			get {
				if (!shift.HasValue)
				    shift = 0;
				return shift.Value;
			}
			protected set {
				if (shift.HasValue && shift.Value == value)
					return;
				shift = value;
				OnIconUpdated ();
			}
		}
		
		/// <summary>
		/// Manually specified pixbuf to be used instead of the icon
		/// Note: If set this will override the Icon property.
		/// </summary>
		Gdk.Pixbuf force_pbuf;
		public virtual Gdk.Pixbuf ForcePixbuf {
			get {
				return force_pbuf;
			}
			protected set {
				if (force_pbuf == value)
					return;
				force_pbuf = value;
				OnIconUpdated ();
			}
		}
		
		/// <summary>
		/// Description of this tile
		/// </summary>
		string desc;
		public virtual string Description {
			get { 
				if (desc == null)
					desc = "";
				return desc;
			}
			protected set {
				if (desc == value)
					return;
				desc = value;
				OnTextUpdated ();
			}
		}		
		
		/// <summary>
		/// Name of this tile
		/// </summary>
		string name;
		public virtual string Name {
			get { 
				if (name == null)
					name = "";
				return name;
			}
			protected set {
				if (name == value)
					return;
				name = value;
				OnTextUpdated ();
			}
		}		

		public virtual void OnActiveChanged ()
		{
		}
		
		/// <summary>
		/// Subdescription title
		/// </summary>
		string sub_desc_title;
		public virtual string SubDescriptionTitle {
			get { 
				if (sub_desc_title == null)
					sub_desc_title = "";
				return sub_desc_title;
			}
			protected set {
				if (sub_desc_title == value)
					return;
				sub_desc_title = value;
				OnTextUpdated ();
			}
		}
		
		/// <summary>
		/// Subdescription text
		/// </summary>
		string sub_desc_text;
		public virtual string SubDescriptionText {
			get { 
				if (sub_desc_text == null)
					sub_desc_text = "";
				return sub_desc_text;
			}
			protected set {
				if (sub_desc_text == value)
					return;
				sub_desc_text = value;
				OnTextUpdated ();
			}
		}
		
		/// <summary>
		/// Text shown on the button when the tile is enabled
		/// </summary>
		string enabled_text;
		public virtual string ButtonStateEnabledText {
			get { 
				if (enabled_text == null)
					enabled_text = Catalog.GetString ("_Remove");
				return enabled_text;
			}
			protected set {
				if (enabled_text == value)
					return;
				enabled_text = value;
			}
		}
	 
		/// <summary>
		/// Text shown on the button when the tile is disabled
		/// </summary>
		string disabled_text;
		public virtual string ButtonStateDisabledText {
			get { 
				if (disabled_text == null)
					disabled_text = Catalog.GetString ("_Add");
				return disabled_text;
			}
			protected set {
				if (disabled_text == value)
					return;
				disabled_text = value;
			}
		}
		
		/// <summary>
		/// Whether or not the button should be shown on the tile
		/// </summary>
		bool? show_button;
		public virtual bool ShowActionButton {
			get { 
				if (!show_button.HasValue)
					show_button = true;
				return show_button.Value;
			}
			protected set {
				if (show_button.HasValue && show_button.Value == value)
					return;
				show_button = value;
				OnButtonsUpdated ();
			}
		}
		
		/// <summary>
		/// State of the tile
		/// </summary>
		bool? enabled;
		public virtual bool Enabled {
			get { 
				if (!enabled.HasValue)
					enabled = false;
				return enabled.Value;
			}
			set {
				if (enabled.HasValue && enabled.Value == value)
					return;
				enabled = value;
			}
		}
		
		internal List<Gtk.Button> ExtraButtons;
		
		/// <summary>
		/// Adds extra buttons to the tile
		/// </summary>
		/// <param name="button">
		/// A <see cref="Gtk.Button"/>
		/// </param>
		public void AddUserButton (Gtk.Button button) {
			if (!ExtraButtons.Contains (button)) {
				ExtraButtons.Add (button);
				OnButtonsUpdated ();
			}
		}
		
		/// <summary>
		/// Removes extra buttons from the tile
		/// </summary>
		/// <param name="button">
		/// A <see cref="Gtk.Button"/>
		/// </param>
		public void RemoveUserButton (Gtk.Button button) {
			if (ExtraButtons.Contains (button)) {
				ExtraButtons.Remove (button);
				OnButtonsUpdated ();
			}
		}
	}
}
