/* documentation of this class */

#pragma once

// macro defines to determine the platform
#define WINDOWS 0
#define UNIX 1
#if defined(_WIN32)
#define PLATFORM WINDOWS
#define popen _popen
#define pclose _pclose
#else
#define PLATFORM UNIX
#endif

#include <string>
#include <fstream>
#include <cstdio>
#include <iostream>
#include <memory>
#include <stdexcept>
#include <array>
#include <vector>
#include <filesystem>
#include <algorithm>

enum LanguageType {
    CPP,
    JAVA,
    PYTHON
};


class CodeEvaluation {
private:
  std::string code_;
  LanguageType language_ ;
  std::string address_;
  std::string filename_;
  std::string compiler_;

public:
    /* constructors */
    CodeEvaluation(LanguageType language, std::string const& address) :
        language_(language), address_(address) {
        code_ = readTxt(address_);
        filename_ = extractFilename(address_);
        // Qianhao, Default compiler
        if (language == CPP) {
            compiler_ = "g++";
        }
        else if (language == JAVA) {
            compiler_ = "javac";
        }
        else {
            compiler_ = ""; 
        }
    }
    
    /* overloading constructors */
    CodeEvaluation(LanguageType language, std::string const& address, std::string const& compiler): 
    language_(language), address_(address), compiler_(compiler){
        code_ = readTxt(address_);
        filename_ = extractFilename(address_);
    }

    /**
     * extract from absolute address the filename without suffix
     * @param address the absolute address of file, e.g. "/homes/user/document/sampleCode.txt"
     * @return the filename without suffix, e.g. "sampleCode"
     */
    std::string extractFilename(std::string const& address);


    /* read from a txt file and return its content as string
    */
    std::string readTxt(std::string const& address) const;

    /* create a file with a certain language type suffix
       and write code_ inside the file
       e.g. address_ filename is 1.txt, language_ is CPP
       should write to 1.cpp file in the same directory
       return the filename of the file */
    std::string createAndWriteFile();


    /* change suffix .txt to certain type and return filename */

    std::string changeSuffix(LanguageType language);

    /**
     * if not use cmake, generate compile cmd from a certain language and compiler
     * @param compiler compiler name, e.g. "g++"
     * @return the compiler command, e.g. "g++ sampleCode.cpp -o sampleCode"
     */
    std::string generateCompileCommand(std::string const& compiler);

    /**
     *  generate executable command
     * @param filename executable filename, e.g. "test" or "text.exe"
     * @param input command input args e.g. "1 2 3"
     * @return a executable command, e.g. "text 1 2 3" 
     */
    std::string generateRunCommand(std::string const& filename, std::string const& input);

    /* This function is to generate input for different platforms. */
    std::string generateInputCommand(std::string const& input);

    /**
    * Use to generate the CMakeLists for C++ projects
    * @param project_name The project name
    * @param main_file The filename of the file that contains main function
    * @param output_cmake_path The output cmake filename and path
    * @param libs The filenames of C++ libraries
    * @param cpp_standard The standard of C++
    * @param cmake_mini_version The minimum version of the cmake for building the project
    */
    static void generateCmakeFile(const std::string& project_name,
                                  const std::string& main_file,
                                  const std::string& output_cmake_path,
                                  std::vector<std::string>& libs,
                                  const int cpp_standard = 20,
                                  const std::string& cmake_mini_version = "3.20");

    /* done!
       execute certain command in command line */
    void executeInCmdLine(std::string const& cmd);

    /* already finished by Zhou */
    std::string executeAndGetFromCmd(std::string const& cmd);


    // sets
    void set_address(std::string const& address) { address_ = address; }
    void set_language(LanguageType language) { language_ = language; }

    // gets
    std::string get_filename() { return filename_; }

    /**
    * run the code of the current address path(ps. saved in private data member "address_")
    * @param input The content of the standard input stream used in the code(can be empty "")
    * @return The result of the code running
    */
    std::string runCode(std::string const& input);

    /**
    * run the code of the specified address path
    * @param address The specified address(btw, this address is copied to the private data member "address_")
    * @param input The content of the standard input stream used in the code(can be empty "")
    * @return The result of the code running
    */
    std::string runCode(std::string const& address, std::string const& input);
};