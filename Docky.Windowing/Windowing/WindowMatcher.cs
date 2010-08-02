//  
//  Copyright (C) 2009-2010 Jason Smith, Robert Dyer, Chris Szikszoy, 
//                          Rico Tzschichholz
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
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

using Wnck;

using Docky.Services;
using Docky.Services.Applications;

namespace Docky.Windowing
{
	public class WindowMatcher
	{
		
		static WindowMatcher ()
		{
			Default = new WindowMatcher ();
		}
		
		public static WindowMatcher Default { get; protected set; }
		
		private WindowMatcher ()
		{
			// Initialize window matching with currently available windows
			window_to_desktop_items = new Dictionary<Wnck.Window, List<DesktopItem>> ();

			foreach (Wnck.Window w in Wnck.Screen.Default.Windows)
				SetupWindow (w);

			Wnck.Screen.Default.WindowOpened += WnckScreenDefaultWindowOpened;
			Wnck.Screen.Default.WindowClosed += WnckScreenDefaultWindowClosed;
			
			Log<WindowMatcher>.Debug ("WindowMatcher initialized.");
		}

		
		IEnumerable<Wnck.Window> UnmatchedWindows {
			get {
				IEnumerable<Wnck.Window> matched = window_to_desktop_items.Keys.Cast<Wnck.Window> ();
				return Wnck.Screen.Default.Windows.Where (w => !w.IsSkipTasklist && !matched.Contains (w));
			}
		}
		
		Dictionary<Wnck.Window, List<DesktopItem>> window_to_desktop_items;
		
		#region Window Setup
		void WnckScreenDefaultWindowOpened (object o, WindowOpenedArgs args)
		{
			SetupWindow (args.Window);
		}

		void WnckScreenDefaultWindowClosed (object o, WindowClosedArgs args)
		{
			if (args.Window != null)
				window_to_desktop_items.Remove (args.Window);
		}
		
		bool SetupWindow (Wnck.Window window)
		{
			IEnumerable<DesktopItem> items = DesktopItemsForWindow (window);
			if (items.Any ()) {
				window_to_desktop_items [window] = items.ToList ();
				return true;
			} else {
				return false;
			}
		}
		#endregion

		#region Window Matching
		readonly List<Regex> prefix_filters;
		readonly List<Regex> suffix_filters;
		
		/* exposed via DockServices.WindowMatching */
		static IEnumerable<string> PrefixStrings {
			get {
				yield return "gksu(do)?";
				yield return "sudo";
				yield return "java";
				yield return "mono";
				yield return "ruby";
				yield return "padsp";
				yield return "perl";
				yield return "aoss";
				yield return "python(\\d+.\\d+)?";
				yield return "wish(\\d+\\.\\d+)?";
				yield return "(ba)?sh";
				yield return "-.*";
				yield return "*.\\.desktop";
			}
		}
		
		List<Regex> BuildPrefixFilters ()
		{
			return new List<Regex> (PrefixStrings.Select (s => new Regex ("^" + s + "$")));
		}
		
		/* exposed via DockServices.WindowMatching */
		static IEnumerable<string> SuffixStrings {
			get {
				// some wine apps are launched via a shell script that sets the proc name to "app.exe"
				yield return "\\.exe";
				// some apps have a script 'foo' which does 'exec foo-bin' or 'exec foo.bin'
				yield return "[.-]bin";
				// some python apps have a script 'foo' for 'python foo.py'
				yield return "\\.py";
				// some apps append versions, such as '-1' or '-3.0'
				yield return "(-)?\\d+(\\.\\d+)?";
			}
		}

		List<Regex> BuildSuffixFilters ()
		{
			return new List<Regex> (SuffixStrings.Select (s => new Regex (s + "$")));
		}

		public bool WindowIsReadyForMatch (Wnck.Window window)
		{
			if (!WindowIsOpenOffice (window))
				return true;

			return SetupWindow (window);
		}
		
