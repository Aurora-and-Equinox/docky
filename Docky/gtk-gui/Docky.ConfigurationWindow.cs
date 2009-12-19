// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Docky {
    
    
    public partial class ConfigurationWindow {
        
        private Gtk.VBox vbox1;
        
        private Gtk.Notebook notebook1;
        
        private Gtk.VBox vbox2;
        
        private Gtk.Alignment alignment1;
        
        private Gtk.VBox vbox6;
        
        private Gtk.Frame frame1;
        
        private Gtk.Alignment GtkAlignment;
        
        private Gtk.VBox vbox3;
        
        private Gtk.CheckButton start_with_computer_checkbutton;
        
        private Gtk.HBox hbox4;
        
        private Gtk.Label label3;
        
        private Gtk.ComboBox theme_combo;
        
        private Gtk.Label GtkLabel5;
        
        private Gtk.Frame frame3;
        
        private Gtk.Alignment config_alignment;
        
        private Gtk.Label GtkLabel6;
        
        private Gtk.Label label1;
        
        private Gtk.Alignment alignment3;
        
        private Gtk.VBox vbox5;
        
        private Gtk.ScrolledWindow scrolledwindow1;
        
        private Gtk.Label label2;
        
        private Gtk.HBox hbox2;
        
        private Gtk.HBox hbox3;
        
        private Gtk.Button new_dock_button;
        
        private Gtk.Button delete_dock_button;
        
        private Gtk.VBox vbox4;
        
        private Gtk.Button close_button;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget Docky.ConfigurationWindow
            this.Name = "Docky.ConfigurationWindow";
            this.Title = Mono.Unix.Catalog.GetString("Docky Configuration");
            this.Icon = Stetic.IconLoader.LoadIcon(this, "gtk-preferences", Gtk.IconSize.Menu, 16);
            this.TypeHint = ((Gdk.WindowTypeHint)(1));
            this.WindowPosition = ((Gtk.WindowPosition)(1));
            this.BorderWidth = ((uint)(2));
            // Container child Docky.ConfigurationWindow.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            // Container child vbox1.Gtk.Box+BoxChild
            this.notebook1 = new Gtk.Notebook();
            this.notebook1.CanFocus = true;
            this.notebook1.Name = "notebook1";
            this.notebook1.CurrentPage = 1;
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.alignment1 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment1.Name = "alignment1";
            this.alignment1.LeftPadding = ((uint)(7));
            this.alignment1.TopPadding = ((uint)(7));
            this.alignment1.RightPadding = ((uint)(7));
            this.alignment1.BottomPadding = ((uint)(7));
            // Container child alignment1.Gtk.Container+ContainerChild
            this.vbox6 = new Gtk.VBox();
            this.vbox6.Name = "vbox6";
            this.vbox6.Spacing = 6;
            // Container child vbox6.Gtk.Box+BoxChild
            this.frame1 = new Gtk.Frame();
            this.frame1.Name = "frame1";
            this.frame1.ShadowType = ((Gtk.ShadowType)(2));
            // Container child frame1.Gtk.Container+ContainerChild
            this.GtkAlignment = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.GtkAlignment.Name = "GtkAlignment";
            this.GtkAlignment.LeftPadding = ((uint)(12));
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.start_with_computer_checkbutton = new Gtk.CheckButton();
            this.start_with_computer_checkbutton.CanFocus = true;
            this.start_with_computer_checkbutton.Name = "start_with_computer_checkbutton";
            this.start_with_computer_checkbutton.Label = Mono.Unix.Catalog.GetString("_Start When Computer Starts");
            this.start_with_computer_checkbutton.DrawIndicator = true;
            this.start_with_computer_checkbutton.UseUnderline = true;
            this.vbox3.Add(this.start_with_computer_checkbutton);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.vbox3[this.start_with_computer_checkbutton]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Spacing = 6;
            // Container child hbox4.Gtk.Box+BoxChild
            this.label3 = new Gtk.Label();
            this.label3.CanFocus = true;
            this.label3.Name = "label3";
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("_Theme:");
            this.label3.UseUnderline = true;
            this.hbox4.Add(this.label3);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox4[this.label3]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.theme_combo = Gtk.ComboBox.NewText();
            this.theme_combo.Name = "theme_combo";
            this.hbox4.Add(this.theme_combo);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox4[this.theme_combo]));
            w3.Position = 1;
            w3.Expand = false;
            w3.Fill = false;
            this.vbox3.Add(this.hbox4);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox3[this.hbox4]));
            w4.Position = 1;
            w4.Expand = false;
            w4.Fill = false;
            this.GtkAlignment.Add(this.vbox3);
            this.frame1.Add(this.GtkAlignment);
            this.GtkLabel5 = new Gtk.Label();
            this.GtkLabel5.Name = "GtkLabel5";
            this.GtkLabel5.LabelProp = Mono.Unix.Catalog.GetString("<b>General Options</b>");
            this.GtkLabel5.UseMarkup = true;
            this.frame1.LabelWidget = this.GtkLabel5;
            this.vbox6.Add(this.frame1);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.vbox6[this.frame1]));
            w7.Position = 0;
            w7.Expand = false;
            w7.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.frame3 = new Gtk.Frame();
            this.frame3.Name = "frame3";
            this.frame3.ShadowType = ((Gtk.ShadowType)(2));
            // Container child frame3.Gtk.Container+ContainerChild
            this.config_alignment = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.config_alignment.Name = "config_alignment";
            this.config_alignment.LeftPadding = ((uint)(12));
            this.config_alignment.RightPadding = ((uint)(10));
            this.config_alignment.BottomPadding = ((uint)(10));
            this.frame3.Add(this.config_alignment);
            this.GtkLabel6 = new Gtk.Label();
            this.GtkLabel6.Name = "GtkLabel6";
            this.GtkLabel6.LabelProp = Mono.Unix.Catalog.GetString("<b>Dock Configuration</b>");
            this.GtkLabel6.UseMarkup = true;
            this.frame3.LabelWidget = this.GtkLabel6;
            this.vbox6.Add(this.frame3);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.vbox6[this.frame3]));
            w9.Position = 1;
            this.alignment1.Add(this.vbox6);
            this.vbox2.Add(this.alignment1);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.vbox2[this.alignment1]));
            w11.Position = 0;
            this.notebook1.Add(this.vbox2);
            // Notebook tab
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Docks");
            this.notebook1.SetTabLabel(this.vbox2, this.label1);
            this.label1.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.alignment3 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment3.Name = "alignment3";
            this.alignment3.LeftPadding = ((uint)(7));
            this.alignment3.TopPadding = ((uint)(7));
            this.alignment3.RightPadding = ((uint)(7));
            this.alignment3.BottomPadding = ((uint)(7));
            this.alignment3.BorderWidth = ((uint)(7));
            // Container child alignment3.Gtk.Container+ContainerChild
            this.vbox5 = new Gtk.VBox();
            this.vbox5.Name = "vbox5";
            this.vbox5.Spacing = 6;
            // Container child vbox5.Gtk.Box+BoxChild
            this.scrolledwindow1 = new Gtk.ScrolledWindow();
            this.scrolledwindow1.CanFocus = true;
            this.scrolledwindow1.Name = "scrolledwindow1";
            this.scrolledwindow1.HscrollbarPolicy = ((Gtk.PolicyType)(2));
            this.scrolledwindow1.ShadowType = ((Gtk.ShadowType)(1));
            this.vbox5.Add(this.scrolledwindow1);
            Gtk.Box.BoxChild w13 = ((Gtk.Box.BoxChild)(this.vbox5[this.scrolledwindow1]));
            w13.Position = 1;
            this.alignment3.Add(this.vbox5);
            this.notebook1.Add(this.alignment3);
            Gtk.Notebook.NotebookChild w15 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.alignment3]));
            w15.Position = 1;
            // Notebook tab
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Scripts");
            this.notebook1.SetTabLabel(this.alignment3, this.label2);
            this.label2.ShowAll();
            this.vbox1.Add(this.notebook1);
            Gtk.Box.BoxChild w16 = ((Gtk.Box.BoxChild)(this.vbox1[this.notebook1]));
            w16.Position = 0;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.new_dock_button = new Gtk.Button();
            this.new_dock_button.CanFocus = true;
            this.new_dock_button.Name = "new_dock_button";
            this.new_dock_button.UseUnderline = true;
            this.new_dock_button.BorderWidth = ((uint)(5));
            // Container child new_dock_button.Gtk.Container+ContainerChild
            Gtk.Alignment w17 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w18 = new Gtk.HBox();
            w18.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w19 = new Gtk.Image();
            w19.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-add", Gtk.IconSize.Menu, 16);
            w18.Add(w19);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w21 = new Gtk.Label();
            w21.LabelProp = Mono.Unix.Catalog.GetString("_New Dock");
            w21.UseUnderline = true;
            w18.Add(w21);
            w17.Add(w18);
            this.new_dock_button.Add(w17);
            this.hbox3.Add(this.new_dock_button);
            Gtk.Box.BoxChild w25 = ((Gtk.Box.BoxChild)(this.hbox3[this.new_dock_button]));
            w25.Position = 0;
            w25.Expand = false;
            w25.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.delete_dock_button = new Gtk.Button();
            this.delete_dock_button.CanFocus = true;
            this.delete_dock_button.Name = "delete_dock_button";
            this.delete_dock_button.UseUnderline = true;
            this.delete_dock_button.BorderWidth = ((uint)(5));
            // Container child delete_dock_button.Gtk.Container+ContainerChild
            Gtk.Alignment w26 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w27 = new Gtk.HBox();
            w27.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w28 = new Gtk.Image();
            w28.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-delete", Gtk.IconSize.Menu, 16);
            w27.Add(w28);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w30 = new Gtk.Label();
            w30.LabelProp = Mono.Unix.Catalog.GetString("_Delete Dock");
            w30.UseUnderline = true;
            w27.Add(w30);
            w26.Add(w27);
            this.delete_dock_button.Add(w26);
            this.hbox3.Add(this.delete_dock_button);
            Gtk.Box.BoxChild w34 = ((Gtk.Box.BoxChild)(this.hbox3[this.delete_dock_button]));
            w34.Position = 1;
            w34.Expand = false;
            w34.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.vbox4 = new Gtk.VBox();
            this.vbox4.Name = "vbox4";
            this.vbox4.Spacing = 6;
            this.hbox3.Add(this.vbox4);
            Gtk.Box.BoxChild w35 = ((Gtk.Box.BoxChild)(this.hbox3[this.vbox4]));
            w35.Position = 2;
            // Container child hbox3.Gtk.Box+BoxChild
            this.close_button = new Gtk.Button();
            this.close_button.WidthRequest = 100;
            this.close_button.CanFocus = true;
            this.close_button.Name = "close_button";
            this.close_button.UseStock = true;
            this.close_button.UseUnderline = true;
            this.close_button.BorderWidth = ((uint)(5));
            this.close_button.Label = "gtk-close";
            this.hbox3.Add(this.close_button);
            Gtk.Box.BoxChild w36 = ((Gtk.Box.BoxChild)(this.hbox3[this.close_button]));
            w36.Position = 3;
            w36.Expand = false;
            w36.Fill = false;
            this.hbox2.Add(this.hbox3);
            Gtk.Box.BoxChild w37 = ((Gtk.Box.BoxChild)(this.hbox2[this.hbox3]));
            w37.Position = 0;
            this.vbox1.Add(this.hbox2);
            Gtk.Box.BoxChild w38 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox2]));
            w38.Position = 1;
            w38.Expand = false;
            w38.Fill = false;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 488;
            this.DefaultHeight = 482;
            this.label3.MnemonicWidget = this.theme_combo;
            this.Show();
            this.notebook1.SwitchPage += new Gtk.SwitchPageHandler(this.OnPageSwitch);
            this.start_with_computer_checkbutton.Toggled += new System.EventHandler(this.OnStartWithComputerCheckbuttonToggled);
            this.theme_combo.Changed += new System.EventHandler(this.OnThemeComboChanged);
            this.new_dock_button.Clicked += new System.EventHandler(this.OnNewDockButtonClicked);
            this.delete_dock_button.Clicked += new System.EventHandler(this.OnDeleteDockButtonClicked);
            this.close_button.Clicked += new System.EventHandler(this.OnCloseButtonClicked);
        }
    }
}
