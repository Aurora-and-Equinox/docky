//  
//  Copyright (C) 2009 Jason Smith, Robert Dyer
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

using Cairo;
using Gdk;
using GLib;
using Gnome;
using Gtk;
using Mono.Unix;

using Docky.Interface;
using Docky.Services;
using Docky.Widgets;
using Docky.Items;

namespace Docky
{
	enum Pages : uint {
		Docks = 0,
		Docklets,
		Helpers,
		NPages
	}
	
	enum HelperShowStates : uint {
		All = 0,
		Enabled,
		Disabled,
		NStates
	}
	
	enum DockletShowStates : uint {
		All = 0,
		Active,
		Disabled,
		NStates
	}
	
	public partial class ConfigurationWindow : Gtk.Window
	{
		string AutoStartKey = "Hidden";
		DesktopItem auto_start_item;
		TileView HelpersTileview, DockletsTileview;
		Widgets.SearchEntry HelperSearch, DockletSearch;
		
		static Dock activeDock;
		public static Dock ActiveDock {
			get { return activeDock; }
			private set {
				if (activeDock == value)
					return;
				
				if (activeDock != null && value != null)
					activeDock.UnsetActiveGlow ();
				
				activeDock = value;
				
				if (activeDock != null)
					activeDock.SetActiveGlow ();
			}
		}
		
		public ConfigurationWindow () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
			
			SkipTaskbarHint = true;
			
			int i = 0;
			foreach (string theme in Docky.Controller.DockThemes) {
				theme_combo.AppendText (theme);
				if (Docky.Controller.DockTheme == theme) {
					theme_combo.Active = i;
				}
				i++;
			}
			
			if (Docky.Controller.Docks.Count () == 1)
				ActiveDock = Docky.Controller.Docks.First ();
			
			SetupConfigAlignment ();
			
			start_with_computer_checkbutton.Active = IsAutoStartEnabled ();
			
			CheckButtons ();
			
			config_notebook.CurrentPage = (int) Pages.Docks;

			// setup docklets {
			DockletSearch = new SearchEntry ();
			DockletSearch.EmptyMessage = Catalog.GetString ("Search Docklets...");
			DockletSearch.InnerEntry.Changed += delegate {
				RefreshDocklets ();
			};
			DockletSearch.Ready = true;
			DockletSearch.Show ();
			hbox1.PackStart (DockletSearch, true, true, 2);
			
			DockletsTileview = new TileView ();
			DockletsTileview.IconSize = 48;
			docklet_scroll.AddWithViewport (DockletsTileview);
			// }
			
			// setup helpers {
			HelperSearch = new SearchEntry ();
			HelperSearch.EmptyMessage = Catalog.GetString ("Search Helpers...");
			HelperSearch.InnerEntry.Changed += delegate {
				RefreshHelpers ();
			};
			HelperSearch.Ready = true;
			HelperSearch.Show ();
			hbox5.PackStart (HelperSearch, true, true, 2);
			
			HelpersTileview = new TileView ();
			HelpersTileview.IconSize = 48;
			helper_scroll.AddWithViewport (HelpersTileview);
			
			DockServices.Helpers.HelperUninstalled += delegate {
				RefreshHelpers ();
			};
			// }
					
			ShowAll ();
		}
		
		protected override bool OnDeleteEvent (Event evnt)
		{
			Hide ();
			ActiveDock = null;
			SetupConfigAlignment ();
			return true;
		}

		protected virtual void OnCloseButtonClicked (object sender, System.EventArgs e)
		{
			Hide ();
			ActiveDock = null;
			SetupConfigAlignment ();
		}
		
		void SetupConfigAlignment ()
		{
			if (config_alignment.Child != null) {
				config_alignment.Remove (config_alignment.Child);
			}
			
			if (ActiveDock == null) {
				VBox vbox = new VBox ();
				
				HBox hboxTop = new HBox ();
				HBox hboxBottom = new HBox ();
				Label label1 = new Gtk.Label (Mono.Unix.Catalog.GetString ("Click on any dock to configure."));
				Label label2 = new Gtk.Label (Mono.Unix.Catalog.GetString ("Drag any dock to move it."));
				
				vbox.Add (hboxTop);
				vbox.Add (label1);
				vbox.Add (label2);
				vbox.Add (hboxBottom);
				
				vbox.SetChildPacking (hboxTop, true, true, 0, PackType.Start);
				vbox.SetChildPacking (label1, false, false, 0, PackType.Start);
				vbox.SetChildPacking (label2, false, false, 0, PackType.Start);
				vbox.SetChildPacking (hboxBottom, true, true, 0, PackType.Start);
				
				config_alignment.Add (vbox);
			} else {
				config_alignment.Add (ActiveDock.PreferencesWidget);
			}
			config_alignment.ShowAll ();
		}

