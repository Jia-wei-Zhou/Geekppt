#include "CodeEvaluation.h"

/*  Comment: this function can be used in function generateCmakeFile, 
    whereas it is recommended to use the same method in generateCmakeFile
    i.e. preferred separator to distinquish between Unix and Windows. 
    -- Jiawei Zhou  */
std::string CodeEvaluation::extractFilename(std::string const& address) {
    std::string filename = "";
    for (auto it = address.begin(); it != address.end(); ++it) {
        if (*it == '/' || *it == '\\') {
            filename = "";
        }
        else {
            filename.push_back(*it);
        }
    }

    if (find(filename.begin(), filename.end(), '.') != filename.end()) {
        return filename.substr(0, find(filename.begin(), filename.end(), '.') - filename.begin());
    }
    filename_ = filename;
    return filename;
}


std::string CodeEvaluation::generateCompileCommand(std::string const& compiler) {
    std::string compileCmd = compiler;
    // PYTHON dose not need compiling
    // There should be defensive sentence when executing cmd (e.g. cmd.length() != 0)
    if (language_ == PYTHON) {return compileCmd;}
    compileCmd.push_back(' ');
    compileCmd += extractFilename(address_);

    switch(language_) {
        case JAVA: 
            compileCmd += ".java";
            return compileCmd;
        case CPP: 
            compileCmd += ".cpp";
            break;
        default: 
            break;
    }

    compileCmd += " -o ";
    compileCmd += filename_;
    return compileCmd;
}


std::string CodeEvaluation::generateInputCommand(std::string const& input) {
    if (PLATFORM == WINDOWS) {
        return generateWindowsInputCommand(input);
    }
    else if (PLATFORM == UNIX) {
        return generateUnixInputCommand(input);
    }
}


std::string CodeEvaluation::generateWindowsInputCommand(std::string const& input) {
    std::ofstream input_file;
    std::string input_filename = filename_ + "_input.txt";
    input_file.open(input_filename, std::ios::out | std::ios::trunc);
    if (input_file.fail()) {
        throw std::runtime_error("Fail to create/open input.txt");
    }

    input_file << input;

    input_file.close();
    if (input_file.good()) {
        throw std::runtime_error("Fail to close input.txt");
    }
    return " < " + input_filename;
}


std::string CodeEvaluation::generateUnixInputCommand(std::string const& input) {
    return " < &" + input;
}


std::string CodeEvaluation::generateRunCommand(std::string const& filename, std::string const& input) {
    std::string run_command = "";
    run_command = filename + generateInputCommand(input);

    return run_command;
}


void CodeEvaluation::generateCmakeFile(const std::string& project_name,
                                       const std::string& main_file,
                                       const std::string& output_cmake_path,
                                       std::vector<std::string>& libs,
                                       const int cpp_standard,
                                       const std::string& cmake_mini_version) {
    // Determine if the validity of the output cmake lists
    if(!output_cmake_path.ends_with("CMakeLists.txt")) {
        throw std::runtime_error("Error in the path for output CMakeLists.txt");
    }

    std::ofstream output(output_cmake_path);
    if(output.fail()) {
        throw std::runtime_error("Error in opening output files");
    }

    // Generate CMakeLists.txt
    output << "cmake_minimum_required(VERSION " << cmake_mini_version << ")\n";
    output << "project(" << project_name << ")\n\n";
    output << "set(CMAKE_CXX_STANDARD " << cpp_standard << ")\n\n";

    std::vector<std::string> lib_names;
    for(auto&& lib : libs) {
        /*  Comment: this part can be extracted and replaced 
            with a separate function in the future -- Jiawei Zhou*/
        int separator_index = lib.find_last_of(std::filesystem::path::preferred_separator);
        separator_index = (separator_index == std::string::npos) ? 0 : separator_index + 1;

        int dot_index = lib.find_last_of('.');
        std::string lib_name = lib.substr(separator_index, dot_index - separator_index);
        lib_names.push_back(lib_name);

        output << "add_library(" << lib_name << " " << lib << ")\n";
    }

    output << "\nadd_executable(main " << main_file << ")\n";

    output << "\ntarget_link_libraries(main ";
    std::for_each(lib_names.begin(), lib_names.end(), [&](auto&& lib) { output << lib << " "; });
    output << ")\n";

    // Finish generating CMakeLists.txt
    output.close();
}


void CodeEvaluation::executeInCmdLine(std::string cmd) {
    // defend empty compile cmd (e.g. language_ is PYTHON)
    if (cmd.length() == 0) {return;}
    std::array<char, 128> buffer;
    std::string result;
    std::unique_ptr<FILE, decltype(&pclose)> pipe(popen(&cmd[0], "r"), pclose);
    if (!pipe) {
        throw std::runtime_error("popen() failed!");
    }
}


std::string CodeEvaluation::executeAndGetFromCmd(std::string cmd) {
    // defend empty compile cmd (e.g. language_ is PYTHON)
    if (cmd.length() == 0) {return "";}
    std::array<char, 128> buffer;
    std::string result;
    std::unique_ptr<FILE, decltype(&pclose)> pipe(popen(&cmd[0], "r"), pclose);
    if (!pipe) {
        throw std::runtime_error("popen() failed!");
    }
    while (fgets(buffer.data(), buffer.size(), pipe.get()) != nullptr) {
        result += buffer.data();
    }
    return result;
}

std::string CodeEvaluation::readTxt(std::string address) const {
    std::ifstream input_file(address);
    if (!input_file.is_open()) {
        std::cerr << "Could not open the file - '"
            << address << "'" << std::endl;
        exit(EXIT_FAILURE);
    }
    return std::string((std::istreambuf_iterator<char>(input_file)), std::istreambuf_iterator<char>());
}

bool CodeEvaluation::createAndWriteFile() {
    std::string address = changeSuffix(language_);
    if (address == "") {
        return false;
    }
    std::ofstream ofs;
    ofs.open(address, std::ios::out);
    if (!ofs.is_open()) {
        return false;
    }
    ofs << code_ << std::endl;
    ofs.close();
    return true;
}
std::string CodeEvaluation::changeSuffix(LanguageType language) {
    std::string address = "";
    if (language == CPP) {
        address = address_.replace(address_.length() - 3, address_.length(), "cpp");
    }
    if (language == JAVA) {
        address = address_.replace(address_.length() - 3, address_.length(), "java");
    }
    if (language == PYTHON) {
        address = address_.replace(address_.length() - 3, address_.length(), "py");
    }
    return address;
}
