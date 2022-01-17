using System;
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

        public static void GenerateTextFileAndInputs(Dictionary<string, string> codes, string path, out Dictionary<string, Language> files, out string inputs)
        {
            files = new Dictionary<string, Language>();
            inputs = "";
            foreach(string key in codes.Keys)
            {
                ExtractCodeBoxInfo(key, out Language type, out bool isMain, out BoxContent content, out int id);
                if(content == BoxContent.Code)
                {
                    files.Add(GenerateTextFile(path, codes[key], type, isMain, id), type);
                }
                else
                {
                    inputs += codes[key] + "\n";
                }
            }
        }

        public static bool RunCode(Language type, string filename, string inputs, out string runResult)
        {            
            CodeEvaluation evaluate = new CodeEvaluation(type, filename);
            evaluate.CreateSourceFile();
            string compileArgs = evaluate.GenerateCompileArgs();
            CodeEvaluation.RunProgram(evaluate.Compiler, compileArgs);

            var data = CodeEvaluation.GenerateInputList(inputs);
            string executable = evaluate.SRCFilename.Substring(0, evaluate.SRCFilename.LastIndexOf('.'));
            runResult = CodeEvaluation.RunProgram(executable, "", data);
            return true;
        }
    }

    /// <summary>
    /// Evaluate the result of the code
    /// Currently support C++ (Java and Python are not implemented)
    /// </summary>
    class CodeEvaluation
    {
        private string code;
        private readonly Language TYPE;
        private readonly string COMPILER;
        private string textAddress;
        private string filename;
        private readonly string OS_NAME;
        private string codeFolder;
        private string srcFilename = null;

        public string Code
        {
            get => code;
        }

        private string SetCode
        {
            set => code = value;
        }

        public Language Type
        {
            get => TYPE;
        }

        public string Compiler
        {
            get => COMPILER;
        }

        public string TextAddress
        {
            get => textAddress;
            set
            {
                if (!File.Exists(value))
                {
                    throw new FileNotFoundException($"File {value} not found");
                }

                textAddress = value;
                SetFilename = Path.GetFileNameWithoutExtension(TextAddress);
                string dirName = CreateFolder(TextAddress, "Code");
                SetCodeFolder = CreateFolder(dirName, Filename);
                SetCode = ReadFile(TextAddress);
                CreateSourceFile();
            }
        }

        public string Filename
        {
            get => filename;
        }

        private string SetFilename
        {
            set => filename = value;
        }

        public string OSName
        {
            get => OS_NAME;
        }

        public string CodeFolder
        {
            get => codeFolder;
        }

        private string SetCodeFolder
        {
            set => codeFolder = value;
        }

        public string SRCFilename
        {
            get => srcFilename;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The pogramming language of the code</param>
        /// <param name="address">The path of the text file that contains the code snippet</param>
        public CodeEvaluation(Language type, string address)
        {
            TYPE = type;
            TextAddress = address;
            OS_NAME = Environment.OSVersion.Platform.ToString();

            switch (type)
            {
                case Language.CPP:
                    COMPILER = "g++";
                    break;
                case Language.Java:
                    COMPILER = "javac";
                    break;
                case Language.Python when OS_NAME.StartsWith("Unix"):
                    COMPILER = "python3";
                    break;
                case Language.Python when OS_NAME.StartsWith("Win"):
                    COMPILER = "python";
                    break;
                default:
                    throw new ArgumentException($"Invalid compiler");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The pogramming language of the code</param>
        /// <param name="address">The path of the text file that contains the code snippet</param>
        /// <param name="compiler">The compiler for this code snipped</param>
        public CodeEvaluation(Language type, string address, string compiler)
        {
            TYPE = type;
            TextAddress = address;
            COMPILER = compiler;
            OS_NAME = Environment.OSVersion.Platform.ToString();
        }

        /// <summary>
        /// Create the source file based on the text file
        /// </summary>
        /// <returns></returns>
        public virtual string CreateSourceFile()
        {
            string srcFile = CodeFolder + Path.DirectorySeparatorChar + Path.GetFileName(ChangeExtension());
            File.WriteAllText(srcFile, code);
            srcFilename = srcFile;
            return srcFile;
        }

        /// <summary>
        /// Generate the compile arguments for this code snippet
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateCompileArgs()
        {
            if (SRCFilename == null)
            {
                throw new FileNotFoundException("No input file for generating compile command");
            }

            if (TYPE == Language.Python)
            {
                return "";
            }

            string cmd = "";
            switch (TYPE)
            {
                case Language.CPP:
                    cmd += SRCFilename;
                    break;
                case Language.Java:
                    cmd += SRCFilename;
                    break;
            }

            cmd += " -o ";
            cmd += SRCFilename.Substring(0, SRCFilename.LastIndexOf('.'));
            return cmd;
        }

        /// <summary>
        /// Run the code snippet
        /// </summary>
        /// <param name="inputs">The input of the code (if necessary), the inputs should be separated by a space or new line</param>
        /// <returns></returns>
        public string RunCode(string inputs = "")
        {
            if (SRCFilename == null)
            {
                throw new FileNotFoundException("No input file for run code");
            }

            var commands = GenerateInputList(inputs);
            return RunProgram(SRCFilename.Substring(0, SRCFilename.LastIndexOf('.')), "", commands);
        }

        /// <summary>
        /// Convert the user input to a list
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns></returns>
        public static List<string> GenerateInputList(string input)
        {
            List<string> commands = new List<string>();
            string[] lines = input.Split('\n');
            foreach (var line in lines)
            {
                commands.Add(line);
            }

            return commands;
        }

        public static string ReadFile(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"File {filename} not found");
            }
            return File.ReadAllText(filename);
        }

        /// <summary>
        /// Generate a cmake file (only available for C++)
        /// </summary>
        /// <param name="projectName">The project name</param>
        /// <param name="mainFile">The filename of the file contains main function</param>
        /// <param name="cmakeFilename">The filename of the output cmake file (including path)</param>
        /// <param name="libs">The libraries used in this project</param>
        /// <param name="cppStandard">The standard of C++ (default is C++20)</param>
        /// <param name="cmakeMinVersion">The minimum version of the cmake for building the project (defulat is 3.10)</param>
        public static void GenerateCMakeFile(string projectName, string mainFile, string cmakeFilename,
            List<string> libs, int cppStandard = 20, string cmakeMinVersion = "3.10")
        {
            if (Path.GetFileName(cmakeFilename) != "CMakeLists.txt")
            {
                throw new ArgumentException($"{cmakeFilename} is not a valid cmake file");
            }

            // generating CMakeLists.txt
            using (var writer = new StreamWriter(cmakeFilename))
            {
                string line = String.Format($"cmake_minimum_required(VERSION {cmakeMinVersion})");
                writer.WriteLine(line);
                line = String.Format($"project({projectName})\n");
                writer.WriteLine(line);
                line = String.Format($"set(CMAKE_CXX_STANDARD {cppStandard})\n");
                writer.WriteLine(line);

                foreach (var lib in libs)
                {
                    line = String.Format($"add_library({Path.GetFileNameWithoutExtension(lib)} {lib})");
                    writer.WriteLine(line);
                }

                line = String.Format($"\nadd_executable(main {mainFile})");
                writer.WriteLine(line);

                line = String.Format($"\ntarget_link_libraries(main ");
                writer.Write(line);
                foreach (var lib in libs)
                {
                    writer.Write($"{Path.GetFileNameWithoutExtension(lib)} ");
                }
                writer.Write(")");
            }
        }

        /// <summary>
        /// Change the extension of the file to corresponding programming language
        /// </summary>
        /// <returns></returns>
        private string ChangeExtension()
        {
            switch (TYPE)
            {
                case Language.CPP:
                    return Path.ChangeExtension(TextAddress, "cpp");
                case Language.Java:
                    return Path.ChangeExtension(TextAddress, "java");
                case Language.Python:
                    return Path.ChangeExtension(TextAddress, "py");
                default:
                    throw new ArgumentException($"The extension {TYPE.ToString()} is invalid");
            }
        }

        /// <summary>
        /// Determine the existance of the compiler (not working on MacOS)
        /// </summary>
        /// <returns></returns>
        protected bool IsCompilerExist()
        {
            string[] variables = Environment.GetEnvironmentVariable("PATH").Split(';', ':');
            string extension = OS_NAME.StartsWith("Win") ? ".exe" : ".app";

            foreach (var path in variables)
            {
                if (Directory.Exists(path))
                {
                    string filename = path + Path.DirectorySeparatorChar + COMPILER + extension;
                    if (File.Exists(filename))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Create a folder, nothing happen if the folder exists
        /// </summary>
        /// <param name="path">The path to create the folder</param>
        /// <param name="name">The name of the new folder</param>
        /// <param name="clear">Whether clear the content of the folder if it exist</param>
        /// <returns></returns>
        public static string CreateFolder(string path, string name, bool clear = false)
        {
            string dirName;
            if (File.Exists(path))
            {
                dirName = Path.GetDirectoryName(Path.GetFullPath(path));
            }
            else if (Directory.Exists(path))
            {
                dirName = path;
            }
            else
            {
                throw new ArgumentException($"The path \"{path}\" is not valid");
            }

            string newDirName = dirName + Path.DirectorySeparatorChar + name;
            if (!Directory.Exists(newDirName))
            {
                Directory.CreateDirectory(newDirName);
            }
            if(Directory.Exists(newDirName) && clear)
            {
                // Error
                DirectoryInfo dirInfo = new DirectoryInfo(newDirName);
                foreach(FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach(DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    dir.Delete(true);
                }
                // Error
            }

            return newDirName;
        }

        /// <summary>
        /// Run a specific program, exception handling should be inserted 
        /// exe not found, input args not match
        /// </summary>
        /// <param name="executable">The name of the program</param>
        /// <param name="args">The arguments of this program</param>
        /// <param name="inputs">The inputs of this program</param>
        /// <returns></returns>
        public static string RunProgram(string executable, string args = null, List<string> inputs = null)
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
            if (inputs != null)
            {
                foreach (var cmd in inputs)
                {
                    process.StandardInput.WriteLine(cmd);
                }
            }
            string result = process.StandardOutput.ReadToEnd();
            process.Close();

            return result;
        }
    }
}
