using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CodeEvaluation
{
    enum Language { CPP, Java, Python, Invalid };
    
    partial class Auxiliary
    {
        public static string GenerateTextFile(string path, string code, Language type, bool isMain, int id)
        {                      
            string filename = path + Path.DirectorySeparatorChar + GenerateFilename(type, isMain, id);
            File.WriteAllText(filename, code);

            return filename;
        }

        /// <summary>
        /// Convert the content in the text boxes into files or string of inputs
        /// </summary>
        /// <param name="codes">A dictionary whose key is the name of the text box, value is its content</param>
        /// <param name="path">The path to create files</param>
        /// <param name="address">The list contains all the output files</param>
        /// <param name="main">The address of the file contains main function</param>
        /// <param name="inputs"></param>
        public static void GenerateTextFileAndInputs(Dictionary<string, string> codes, string path, out List<string> address, out string main, out string inputs)
        {
            address = new List<string>();
            inputs = "";
            main = "Invalid";
            foreach(string key in codes.Keys)
            {
                ExtractCodeBoxInfo(key, out Language type, out bool isMain, out BoxContent content, out int id);
                if(content == BoxContent.Code && !isMain)
                {
                    address.Add(GenerateTextFile(path, codes[key], type, false, id));
                }
                else if(content == BoxContent.Code && isMain)
                {
                    main = GenerateTextFile(path, codes[key], type, true, id);
                }
                else
                {
                    inputs += codes[key] + "\n";
                }
            }
        }

        /// <summary>
        /// Create a new folder
        /// </summary>
        /// <param name="name">The name of the new folder</param>
        /// <param name="current">The path to create the folder</param>
        /// <param name="clear">Whether the folder should be cleared if it exists</param>
        /// <returns></returns>
        public static string CreateFolder(string name, string current = "", bool clear = false)
        {
            string currentDirectory = current.Equals("") ? Directory.GetCurrentDirectory() : current;
            if (!Directory.Exists(currentDirectory))
            {
                throw new ArgumentException($"{currentDirectory} does not exist");
            }

            string newDir = currentDirectory + Path.DirectorySeparatorChar + name;
            if (!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            }
            if (Directory.Exists(newDir) && clear)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(newDir);
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    dir.Delete(true);
                }
            }

            return newDir;
        }

        /// <summary>
        /// Run a specific program, exception handling should be inserted 
        /// exe not found, input args not match
        /// </summary>
        /// <param name="executable">The name of the program</param>
        /// <param name="args">The arguments of this program</param>
        /// <param name="inputs">The inputs of this program</param>
        /// <returns></returns>
        public static string RunProgram(string executable, string args = "", string inputs = "")
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = false,
                    FileName = executable,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    Arguments = args
                }
            };

            process.Start();
            if (!inputs.Equals(""))
            {
                string[] lines = inputs.Split('\n');
                foreach (var cmd in lines)
                {
                    process.StandardInput.WriteLine(cmd);
                }
            }
            string result = process.StandardOutput.ReadToEnd();
            process.Close();

            return result;
        }
    }

    public interface ICodeEvaluation
    {
        void CreateSourceFile();
        bool RunCode(out string result, string cmdArgs = "", string inputs = "");
    }

    public class CodeEvaluationCpp : ICodeEvaluation
    {
        private List<string> textAddress;
        private List<string> libs;
        private string mainFile;
        private const Language TYPE = Language.CPP;
        private readonly string OS_NAME;
        private readonly string CODE_FOLDER;

        public List<string> TextAddress
        {
            get => libs;
        }

        public string MainFile
        {
            get => mainFile;
        }

        public string OSName
        {
            get => OS_NAME;
        }

        public string CodeFolder
        {
            get => CODE_FOLDER;
        }

        public CodeEvaluationCpp(string mainFile, List<string> textAddress)
        {
            OS_NAME = Environment.OSVersion.Platform.ToString();
            CODE_FOLDER = Auxiliary.CreateFolder(Auxiliary.GenerateRandomName(), Auxiliary.tempFolder, true);
            libs = new List<string>();
            this.textAddress = textAddress;
            this.mainFile = mainFile;
        }

        public void CreateSourceFile()
        {
            foreach (var address in textAddress)
            {
                string filename = CodeFolder + Path.DirectorySeparatorChar + Auxiliary.GenerateRandomName() + ".cpp";
                string content = File.ReadAllText(address);
                File.WriteAllText(filename, content);
                libs.Add(filename);
            }

            string sourceMain = CodeFolder + Path.DirectorySeparatorChar + "main.cpp";
            using (var writer = new StreamWriter(sourceMain))
            {
                foreach (var address in TextAddress)
                {
                    string line = String.Format($"#include \"{Path.GetFileName(address)}\"");
                    writer.WriteLine(line);
                }

                writer.Write(File.ReadAllText(MainFile));
            }
            mainFile = sourceMain;
        }

        public string GenerateCmakeLists(int cppStandard = 20, string cmakeMinVersion = "3.10")
        {
            string cmakeFilename = CodeFolder + Path.DirectorySeparatorChar + "CMakeLists.txt";

            using (var writer = new StreamWriter(cmakeFilename))
            {
                string line = String.Format($"cmake_minimum_required(VERSION {cmakeMinVersion})");
                writer.WriteLine(line);
                line = String.Format($"project({Path.GetFileNameWithoutExtension(MainFile)})\n");
                writer.WriteLine(line);
                line = String.Format($"set(CMAKE_CXX_STANDARD {cppStandard})\n");
                writer.WriteLine(line);

                foreach (var lib in TextAddress)
                {
                    line = String.Format($"add_library({Path.GetFileNameWithoutExtension(lib)} {Path.GetFileName(lib)})");
                    writer.WriteLine(line);
                }

                line = String.Format($"\nadd_executable(main {Path.GetFileName(MainFile)})");
                writer.WriteLine(line);

                line = String.Format($"\ntarget_link_libraries(main ");
                writer.Write(line);
                foreach (var lib in TextAddress)
                {
                    writer.Write($"{Path.GetFileNameWithoutExtension(lib)} ");
                }
                writer.Write(")");
            }

            return cmakeFilename;
        }

        public bool RunCode(out string result, string cmdArgs = "", string inputs = "")
        {
            string buildDir = CompileCode(out string init, out string build);            

            string current = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(buildDir);
            result = Auxiliary.RunProgram("main", cmdArgs, inputs);
            Directory.SetCurrentDirectory(current);

            return true;
        }

        private string CompileCode(out string init, out string build)
        {
            string buildDir = Auxiliary.CreateFolder("build", CodeFolder, true);
            string current = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(buildDir);

            init = Auxiliary.RunProgram("cmake", "-G \"MinGW Makefiles\" ../");
            init = Auxiliary.RunProgram("cmake", "-G \"MinGW Makefiles\" ../");
            build = Auxiliary.RunProgram("cmake", "--build .");

            Directory.SetCurrentDirectory(current);
            return buildDir;
        }
    }

    public class CodeEvaluationPython : ICodeEvaluation
    {
        private List<string> textAddress;
        private List<string> libs;
        private string mainFile;
        private const Language TYPE = Language.Python;
        private readonly string OS_NAME;
        private readonly string CODE_FOLDER;

        public List<string> TextAddress
        {
            get => libs;
        }

        public string MainFile
        {
            get => mainFile;
        }

        public string OSName
        {
            get => OS_NAME;
        }

        public string CodeFolder
        {
            get => CODE_FOLDER;
        }


        
        public CodeEvaluationPython(string mainFile, List<string> textAddress)
        {
            OS_NAME = Environment.OSVersion.Platform.ToString();
            CODE_FOLDER = Auxiliary.CreateFolder(Auxiliary.GenerateRandomName(), Auxiliary.tempFolder, true);
            libs = new List<string>();
            this.textAddress = textAddress;
            this.mainFile = mainFile;
        }

        /// <summary>
        /// get local python executable file
        /// </summary>
        /// <returns></returns>
        public static string GetPythonPath()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            string pathVariable = environmentVariables["Path"] as string;
            if (pathVariable != null)
            {
                string[] allPaths = pathVariable.Split(';');
                foreach (var path in allPaths)
                {
                    string pythonPathFromEnv = path + "\\python.exe";
                    if (File.Exists(pythonPathFromEnv))
                        return pythonPathFromEnv;
                }
            }
            return null;
        }

        public void CreateSourceFile()
        {
            foreach (var address in textAddress)
            {
                string filename = CodeFolder + Path.DirectorySeparatorChar + Auxiliary.GenerateRandomName() + ".py";
                string content = File.ReadAllText(address);
                File.WriteAllText(filename, content);
                libs.Add(filename);
            }

            string sourceMain = CodeFolder + Path.DirectorySeparatorChar + "main.py";
            {
                foreach (var address in TextAddress)
                {
                    // write main func, but import other .py file
                    string content = File.ReadAllText(address) + "\n";
                    string[] module = Path.GetFileName(address).Split('.');
                    string import = String.Format($"from {module[0]} import *\n");

                    if (content.Contains("__main__"))
                    {
                        File.AppendAllText(sourceMain, content);
                    }
                    else
                    {
                        if (File.Exists(sourceMain))
                        {
                            var currentContent = File.ReadAllText(sourceMain);
                            File.WriteAllText(sourceMain, import + currentContent);
                        }
                        else
                        {
                            File.AppendAllText(sourceMain, import);
                        }

                    }

                }
            }
            mainFile = sourceMain;
        }

        public bool RunCode(out string result, string cmdArgs = "", string inputs = "")
        {
            string executable = GetPythonPath();
            cmdArgs = mainFile + cmdArgs;
            result = Auxiliary.RunProgram(executable, cmdArgs, inputs);

            return true;
        }
    }
}
