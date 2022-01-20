/**
 * @file CodeEvaluation.h
 * @author Team Cadenza
 * @brief CodeEvaluation contains a class that can compile 
 *        and run code automatically and return its output
 * @version 0.1
 * @date 2022-01-06
 * 
 * @copyright Copyright (c) 2022
 * 
 */

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
    /**
     * @brief Construct a new Code Evaluation object
     * 
     * @param language The language to be used
     * @param address The address of source file, which is the code to be run
     */
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
    
    /**
     * @brief Construct a new Code Evaluation object
     * 
     * @param language The language to be used
     * @param address The address of source file, which is the code to be run
     * @param compiler The compiler to be used
     */
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


     /**
     * used to read TXT format file and store the information in string
     *
     * @param file address
     *
     * @return string which store the content of the input txt file
    */
    std::string readTxt(std::string const& address) const;

    /**
    * create a file with a certain language type suffix
      and write code_ inside the file
    *
    * @return the filename of the file
    */
    std::string createAndWriteFile();

    /**
    * used to change the suffix of the given file which in txt format
    *
    * @param the wanted language such as python and java
    *
    * @return the filename of the file
    */
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

    /**
     * @brief This function is used to execute a command in command line
     * 
     * @param cmd The command that will be executed
     */
    void executeInCmdLine(std::string const& cmd);

    /**
     * @brief This function executes a command in command line and read the content in command line
     * 
     * @param cmd The command that will be executed
     * @return std::string The result of the command (output)
     */
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