		protected override void OnShown ()
		{
			foreach (Dock dock in Docky.Controller.Docks) {
				dock.EnterConfigurationMode ();
				dock.ConfigurationClick += HandleDockConfigurationClick;
			}
			
			if (Docky.Controller.Docks.Count () == 1) {
				ActiveDock = Docky.Controller.Docks.First ();
				SetupConfigAlignment ();
			}
			
			KeepAbove = true;
			Stick ();

			base.OnShown ();
		}

		void HandleDockConfigurationClick (object sender, EventArgs e)
		{
			Dock dock = sender as Dock;
			
			if (ActiveDock != dock) {
				ActiveDock = dock;
				SetupConfigAlignment ();
				CheckButtons ();
				if (config_notebook.Page == (int)Pages.Docklets)
					RefreshDocklets ();
			}
		}

		protected override void OnHidden ()
		{
			foreach (Dock dock in Docky.Controller.Docks) {
				dock.ConfigurationClick -= HandleDockConfigurationClick;
				dock.LeaveConfigurationMode ();
				dock.UnsetActiveGlow ();
			}
			base.OnHidden ();
		}

		protected virtual void OnThemeComboChanged (object sender, System.EventArgs e)
		{
			Docky.Controller.DockTheme = theme_combo.ActiveText;
			if (Docky.Controller.NumDocks == 1) {
				ActiveDock = null;
				SetupConfigAlignment ();
				CheckButtons ();
			}
		}
	
		protected virtual void OnDeleteDockButtonClicked (object sender, System.EventArgs e)
		{
			if (!(Docky.Controller.Docks.Count () > 1))
				return;
			
			if (ActiveDock != null) {
				Gtk.MessageDialog md = new Gtk.MessageDialog (null, 
						  0,
						  Gtk.MessageType.Warning, 
						  Gtk.ButtonsType.None,
						  "<b><big>" + Catalog.GetString ("Delete the currently selected dock?") + "</big></b>");
				md.Icon = DockServices.Drawing.LoadIcon ("docky", 22);
				md.SecondaryText = Catalog.GetString ("If you choose to delete the dock, all settings\n" +
					"for the deleted dock will be permanently lost.");
				md.Modal = true;
				md.KeepAbove = true;
				md.Stick ();
				
				Gtk.Button cancel_button = new Gtk.Button();
				cancel_button.CanFocus = true;
				cancel_button.CanDefault = true;
				cancel_button.Name = "cancel_button";
				cancel_button.UseStock = true;
				cancel_button.UseUnderline = true;
				cancel_button.Label = "gtk-cancel";
				cancel_button.Show ();
				md.AddActionWidget (cancel_button, Gtk.ResponseType.Cancel);
				md.AddButton (Catalog.GetString ("_Delete Dock"), Gtk.ResponseType.Ok);
				md.DefaultResponse = Gtk.ResponseType.Cancel;
			
				if ((ResponseType)md.Run () == Gtk.ResponseType.Ok) {
					Docky.Controller.DeleteDock (ActiveDock);
					if (Docky.Controller.Docks.Count () == 1)
						ActiveDock = Docky.Controller.Docks.First ();
					else
						ActiveDock = null;
					SetupConfigAlignment ();
				}
				
				md.Destroy ();
				
			}
			CheckButtons ();
		}
		
		protected virtual void OnNewDockButtonClicked (object sender, System.EventArgs e)
		{
			Dock newDock = Docky.Controller.CreateDock ();
			
			if (newDock != null) {
				newDock.ConfigurationClick += HandleDockConfigurationClick;
				newDock.EnterConfigurationMode ();
				ActiveDock = newDock;
				SetupConfigAlignment ();
			}
			CheckButtons ();
		}
		
		void CheckButtons ()
		{
			int spotsAvailable = 0;
			for (int i = 0; i < Screen.Default.NMonitors; i++)
				spotsAvailable += Docky.Controller.PositionsAvailableForDock (i).Count ();
			
			delete_dock_button.Sensitive = (Docky.Controller.Docks.Count () == 1 || ActiveDock == null) ? false : true;
			new_dock_button.Sensitive = (spotsAvailable == 0) ? false : true;
		}