		public bool WindowIsOpenOffice (Wnck.Window window)
		{
			return window.ClassGroup != null && window.ClassGroup.Name.ToLower ().StartsWith ("openoffice");
		}
		
		public IEnumerable<Wnck.Window> WindowsForDesktopItem (DesktopItem item)
		{
			if (item == null)
				throw new ArgumentNullException ("DesktopItem item");
			
			foreach (KeyValuePair<Wnck.Window, List<DesktopItem>> kvp in window_to_desktop_items)
				if (kvp.Value.Any (df => df == item))
					yield return kvp.Key;
		}
		
		public IEnumerable<Wnck.Window> SimilarWindows (Wnck.Window window)
		{
			if (window == null)
				throw new ArgumentNullException ("Wnck.Window window");
			
			//TODO perhaps make it a bit smarter
			if (!window_to_desktop_items.ContainsKey (window))
				foreach (Wnck.Window win in UnmatchedWindows) {
					if (win == window)
						continue;
					
					if (win.Pid == window.Pid)
						yield return win;
					else if (window.Pid <= 1) {
						if (window.ClassGroup != null
								&& win.ClassGroup != null
								&& !string.IsNullOrEmpty (window.ClassGroup.ResClass)
								&& !string.IsNullOrEmpty (win.ClassGroup.ResClass)
								&& win.ClassGroup.ResClass.Equals (window.ClassGroup.ResClass))
							yield return win;
						else if (!string.IsNullOrEmpty (win.Name) && win.Name.Equals (window.Name)) 
							yield return win;
					}
				}
			
			yield return window;
		}

		
		public DesktopItem DesktopItemForWindow (Wnck.Window window)
		{
			if (window == null)
				throw new ArgumentNullException ("window");
			
			List<DesktopItem> matches;
			if (window_to_desktop_items.TryGetValue (window, out matches)) {
				DesktopItem useritem = matches.Find (item => item.File.Path.StartsWith (DockServices.Paths.HomeFolder.Path));
				if (useritem != null)
					return useritem;
				return matches.FirstOrDefault ();
			}
			
			return null;
		}
		
