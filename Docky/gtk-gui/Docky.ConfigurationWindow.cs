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
        
        private Gtk.HBox hbox1;
        
        private Gtk.VBox vbox1;
        
        private Gtk.Frame frame1;
        
        private Gtk.Alignment GtkAlignment;
        
        private Gtk.VBox vbox2;
        
        private Gtk.CheckButton checkbutton1;
        
        private Gtk.CheckButton cpu_saver_checkbox;
        
        private Gtk.Label GtkLabel2;
        
        private Gtk.Frame dock_placement_frame;
        
        private Gtk.Alignment GtkAlignment2;
        
        private Gtk.Alignment dock_pacement_align;
        
        private Gtk.Label GtkLabel4;
        
        private Gtk.Frame frame2;
        
        private Gtk.Alignment GtkAlignment1;
        
        private Gtk.Table table1;
        
        private Gtk.ComboBox combobox1;
        
        private Gtk.Label label2;
        
        private Gtk.Label GtkLabel3;
        
        private Gtk.Notebook configuration_widget_notebook;
        
        private Gtk.Label label1;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget Docky.ConfigurationWindow
            this.Name = "Docky.ConfigurationWindow";
            this.Title = Mono.Unix.Catalog.GetString("ConfigurationWindow");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            // Container child Docky.ConfigurationWindow.Gtk.Container+ContainerChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Homogeneous = true;
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.frame1 = new Gtk.Frame();
            this.frame1.Name = "frame1";
            this.frame1.ShadowType = ((Gtk.ShadowType)(0));
            // Container child frame1.Gtk.Container+ContainerChild
            this.GtkAlignment = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.GtkAlignment.Name = "GtkAlignment";
            this.GtkAlignment.LeftPadding = ((uint)(12));
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.checkbutton1 = new Gtk.CheckButton();
            this.checkbutton1.CanFocus = true;
            this.checkbutton1.Name = "checkbutton1";
            this.checkbutton1.Label = Mono.Unix.Catalog.GetString("Start When Computer Starts");
            this.checkbutton1.DrawIndicator = true;
            this.checkbutton1.UseUnderline = true;
            this.vbox2.Add(this.checkbutton1);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.vbox2[this.checkbutton1]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.cpu_saver_checkbox = new Gtk.CheckButton();
            this.cpu_saver_checkbox.TooltipMarkup = "Warning! Checking this box may severely hurt the rendering speed of Docky. This option prevents docky from keeping several caches and buffers.";
            this.cpu_saver_checkbox.CanFocus = true;
            this.cpu_saver_checkbox.Name = "cpu_saver_checkbox";
            this.cpu_saver_checkbox.Label = Mono.Unix.Catalog.GetString("Low Memory Mode [Slow Rendering]");
            this.cpu_saver_checkbox.DrawIndicator = true;
            this.cpu_saver_checkbox.UseUnderline = true;
            this.vbox2.Add(this.cpu_saver_checkbox);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox2[this.cpu_saver_checkbox]));
            w2.Position = 1;
            w2.Expand = false;
            w2.Fill = false;
            this.GtkAlignment.Add(this.vbox2);
            this.frame1.Add(this.GtkAlignment);
            this.GtkLabel2 = new Gtk.Label();
            this.GtkLabel2.Name = "GtkLabel2";
            this.GtkLabel2.LabelProp = Mono.Unix.Catalog.GetString("<b>General Options</b>");
            this.GtkLabel2.UseMarkup = true;
            this.frame1.LabelWidget = this.GtkLabel2;
            this.vbox1.Add(this.frame1);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox1[this.frame1]));
            w5.Position = 0;
            w5.Expand = false;
            w5.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.dock_placement_frame = new Gtk.Frame();
            this.dock_placement_frame.Name = "dock_placement_frame";
            this.dock_placement_frame.ShadowType = ((Gtk.ShadowType)(0));
            // Container child dock_placement_frame.Gtk.Container+ContainerChild
            this.GtkAlignment2 = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.GtkAlignment2.Name = "GtkAlignment2";
            this.GtkAlignment2.LeftPadding = ((uint)(12));
            // Container child GtkAlignment2.Gtk.Container+ContainerChild
            this.dock_pacement_align = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.dock_pacement_align.Name = "dock_pacement_align";
            this.GtkAlignment2.Add(this.dock_pacement_align);
            this.dock_placement_frame.Add(this.GtkAlignment2);
            this.GtkLabel4 = new Gtk.Label();
            this.GtkLabel4.Name = "GtkLabel4";
            this.GtkLabel4.LabelProp = Mono.Unix.Catalog.GetString("<b>Dock Placement</b>");
            this.GtkLabel4.UseMarkup = true;
            this.dock_placement_frame.LabelWidget = this.GtkLabel4;
            this.vbox1.Add(this.dock_placement_frame);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.vbox1[this.dock_placement_frame]));
            w8.Position = 1;
            // Container child vbox1.Gtk.Box+BoxChild
            this.frame2 = new Gtk.Frame();
            this.frame2.Name = "frame2";
            this.frame2.ShadowType = ((Gtk.ShadowType)(0));
            // Container child frame2.Gtk.Container+ContainerChild
            this.GtkAlignment1 = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.GtkAlignment1.Name = "GtkAlignment1";
            this.GtkAlignment1.LeftPadding = ((uint)(12));
            // Container child GtkAlignment1.Gtk.Container+ContainerChild
            this.table1 = new Gtk.Table(((uint)(2)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.combobox1 = Gtk.ComboBox.NewText();
            this.combobox1.AppendText(Mono.Unix.Catalog.GetString("classic"));
            this.combobox1.AppendText(Mono.Unix.Catalog.GetString("greyscale"));
            this.combobox1.Name = "combobox1";
            this.table1.Add(this.combobox1);
            Gtk.Table.TableChild w9 = ((Gtk.Table.TableChild)(this.table1[this.combobox1]));
            w9.LeftAttach = ((uint)(1));
            w9.RightAttach = ((uint)(2));
            w9.XOptions = ((Gtk.AttachOptions)(4));
            w9.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Background Appearance:");
            this.table1.Add(this.label2);
            Gtk.Table.TableChild w10 = ((Gtk.Table.TableChild)(this.table1[this.label2]));
            w10.XOptions = ((Gtk.AttachOptions)(4));
            w10.YOptions = ((Gtk.AttachOptions)(4));
            this.GtkAlignment1.Add(this.table1);
            this.frame2.Add(this.GtkAlignment1);
            this.GtkLabel3 = new Gtk.Label();
            this.GtkLabel3.Name = "GtkLabel3";
            this.GtkLabel3.LabelProp = Mono.Unix.Catalog.GetString("<b>Theming</b>");
            this.GtkLabel3.UseMarkup = true;
            this.frame2.LabelWidget = this.GtkLabel3;
            this.vbox1.Add(this.frame2);
            Gtk.Box.BoxChild w13 = ((Gtk.Box.BoxChild)(this.vbox1[this.frame2]));
            w13.Position = 2;
            this.hbox1.Add(this.vbox1);
            Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox1]));
            w14.Position = 0;
            // Container child hbox1.Gtk.Box+BoxChild
            this.configuration_widget_notebook = new Gtk.Notebook();
            this.configuration_widget_notebook.CanFocus = true;
            this.configuration_widget_notebook.Name = "configuration_widget_notebook";
            this.configuration_widget_notebook.CurrentPage = 0;
            this.configuration_widget_notebook.ShowBorder = false;
            this.configuration_widget_notebook.ShowTabs = false;
            // Notebook tab
            Gtk.Label w15 = new Gtk.Label();
            w15.Visible = true;
            this.configuration_widget_notebook.Add(w15);
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("page1");
            this.configuration_widget_notebook.SetTabLabel(w15, this.label1);
            this.label1.ShowAll();
            this.hbox1.Add(this.configuration_widget_notebook);
            Gtk.Box.BoxChild w16 = ((Gtk.Box.BoxChild)(this.hbox1[this.configuration_widget_notebook]));
            w16.Position = 1;
            this.Add(this.hbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 728;
            this.DefaultHeight = 418;
            this.Show();
        }
    }
}
