namespace CodeEvaluation
{
    partial class Code : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Code()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Office.Tools.Ribbon.RibbonDialogLauncher ribbonDialogLauncherImpl1 = this.Factory.CreateRibbonDialogLauncher();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl1 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl2 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl3 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl4 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl5 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl6 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl7 = this.Factory.CreateRibbonDropDownItem();
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.languageBox = this.Factory.CreateRibbonComboBox();
            this.evaluateButton = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "Code Evaluation";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.DialogLauncher = ribbonDialogLauncherImpl1;
            this.group1.Items.Add(this.languageBox);
            this.group1.Items.Add(this.evaluateButton);
            this.group1.Label = "group1";
            this.group1.Name = "group1";
            // 
            // languageBox
            // 
            ribbonDropDownItemImpl1.Label = "C++ main";
            ribbonDropDownItemImpl2.Label = "C++";
            ribbonDropDownItemImpl3.Label = "Java main";
            ribbonDropDownItemImpl4.Label = "Java";
            ribbonDropDownItemImpl5.Label = "Python";
            ribbonDropDownItemImpl6.Label = "General Inputs";
            ribbonDropDownItemImpl7.Label = "parameter table";
            this.languageBox.Items.Add(ribbonDropDownItemImpl1);
            this.languageBox.Items.Add(ribbonDropDownItemImpl2);
            this.languageBox.Items.Add(ribbonDropDownItemImpl3);
            this.languageBox.Items.Add(ribbonDropDownItemImpl4);
            this.languageBox.Items.Add(ribbonDropDownItemImpl5);
            this.languageBox.Items.Add(ribbonDropDownItemImpl6);
            this.languageBox.Items.Add(ribbonDropDownItemImpl7);
            this.languageBox.Label = "Language";
            this.languageBox.Name = "languageBox";
            this.languageBox.Text = null;
            this.languageBox.TextChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.languageBox_TextChanged);
            // 
            // evaluateButton
            // 
            this.evaluateButton.Label = "Evaluate";
            this.evaluateButton.Name = "evaluateButton";
            this.evaluateButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.evaluateButton_Click);
            // 
            // Code
            // 
            this.Name = "Code";
            this.RibbonType = "Microsoft.PowerPoint.Presentation";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Code_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonComboBox languageBox;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton evaluateButton;
    }

    partial class ThisRibbonCollection
    {
        internal Code Code
        {
            get { return this.GetRibbon<Code>(); }
        }
    }
}