		IEnumerable<DesktopItem> DesktopItemsForWindow (Wnck.Window window)
		{
			// use the StartupWMClass as the definitive match
			if (window.ClassGroup != null
					&& !string.IsNullOrEmpty (window.ClassGroup.ResClass)
					&& window.ClassGroup.ResClass != "Wine"
					&& class_to_desktop_items.ContainsKey (window.ClassGroup.ResClass)) {
				yield return class_to_desktop_items [window.ClassGroup.ResClass];
				yield break;
			}
			
			int pid = window.Pid;
			if (pid <= 1) {
				if (window.ClassGroup != null && !string.IsNullOrEmpty (window.ClassGroup.ResClass)) {
					IEnumerable<DesktopItem> matches = DesktopItemsForDesktopID (window.ClassGroup.ResClass);
					if (matches.Any ())
						foreach (DesktopItem s in matches)
							yield return s;
				}
				yield break;
			}
			
			bool matched = false;
			int currentPid = 0;
			
			// get ppid and parents
			IEnumerable<int> pids = PidAndParents (pid);
			// this list holds a list of the command line parts from left (0) to right (n)
			List<string> command_line = new List<string> ();
			
			// if we have a classname that matches a desktopid we have a winner
			if (window.ClassGroup != null) {
				if (WindowIsOpenOffice (window)) {
					string title = window.Name.Trim ();
					if (title.EndsWith ("Writer"))
						command_line.Add ("ooffice-writer");
					else if (title.EndsWith ("Draw"))
						command_line.Add ("ooffice-draw");
					else if (title.EndsWith ("Impress"))
						command_line.Add ("ooffice-impress");
					else if (title.EndsWith ("Calc"))
						command_line.Add ("ooffice-calc");
					else if (title.EndsWith ("Math"))
						command_line.Add ("ooffice-math");
				} else if (window.ClassGroup.ResClass == "Wine") {
					// we can match Wine apps normally so don't do anything here
				} else {
					string class_name = window.ClassGroup.ResClass.Replace (".", "");
					IEnumerable<DesktopItem> matches = DesktopItemsForDesktopID (class_name);
					
					foreach (DesktopItem s in matches) {
						yield return s;
						matched = true;
					}
				}
			}
	
			lock (update_lock) {
				do {
					// do a match on the process name
					string name = NameForPid (pids.ElementAt (currentPid));
					if (exec_to_desktop_items.ContainsKey (name)) {
						foreach (DesktopItem s in exec_to_desktop_items[name]) {
							yield return s;
							matched = true;
						}
					}
					
					// otherwise do a match on the commandline
					command_line.AddRange (CommandLineForPid (pids.ElementAt (currentPid++))
						.Select (cmd => cmd.Replace (@"\", @"\\")));
					
					if (command_line.Count () == 0)
						continue;
					
					foreach (string cmd in command_line) {
						if (exec_to_desktop_items.ContainsKey (cmd)) {
							foreach (DesktopItem s in exec_to_desktop_items[cmd]) {
								yield return s;
								matched = true;
							}
						}
						if (matched)
							break;
					}
					
					// if we found a match, bail.
					if (matched)
						yield break;
				} while (currentPid < pids.Count ());
			}
			command_line.Clear ();
			yield break;
		}
		
		IEnumerable<int> PidAndParents (int pid)
		{
			string cmdline;

			do {
				yield return pid;
				
				try {
					string procPath = new [] { "/proc", pid.ToString (), "stat" }.Aggregate (Path.Combine);
					using (StreamReader reader = new StreamReader (procPath)) {
						cmdline = reader.ReadLine ();
						reader.Close ();
					}
				} catch { 
					yield break; 
				}
				
				if (cmdline == null)
					yield break;
				
				string [] result = cmdline.Split (Convert.ToChar (0x0)) [0].Split (' ');

				if (result.Count () < 4)
					yield break;
				
				// the ppid is index number 3
				if (!int.TryParse (result [3], out pid))
					yield break;
			} while (pid > 1);
		}
		
		IEnumerable<string> CommandLineForPid (int pid)
		{
			string cmdline;

			try {
				string procPath = new [] { "/proc", pid.ToString (), "cmdline" }.Aggregate (Path.Combine);
				using (StreamReader reader = new StreamReader (procPath)) {
					cmdline = reader.ReadLine ();
					reader.Close ();
				}
			} catch { yield break; }
			
			if (cmdline == null)
				yield break;
			
			cmdline = cmdline.Trim ();
						
			string [] result = cmdline.Split (Convert.ToChar (0x0));
			
			// these are sanitized results
			foreach (string sanitizedCmd in result
				.Select (s => s.Split (new []{'/', '\\'}).Last ())
			    .Distinct ()
				.Where (s => !string.IsNullOrEmpty (s) && !prefix_filters.Any (f => f.IsMatch (s)))) {
				
				yield return sanitizedCmd;
				
				if (DockServices.DesktopItems.Remaps.ContainsKey (sanitizedCmd))
					yield return DockServices.DesktopItems.Remaps [sanitizedCmd];
				
				// if it ends with a special suffix, strip the suffix and return an additional result
				foreach (Regex f in suffix_filters)
					if (f.IsMatch (sanitizedCmd))
						yield return f.Replace (sanitizedCmd, "");
			}
			
			// return the entire cmdline last as a last ditch effort to find a match
			yield return cmdline;
		}
		
		string NameForPid (int pid)
		{
			string name;

			try {
				string procPath = new [] { "/proc", pid.ToString (), "status" }.Aggregate (Path.Combine);
				using (StreamReader reader = new StreamReader (procPath)) {
					name = reader.ReadLine ();
					reader.Close ();
				}
			} catch { return ""; }
			
			if (string.IsNullOrEmpty (name) || !name.StartsWith ("Name:"))
				return "";
			
			return name.Substring (6);
		}
		#endregion
	}
}
