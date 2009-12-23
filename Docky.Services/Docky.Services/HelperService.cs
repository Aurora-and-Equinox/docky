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
using IO = System.IO;
using System.Linq;
using System.Collections.Generic;

using GLib;

using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Tar;

namespace Docky.Services
{

	public class HelperService
	{
		public static File UserScriptsDir = GLib.FileFactory.NewForPath (DockServices.System.UserDataFolder).GetChild ("helpers");
		public static File UserMetaDir = HelperService.UserScriptsDir.GetChild ("metadata");
		public static File SysScriptsDir = GLib.FileFactory.NewForPath (DockServices.System.SystemDataFolder).GetChild ("helpers");
		public static File SysMetaDir = HelperService.SysScriptsDir.GetChild ("metadata");
		
		IEnumerable<GLib.File> HelperDirs = new [] {
			UserScriptsDir,
			SysScriptsDir,
		}.Where (dir => dir.Exists);
		
		public event EventHandler<HelperStatusChangedEventArgs> HelperStatusChanged;
		public event EventHandler HelperUninstalled;
		
		public bool ShowOutput {
			get {
				return prefs.Get<bool> ("ShowOutput", true);
			}
			set {
				if (ShowOutput == value)
					return;
				prefs.Set<bool> ("ShowOutput", value);
			}
		}

		IPreferences prefs;
		public List<Helper> Helpers { get; private set; }
		
		public HelperService ()
		{
			prefs = DockServices.Preferences.Get<HelperService> ();
			Helpers = new List<Helper> ();
			
			GLib.Timeout.Add (2000, delegate {
				Refresh ();
				return false;
			});
		}
		
		void Refresh ()
		{
			// fetch a list of helpers
			FetchHelpers ();
			
			// set up the file monitors to watch our script directories
			SetupDirMonitors ();
		}

		void SetupDirMonitors ()
		{
			foreach (File dir in HelperDirs) {
				FileMonitor mon = dir.Monitor (0, null);
				mon.RateLimit = 5000;
				mon.Changed += delegate(object o, ChangedArgs args) {
					FetchHelpers ();
				};
			}
		}

		void FetchHelpers ()
		{			
			Helpers = HelperDirs
				.SelectMany (d => d.GetFiles (""))
				.Where (file => !file.Basename.EndsWith ("~"))
				.Select (hf => LookupHelper (hf))
				.ToList ();
			
			foreach (Helper s in Helpers) {
				s.HelperStatusChanged += delegate(object sender, HelperStatusChangedEventArgs e) {
					Console.WriteLine ("Helper event: {0} {1} {2}", e.File.Basename, e.Enabled, e.IsRunning);
					OnHelperStatusChanged (e);
				};
			}
		}
		
		void OnHelperStatusChanged (HelperStatusChangedEventArgs args)
		{
			if (HelperStatusChanged != null)
				HelperStatusChanged (this, args);
		}
		
		void OnHelperDeleted ()
		{
			if (HelperUninstalled != null)
				HelperUninstalled (this, EventArgs.Empty);
		}
		
		Helper LookupHelper (File helperFile)
		{
			if (!Helpers.Any (h => h.File.Path == helperFile.Path))
				Helpers.Add (new Helper (helperFile));
			
			return Helpers.First (h => h.File.Path == helperFile.Path);
		}
		
		public bool InstallHelper (string path, out Helper installedHelper)
		{
			File file = FileFactory.NewForPath (path);
			
			// WARN: This is _only_ valid if this method returns true
			installedHelper = null;
			
			if (!file.Exists)
				return false;
			if (!UserScriptsDir.Exists)
				UserScriptsDir.MakeDirectory (null);
			if (!UserMetaDir.Exists)
				UserMetaDir.MakeDirectory (null);
			
			Log<HelperService>.Info ("Trying to install: {0}", file.Path);
			
			try {
				TarArchive ar = TarArchive.CreateInputTarArchive (new IO.FileStream (file.Path, IO.FileMode.Open));
				ar.ExtractContents (UserScriptsDir.Path);
			} catch (Exception e) {
				Log<HelperService>.Error ("Error trying to unpack '{0}': {1}", file.Path, e.Message);
				Log<HelperService>.Debug (e.StackTrace);
				return false;
			}
			
			try {
				List<Helper> currentHelpers = Helpers.ToList ();
				Refresh ();
				installedHelper = Helpers.Except (currentHelpers).First ();
				return true;
			} catch (Exception e) {
				Log<HelperService>.Error ("Error trying to install helper '{0}': {1}", file.Path, e.Message);
				Log<HelperService>.Debug (e.StackTrace);
			}
			
			return false;
		}
		
		public bool UninstallHelper (Helper helper)
		{
			Log<HelperService>.Info ("Trying to unininstall: {0}", helper.File.Path);
			
			try {
				helper.File.Delete ();
				if (helper.Data != null) {
					if (helper.Data.DataFile.Exists)
						helper.Data.DataFile.Delete ();
					if (helper.Data.IconFile != null && helper.Data.IconFile.Exists)
						helper.Data.IconFile.Delete ();
				}
				Refresh ();
				OnHelperDeleted ();
				return true;
			} catch (Exception e) {
				Log<HelperService>.Error ("Error trying to uninstall helper '{0}': {1}", helper.File.Path, e.Message);
				Log<HelperService>.Debug (e.StackTrace);
			}
			
			return false;
		}
	}
}
