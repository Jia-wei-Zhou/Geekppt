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
        const string tempFolder = "temp_PPT_add_in";

        private void Code_Load(object sender, RibbonUIEventArgs e)
        {

        }


        private void cppMain_Click(object sender, RibbonControlEventArgs e)
        {
            string language = "C++ main";
            add_shape(language);
        }

        private void cpp_Click(object sender, RibbonControlEventArgs e)
        {
            string language = "C++";
            add_shape(language);
        }

        private void javaMain_Click(object sender, RibbonControlEventArgs e)
        {
            string language = "Java main";
            add_shape(language);
        }

        private void java_Click(object sender, RibbonControlEventArgs e)
        {
            string language = "Java";
            add_shape(language);
        }

        private void python_Click(object sender, RibbonControlEventArgs e)
        {
            string language = "Python";
            add_shape(language);
        }


        private void generalInputs_Click(object sender, RibbonControlEventArgs e)
        {
            string language = "General Inputs";
            add_shape(language);
        }

        private void removeTag_Click(object sender, RibbonControlEventArgs e)
        {
            
            List<PowerPoint.Shape> shapes = new List<PowerPoint.Shape>();
            string color = Auxiliary.GenerateColor();
            foreach (PowerPoint.Slide slide in Globals.ThisAddIn.Application.ActivePresentation.Slides)
            {
                
                try
                {
                    // obtain all the selected textboxes   
                    Dictionary<string, string> codes = new Dictionary<string, string>();
                    Dictionary<string, string> tableInput = new Dictionary<string, string>();
                    bool hasTableInput = false;


                    foreach (PowerPoint.Shape shape in slide.Shapes)
                    {
                        if (shape.HasTextFrame == Office.MsoTriState.msoTrue && shape.HasTable != Office.MsoTriState.msoTrue)
                        {
                            //to do!!!!!!!!!
                            //
                            string[] listTags = { "@code_java_main", "@code_java", "@code_python", "@input_general" };

                            if (shape.Name.IndexOf("java") < 0 && shape.Name.IndexOf("python") < 0 && shape.Name.IndexOf("c++") < 0 && shape.Name.IndexOf("general") < 0)
                            {
                                string[] separatingStrings = { "\r", "\n" };
                                string[] tags = shape.TextFrame.TextRange.Text.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                                string tag = tags[0];
                                if (listTags.Contains(tag)) {
                                    string shapeName = Auxiliary.GenerateCodeBoxNameForMd(tag);
                                    separatingStrings[0] = tag;
                                    string[] texts = shape.TextFrame.TextRange.Text.Split(separatingStrings, StringSplitOptions.RemoveEmptyEntries);
                                    string text = null;
                                    for (int i = 0; i < texts.Length; i++)
                                    {
                                        text += texts[i] + "\n";
                                    }
                                    if (hasTableInput)
                                    {
                                        text = Auxiliary.ReplaceParametersWithTableInputList(text, tableInput);
                                    }

                                    shape.TextFrame.TextRange.Text = text;
                                    shape.Name = shapeName;
                                    shapes.Add(shape);
                                }
                                

                            }
                           
                        }
                    }
                }
                catch (System.Runtime.InteropServices.COMException exception)
                {
                    Console.WriteLine(exception.ToString());
                }


            }
                
        }
        private void parameterTable_Click(object sender, RibbonControlEventArgs e)
        {
            // obtain current active slide
            PowerPoint.Slide slide = (PowerPoint.Slide)Globals.ThisAddIn.Application.ActiveWindow.View.Slide;
            PowerPoint.Shape table = slide.Shapes.AddTable(3, 3, 0, 0);
            table.Name = Auxiliary.GenerateCodeTableName();
            table.Table.Cell(1, 1).Merge(table.Table.Cell(1, 3));
            table.Table.Cell(1, 1).Shape.TextFrame.TextRange.Text = table.Name;
            return;
        }


        private void add_shape(string language)
        {
            string text = language.Equals("General Inputs", StringComparison.OrdinalIgnoreCase) ?
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
            string path = Auxiliary.CreateFolder(tempFolder, Directory.GetCurrentDirectory());
            // obtain current slide
            PowerPoint.Slide slide = (PowerPoint.Slide)Globals.ThisAddIn.Application.ActiveWindow.View.Slide;
            List<PowerPoint.Shape> shapes = new List<PowerPoint.Shape>();
            string color = Auxiliary.GenerateColor();
            

            try
            {
                // obtain all the selected textboxes   
                Dictionary<string, string> codes = new Dictionary<string, string>();
                Dictionary<string, string> tableInput = new Dictionary<string, string>();
                bool hasTableInput = false;
                Language language = Language.Invalid;
                foreach (PowerPoint.Shape shape in Globals.ThisAddIn.Application.ActiveWindow.Selection.ShapeRange)
                {
                    if (shape.Name.IndexOf("cpp") != -1)
                    {
                        language = Language.CPP;
                        break;
                    }
                    else if (shape.Name.IndexOf("java") != -1)
                    {
                        language = Language.Java;
                        break;
                    }
                    else if (shape.Name.IndexOf("python") != -1)
                    {
                        language = Language.Python;
                        break;
                    }
                }

                foreach (PowerPoint.Shape shape in Globals.ThisAddIn.Application.ActiveWindow.Selection.ShapeRange)
                {
                    if (shape.HasTable == Office.MsoTriState.msoTrue)
                    {
                        hasTableInput = true;
                        string bucketType = (language == Language.Python) ? "[]" : "{}";
                        string inputList = Auxiliary.GenerateTableInputList(shape, bucketType);
                        tableInput.Add(shape.Name, inputList);
                        //shape.Table.Cell(2, 1).Shape.TextFrame.TextRange.Text = inputList;
                    }
                }
                foreach (PowerPoint.Shape shape in Globals.ThisAddIn.Application.ActiveWindow.Selection.ShapeRange)
                {
                    if (shape.HasTextFrame == Office.MsoTriState.msoTrue && shape.HasTable != Office.MsoTriState.msoTrue)
                    {
                        //to do!!!!!!!!!
                        //

                        if (shape.Name.IndexOf("java") < 0 && shape.Name.IndexOf("python") < 0 && shape.Name.IndexOf("c++") < 0 && shape.Name.IndexOf("general") < 0)
                        {
                            string[] separatingStrings = { "\r" ,"\n"};
                            string[] tags = shape.TextFrame.TextRange.Text.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                            string tag = tags[0];
                            string shapeName = Auxiliary.GenerateCodeBoxNameForMd(tag);
                            separatingStrings[0] = tag;
                            string[] texts = shape.TextFrame.TextRange.Text.Split(separatingStrings, StringSplitOptions.RemoveEmptyEntries);
                            string text = null;
                            for (int i = 0; i < texts.Length; i++) {
                                text += texts[i] + "\n";
                            }
                            if (hasTableInput)
                            {
                                text = Auxiliary.ReplaceParametersWithTableInputList(text, tableInput);
                            }
                          
                            shape.TextFrame.TextRange.Text = text;
                            shape.Name = shapeName;
                            codes.Add(shape.Name, text);
                            shapes.Add(shape);
                            
                        }
                        else
                        {
                            string text = shape.TextFrame.TextRange.Text;
                            if (hasTableInput)
                            {
                                text = Auxiliary.ReplaceParametersWithTableInputList(text, tableInput);
                            }
                            codes.Add(shape.Name, text);
                            shapes.Add(shape);
                        }

                        //result += shape.Name + "\n";
                    }
                }

                Auxiliary.GenerateTextFileAndInputs(codes, path, out var files, out var main, out var inputs);
                if (!Auxiliary.ObtainLanguageType(codes, out Language type))
                {
                    if (type == Language.Invalid)
                    {
                        AddTextBox(slide, "Arial", 18, color, "No valid language is selected");
                        return;
                    }
                    else
                    {
                        AddTextBox(slide, "Arial", 18, color, "More than one programming languages are selected");
                        return;
                    }                    
                }

                ICodeEvaluation evaluate;
                switch (type)
                {
                    case Language.CPP:
                        evaluate = new CodeEvaluationCpp(main, files);
                        break;
                    case Language.Python:
                        evaluate = new CodeEvaluationPython(main, files);
                        break;
                    case Language.Java:
                        evaluate = new CodeEvaluationJava(main, files);
                        break;
                    default:
                        throw new ArgumentException($"The selected language ({type}) is invalid");
                }
                evaluate.CreateSourceFile();
                // python does not need cmake
                // evaluate.GenerateCmakeLists();
                evaluate.RunCode(out var res, "", inputs);
                if (res.Length > 0)
                {
                    AddTextBox(slide, "Arial", 18, color, res);
                }

                foreach (var shape in shapes)
                {
                    shape.TextFrame.TextRange.Font.Color.RGB = Int32.Parse(color, System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (System.Runtime.InteropServices.COMException exception)
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

        private void ChangeName_TextChanged(object sender, RibbonControlEventArgs e)
        {
            string name = this.ChangeName.Text;
            foreach (PowerPoint.Shape shape in Globals.ThisAddIn.Application.ActiveWindow.Selection.ShapeRange)
            {
                shape.Name = name;
                if (shape.HasTable == Office.MsoTriState.msoTrue)
                {
                    shape.Table.Cell(1, 1).Shape.TextFrame.TextRange.Text = name;
                }
            }
        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            foreach (PowerPoint.Shape shape in Globals.ThisAddIn.Application.ActiveWindow.Selection.ShapeRange)
            {
                if (shape.HasTable == Office.MsoTriState.msoTrue)
                {
                    string pythonPlotTemplate = "import matplotlib.pyplot as plt" + System.Environment.NewLine +
                    "plt.title(\"Scatter Diagram\")" + System.Environment.NewLine +
                "plt.xlabel(\"Horizontal Axis\")" + System.Environment.NewLine +
                "plt.ylabel(\"Vertical Axis\")" + System.Environment.NewLine +
                "plt.plot(" + shape.Name + "[0]," + shape.Name + "[1],'bo')" + System.Environment.NewLine + System.Environment.NewLine +
                "### if you need" + System.Environment.NewLine +
                "# plt.xlim(xmax=???,xmin=???)" + System.Environment.NewLine +
                "# plt.ylim(ymax=???,ymin=???)" + System.Environment.NewLine +
                "# plt.annotate(\"???\", xy = ???, xytext = ???, arrowprops = dict(facecolor = 'black', shrink = 0.1))" + System.Environment.NewLine +
                "plt.show()";
                    PowerPoint.Slide slide = (PowerPoint.Slide)Globals.ThisAddIn.Application.ActiveWindow.View.Slide;
                    PowerPoint.Shape textBox = AddTextBox(slide, "Arial", 18, "000000", pythonPlotTemplate);
                    BoxContent content = BoxContent.Code;
                    textBox.Name = Auxiliary.GenerateCodeBoxName("python", content);
                }
            }
        }
    }
}
