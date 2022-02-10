using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluation
{
    enum BoxContent { Code, Input, Output };

    partial class Auxiliary
    {
        public const string tempFolder = "temp_PPT_add_in";
        private static int boxID = 0;
        private static int tableID = 0;
        private static readonly Random rand = new Random();


        /// <summary>
        /// Generate text box name
        /// </summary>
        /// <param name="baseName">The user selection or name from other text boxes, for example, C++_main</param>
        /// <param name="content">The content of the text box</param>
        /// <param name="id">The ID of this text box, the internal value is used if no valus is provided</param>
        /// <returns></returns>
        public static string GenerateCodeBoxName(string baseName, BoxContent content, int? id = null)
        {
            string boxName = baseName.ToLower() + "_";
            boxName += content.ToString().ToLower() + "_";
            boxName += id ?? boxID++;

            return boxName;
        }
        public static string GenerateCodeTableName(int? id = null)
        {
            string tableName = "table";
            tableName += id ?? tableID++;

            return tableName;
        }

        public static string GenerateCodeBoxNameForMd(string baseName, int? id = null)
        {
            string[] seperate = { "@", "_" };
            string boxName = null;
            string[] names = baseName.Split(seperate, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length == 2)
            {
                boxName = names[1] + '_' + names[0] + '_';
            }
            if (names.Length == 3)
            {
                boxName = names[1] + ' ' + names[2] + '_' + names[0] + '_';
            }

            boxName += id ?? boxID++;

            return boxName;
        }

        /// <summary>
        /// Extract code box information from its name
        /// </summary>
        /// <param name="boxName">The name of the code box</param>
        /// <param name="type">The programming language of the code</param>
        /// <param name="isMain">Whether there is main function in the code box (always false for Python)</param>
        /// <param name="content">The content of the code box, for example, code and input/output text</param>
        /// <param name="id">The ID of the code box</param>
        public static void ExtractCodeBoxInfo(string boxName, out Language type, out bool isMain, out BoxContent content, out int id)
        {
            string[] data = boxName.Split('_');
            string[] fileInfo = data[0].Split(' ');

            isMain = false;
            switch (fileInfo[0])
            {
                case "c++":
                    type = Language.CPP;
                    isMain = (fileInfo.Length == 2) ? true : false;
                    break;
                case "java":
                    type = Language.Java;
                    isMain = (fileInfo.Length == 2) ? true : false;
                    break;
                case "python":
                    type = Language.Python;
                    break;
                default:
                    type = Language.Invalid;
                    break;
            }

            switch (data[1])
            {
                case "code":
                    content = BoxContent.Code;
                    break;
                case "input":
                    content = BoxContent.Input;
                    break;
                case "output":
                    content = BoxContent.Output;
                    break;
                default:
                    throw new ArgumentException($"{boxName} is not a valid text box name for this add-in");
            }

            if (!Int32.TryParse(data[2], out id))
            {
                throw new ArgumentException($"{boxName} is not a valid text box name for this add-in");
            }
        }

        /// <summary>
        /// Generate the filename for the text file
        /// </summary>
        /// <param name="type">The programming language of the code</param>
        /// <param name="isMain">Whether the main function is in the file</param>
        /// <param name="id">The ID of the file</param>
        /// <returns></returns>
        public static string GenerateFilename(Language type, bool isMain, int id)
        {
            if (type == Language.CPP)
            {
                return GenerateRandomName() + ".txt";
            }

            string filename = (type == Language.CPP) ? "cpp" : type.ToString().ToLower();
            filename += isMain ? "_main_" : "_lib_";
            filename += id;
            filename += ".txt";
            return filename;
        }

        public static string GenerateRandomName()
        {
            // Leading X ensure that the filename does not begin with a number
            string result = "X" + rand.Next(1000000).ToString("X") + "_";
            result += rand.Next(100000).ToString("x") + "_";
            result += rand.Next(10000).ToString("X");
            return result;

        }
    }
}