		string AutoStartDir {
			get { return System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "autostart"); }
		}

		string AutoStartFileName {
			get { return System.IO.Path.Combine (AutoStartDir, "docky.desktop"); }
		}
		
		DesktopItem AutoStartItem {
			get {
				if (auto_start_item != null)
					return auto_start_item;
				
				GLib.File file = DockServices.Paths.AutoStartFile;
				
				try {
					auto_start_item = DesktopItem.NewFromFile (file.Path, DesktopItemLoadFlags.NoTranslations);
				} catch (GLib.GException loadException) {
					Log<ConfigurationWindow>.Info ("Unable to load existing autostart file: {0}", loadException.Message);
					Log<ConfigurationWindow>.Info ("Writing new autostart file to {0}", file.Path);
					auto_start_item = DesktopItem.NewFromFile (System.IO.Path.Combine (AssemblyInfo.InstallData, "applications/docky.desktop"), DesktopItemLoadFlags.NoTranslations);
					try {
						if (!file.Parent.Exists)
							file.Parent.MakeDirectoryWithParents (null);						
						
						auto_start_item.Save (file.StringUri (), true);
						auto_start_item.Location = file.StringUri ();
					} catch (Exception e) {
						Log<ConfigurationWindow>.Error ("Failed to write initial autostart file: {0}", e.Message);
					}
				}
				return auto_start_item;
			}
		}

		bool IsAutoStartEnabled ()
		{
			if (!AutoStartItem.Exists ()) {
				Log<SystemService>.Error ("Could not open autostart file {0}", DockServices.Paths.AutoStartFile.Path);
			}
			
			if (AutoStartItem.AttrExists (AutoStartKey)) {
				return !String.Equals (AutoStartItem.GetString (AutoStartKey), "true", StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}

		void SetAutoStartEnabled (bool enabled)
		{
			AutoStartItem.SetBoolean (AutoStartKey, !enabled);
			try {
				AutoStartItem.Save (null, true);
			} catch (Exception e) {
				Log<SystemService>.Error ("Failed to update autostart file: {0}", e.Message);
			}
		}
		
		protected virtual void OnStartWithComputerCheckbuttonToggled (object sender, System.EventArgs e)
		{
			SetAutoStartEnabled (start_with_computer_checkbutton.Active);
		}

		[GLib.ConnectBefore]
		protected virtual void OnPageSwitch (object o, Gtk.SwitchPageArgs args)
		{
			if (args.PageNum == (uint) Pages.Helpers)
				RefreshHelpers ();
			else if (args.PageNum ==  (uint) Pages.Docklets)
				RefreshDocklets ();
		}

		protected virtual void OnInstallClicked (object sender, System.EventArgs e)
		{
			GLib.File file = null;
			Gtk.FileChooserDialog script_chooser = new Gtk.FileChooserDialog ("Helpers", this, FileChooserAction.Open, Gtk.Stock.Cancel, ResponseType.Cancel, Catalog.GetString ("_Select"), ResponseType.Ok);
			FileFilter filter = new FileFilter ();
			filter.AddPattern ("*.tar");
			filter.Name = Catalog.GetString (".tar Archives");
			script_chooser.AddFilter (filter);
			
			if ((ResponseType) script_chooser.Run () == ResponseType.Ok)
				file = GLib.FileFactory.NewForPath (script_chooser.Filename);

			script_chooser.Destroy ();
			
			if (file == null)
				return;
			
			Helper installedHelper;
			if (DockServices.Helpers.InstallHelper (file.Path, out installedHelper)) {
				installedHelper.Data.DataReady += delegate {
					RefreshHelpers ();
				};
			}
		}

		protected virtual void OnShowHelperChanged (object sender, System.EventArgs e)
		{
			RefreshHelpers ();
		}
		
		protected virtual void OnShowDockletChanged (object sender, System.EventArgs e)
		{
			RefreshDocklets ();
		}
		
		void RefreshHelpers ()
		{
			string query = HelperSearch.InnerEntry.Text.ToLower ();
			IEnumerable<HelperTile> tiles = DockServices.Helpers.Helpers.Select (h => new HelperTile (h))
				.Where (h => h.Name.ToLower ().Contains (query) || h.Description.ToLower ().Contains (query))
				.OrderBy (t => t.Name);
			
			if (helper_show_cmb.Active == (uint) HelperShowStates.Enabled)
				tiles = tiles.Where (h => h.Enabled);
			else if (helper_show_cmb.Active == (uint) HelperShowStates.Disabled)
				tiles = tiles.Where (h => !h.Enabled);
			
			HelpersTileview.Clear ();
			foreach (HelperTile helper in tiles) {
				HelpersTileview.AppendTile (helper);
			}
		}
		
		void RefreshDocklets ()
		{
			if (ActiveDock == null)
				return;
			
			string query = DockletSearch.InnerEntry.Text.ToLower ();
			// build a list of DockletTiles, starting with the currently active tiles for the active dock,
			// and the available addins
			List<DockletTile> tiles = new List<DockletTile> ();
			
			foreach (AbstractDockItemProvider provider in ActiveDock.Preferences.ItemProviders) {
				string providerID = PluginManager.AddinIDFromProvider (provider);
				if (string.IsNullOrEmpty (providerID))
				    continue;

				tiles.Add (new DockletTile (providerID, provider));
			}
			
			tiles = tiles.Concat (PluginManager.AvailableProviderIDs.Select (id => new DockletTile (id))).ToList ();
			
			if (docklet_show_cmb.Active == (int) DockletShowStates.Active)
				tiles = tiles.Where (t => t.Enabled).ToList ();
			else if (docklet_show_cmb.Active == (int) DockletShowStates.Disabled)
				tiles = tiles.Where (t => !t.Enabled).ToList ();
			
			tiles = tiles.Where (t => t.Description.ToLower ().Contains (query) || t.Name.ToLower ().Contains (query)).ToList ();
			
			DockletsTileview.Clear ();
			foreach (DockletTile docklet in tiles) {
				DockletsTileview.AppendTile (docklet);
			}
		}
	}
}
