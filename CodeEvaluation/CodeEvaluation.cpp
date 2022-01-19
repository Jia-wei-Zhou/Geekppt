#include "CodeEvaluation.h"

std::string CodeEvaluation::extractFilename(std::string const& address) {
    int separator_index = address.find_last_of(std::filesystem::path::preferred_separator);
    separator_index = (separator_index == std::string::npos) ? 0 : separator_index + 1;

    int dot_index = address.find_last_of('.');
    std::string filename = address.substr(separator_index, dot_index - separator_index);

    return filename;

}


std::string CodeEvaluation::generateCompileCommand(std::string const& compiler) {
    std::string compileCmd = compiler;
    // PYTHON dose not need compiling
    // There should be defensive sentence when executing cmd (e.g. cmd.length() != 0)
    if (language_ == PYTHON) { return compileCmd; }
    compileCmd.push_back(' ');
    compileCmd += extractFilename(address_);

    switch (language_) {
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
    std::ofstream input_file;
    std::system("mkdir output");
    std::filesystem::path path = "output/" + filename_ + "_input.txt";
    std::string input_filename = path.make_preferred().string();
    // TODO: this shall be replaced by a sub-function (write to file) later
    input_file.open(input_filename, std::ios::out | std::ios::trunc);
    if (input_file.fail()) {
        throw std::runtime_error("Fail to create/open input.txt");
    }

    input_file << input;

    input_file.close();

    std::string input_command = " < " + input_filename;
    return input_command;
}



std::string CodeEvaluation::generateRunCommand(std::string const& filename, std::string const& input) {
    std::string run_command = (PLATFORM == WINDOWS? "" : "./");
    run_command += filename + (input.size() > 0 ? generateInputCommand(input) : "");

    return run_command;
}


void CodeEvaluation::generateCmakeFile(const std::string& project_name,
                                       const std::string& main_file,
                                       const std::string& output_cmake_path,
                                       std::vector<std::string>& libs,
                                       const int cpp_standard,
                                       const std::string& cmake_mini_version) {
    // Determine if the validity of the output cmake lists
    if (!output_cmake_path.ends_with("CMakeLists.txt")) {
        throw std::runtime_error("Error in the path for output CMakeLists.txt");
    }

    std::ofstream output(output_cmake_path);
    if (output.fail()) {
        throw std::runtime_error("Error in opening output files");
    }

    // Generate CMakeLists.txt
    output << "cmake_minimum_required(VERSION " << cmake_mini_version << ")\n";
    output << "project(" << project_name << ")\n\n";
    output << "set(CMAKE_CXX_STANDARD " << cpp_standard << ")\n\n";

    std::vector<std::string> lib_names;
    for (auto&& lib : libs) {
        // TODO - should be replaced by extractFile()
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


void CodeEvaluation::executeInCmdLine(std::string const& cmd) {
    // defend empty compile cmd
    if (cmd.length() == 0) { return; }
    std::array<char, 128> buffer;
    std::string result;
    std::unique_ptr<FILE, decltype(&pclose)> pipe(popen(&cmd[0], "r"), pclose);
    if (!pipe) {
        throw std::runtime_error("popen() failed!");
    }
}


std::string CodeEvaluation::executeAndGetFromCmd(std::string const& cmd) {
    // defend empty compile cmd
    if (cmd.length() == 0) { return ""; }
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


std::string CodeEvaluation::runCode(std::string const& input) {
    try {
        filename_ = extractFilename(address_);
        changeSuffix(language_);
        std::string compileCmd = generateCompileCommand(compiler_);
        executeInCmdLine(compileCmd);
        std::string runCmd = generateRunCommand(filename_, input);
        return executeAndGetFromCmd(runCmd);
    }
    catch(std::runtime_error errors) {
        std::cerr << "Error message:" << errors.what() << '\n';
        throw errors;
    }
}


std::string CodeEvaluation::runCode(std::string const& address, std::string const& input) {
    try {
        address_ = address;
        filename_ = extractFilename(address_);
        changeSuffix(language_);
        std::string compileCmd = generateCompileCommand(compiler_);
        executeInCmdLine(compileCmd);
        std::string runCmd = generateRunCommand(filename_, input);
        return executeAndGetFromCmd(runCmd);
    }
    catch(std::runtime_error errors) {
        std::cerr << "Error message:" << errors.what() << '\n';
        throw errors;
    }
}

std::string CodeEvaluation::readTxt(std::string const& address) const {
    std::ifstream input_file(address);
    if (!input_file.is_open()) {
        throw std::runtime_error("Error opening file");
    }
    return std::string((std::istreambuf_iterator<char>(input_file)), std::istreambuf_iterator<char>());
}

std::string CodeEvaluation::createAndWriteFile() {
    std::string address = changeSuffix(language_);
    if (address == "") {
        return "file not found";
    }
    std::ofstream ofs;
    ofs.open(address, std::ios::out);
    if (!ofs.is_open()) {
        return "file can not be opened";
    }
    ofs << code_ << std::endl;
    ofs.close();
    return address;
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
