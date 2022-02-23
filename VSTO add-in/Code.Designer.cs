using System.IO;

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

            string str1 = Directory.GetCurrentDirectory();
            str1 += Path.DirectorySeparatorChar + "icons" + Path.DirectorySeparatorChar;
            string cpp = str1 + "cpp.png";
            string java = str1 + "java.jpg";
            string python = str1 + "python.png";
            string evaluation = str1 + "evaluation.png";
            string reformat = str1 + "reformat.png";
            this.cppMain.ShowImage = true;
            this.cppMain.Image = System.Drawing.Image.FromFile(cpp);
            this.cpp.ShowImage = true;
            this.cpp.Image = System.Drawing.Image.FromFile(cpp);
            this.javaMain.ShowImage = true;
            this.javaMain.Image = System.Drawing.Image.FromFile(java);
            this.java.ShowImage = true;
            this.java.Image = System.Drawing.Image.FromFile(java);
            this.python.ShowImage = true;
            this.python.Image = System.Drawing.Image.FromFile(python);
            this.evaluateButton.ShowImage = true;
            this.evaluateButton.Image = System.Drawing.Image.FromFile(evaluation);
            this.reformatCode.ShowImage = true;
            this.reformatCode.Image = System.Drawing.Image.FromFile(reformat);
            this.generalInputs.ShowImage = true;
            this.generalInputs.Image = System.Drawing.Image.FromFile(reformat);
            this.parameterTable.ShowImage = true;
            this.parameterTable.Image = System.Drawing.Image.FromFile(reformat);
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
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.cppMain = this.Factory.CreateRibbonButton();
            this.cpp = this.Factory.CreateRibbonButton();
            this.javaMain = this.Factory.CreateRibbonButton();
            this.java = this.Factory.CreateRibbonButton();
            this.python = this.Factory.CreateRibbonButton();
            this.generalInputs = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.evaluateButton = this.Factory.CreateRibbonButton();
            this.reformatCode = this.Factory.CreateRibbonButton();
            this.button1 = this.Factory.CreateRibbonButton();
            this.parameterTable = this.Factory.CreateRibbonButton();
            this.ChangeName = this.Factory.CreateRibbonEditBox();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Label = "Code Evaluation";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.DialogLauncher = ribbonDialogLauncherImpl1;
            this.group1.Items.Add(this.cppMain);
            this.group1.Items.Add(this.cpp);
            this.group1.Items.Add(this.javaMain);
            this.group1.Items.Add(this.java);
            this.group1.Items.Add(this.python);
            this.group1.Items.Add(this.generalInputs);
            this.group1.Label = "Languages";
            this.group1.Name = "group1";
            // 
            // cppMain
            // 
            this.cppMain.Label = "C++ main";
            this.cppMain.Name = "cppMain";
            this.cppMain.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.cppMain_Click);
            // 
            // cpp
            // 
            this.cpp.Label = "C++";
            this.cpp.Name = "cpp";
            this.cpp.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.cpp_Click);
            // 
            // javaMain
            // 
            this.javaMain.Label = "Java main";
            this.javaMain.Name = "javaMain";
            this.javaMain.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.javaMain_Click);
            // 
            // java
            // 
            this.java.Label = "Java";
            this.java.Name = "java";
            this.java.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.java_Click);
            // 
            // python
            // 
            this.python.ImageName = "python";
            this.python.Label = "Python";
            this.python.Name = "python";
            this.python.ShowImage = true;
            this.python.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.python_Click);
            // 
            // generalInputs
            // 
            this.generalInputs.Label = "General Inputs";
            this.generalInputs.Name = "generalInputs";
            this.generalInputs.ShowImage = true;
            this.generalInputs.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.generalInputs_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.evaluateButton);
            this.group2.Items.Add(this.reformatCode);
            this.group2.Items.Add(this.button1);
            this.group2.Items.Add(this.parameterTable);
            this.group2.Items.Add(this.ChangeName);
            this.group2.Label = "Functionalities";
            this.group2.Name = "group2";
            // 
            // evaluateButton
            // 
            this.evaluateButton.Label = "Evaluate";
            this.evaluateButton.Name = "evaluateButton";
            this.evaluateButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.evaluateButton_Click);
            // 
            // reformatCode
            // 
            this.reformatCode.Label = "reformat code";
            this.reformatCode.Name = "reformatCode";
            this.reformatCode.ShowImage = true;
            this.reformatCode.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.removeTag_Click);
            // 
            // button1
            // 
            this.button1.Label = "Plot Template";
            this.button1.Name = "button1";
            this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // parameterTable
            // 
            this.parameterTable.Label = "Parameter Table";
            this.parameterTable.Name = "parameterTable";
            this.parameterTable.ShowImage = true;
            this.parameterTable.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.parameterTable_Click);
            // 
            // ChangeName
            // 
            this.ChangeName.Label = "ChangeName";
            this.ChangeName.Name = "ChangeName";
            this.ChangeName.Text = null;
            this.ChangeName.TextChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ChangeName_TextChanged);
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
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton evaluateButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox ChangeName;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton cppMain;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton cpp;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton javaMain;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton java;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton python;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton generalInputs;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton parameterTable;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton reformatCode;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
    }

    partial class ThisRibbonCollection
    {
        internal Code Code
        {
            get { return this.GetRibbon<Code>(); }
        }
    }
}
