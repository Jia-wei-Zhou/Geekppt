using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Office.Tools.Ribbon;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;

namespace CodeEvaluation
{
    public partial class Code
    {
        private void Code_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void languageBox_TextChanged(object sender, RibbonControlEventArgs e)
        {
            // obtain the selected language
            string language = this.languageBox.Text;
            string text = language.Equals("General Inputs", StringComparison.OrdinalIgnoreCase)? 
                "Input arguments (separate by a space or new line)" : String.Format("Please insert your {0} code", language);

            // obtain current active slide
            PowerPoint.Slide slide = (PowerPoint.Slide)Globals.ThisAddIn.Application.ActiveWindow.View.Slide;
            
            PowerPoint.Shape textBox = AddTextBox(slide, "Arial", 18, "000000", text);
            BoxContent content = language.Equals("General Inputs", StringComparison.OrdinalIgnoreCase) ? BoxContent.Input : BoxContent.Code;
            textBox.Name = Auxiliary.GenerateCodeBoxName(language, content);
        }

        private void evaluateButton_Click(object sender, RibbonControlEventArgs e)
        {
            // create a new folder for the selected code
            string path = Auxiliary.CreateFolder("temp_PPT_add_in", Directory.GetCurrentDirectory(), true);            
            // obtain current slide
            PowerPoint.Slide slide = (PowerPoint.Slide)Globals.ThisAddIn.Application.ActiveWindow.View.Slide;
            List<PowerPoint.Shape> shapes = new List<PowerPoint.Shape>();
            string color = Auxiliary.GenerateColor();
            string result = "";
            
            try
            {
                // obtain all the selected textboxes   
                Dictionary<string, string> codes = new Dictionary<string, string>();
                foreach (PowerPoint.Shape shape in Globals.ThisAddIn.Application.ActiveWindow.Selection.ShapeRange)
                {
                    if (shape.HasTextFrame == Office.MsoTriState.msoTrue)
                    {
                        codes.Add(shape.Name, shape.TextFrame.TextRange.Text);
                        shapes.Add(shape);
                        //result += shape.Name + "\n";
                    }
                }

                Auxiliary.GenerateTextFileAndInputs(codes, path, out var files, out var main, out var inputs);
                CodeEvaluationCpp evaluate = new CodeEvaluationCpp(main, files);
                evaluate.CreateSourceFile();
                evaluate.GenerateCmakeLists();
                evaluate.RunCode(out var res, "", inputs);
                AddTextBox(slide, "Arial", 18, color, res);               

                foreach (var shape in shapes)
                {
                    shape.TextFrame.TextRange.Font.Color.RGB = Int32.Parse(color, System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch(System.Runtime.InteropServices.COMException exception)
            {                
                Console.WriteLine(exception.ToString());
            }
        }

        private PowerPoint.Shape AddTextBox(PowerPoint.Slide slide, string fontName, int fontSize, string fontColor, string text)
        {
            // insert textbox and display corresponding message
            PowerPoint.Shape textBox = slide.Shapes.AddTextbox(Office.MsoTextOrientation.msoTextOrientationHorizontal, 0, 0, 500, 50);
            // set font and color
            textBox.TextFrame.TextRange.Font.Name = fontName;
            textBox.TextFrame.TextRange.Font.Size = fontSize;
            textBox.TextFrame.TextRange.Font.Color.RGB = Int32.Parse(fontColor, System.Globalization.NumberStyles.HexNumber);
            // insert text
            textBox.TextFrame.TextRange.InsertAfter(text);

            return textBox;
        }
    }
}
