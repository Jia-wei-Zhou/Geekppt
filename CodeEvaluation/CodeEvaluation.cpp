#include "CodeEvaluation.h"
#include <fstream>
#include <stdexcept>
#include <filesystem>

void CodeEvaluation::executeInCmdLine(std::string cmd) {
    std::array<char, 128> buffer;
    std::string result;
    std::unique_ptr<FILE, decltype(&pclose)> pipe(popen(&cmd[0], "r"), pclose);
    if (!pipe) {
        throw std::runtime_error("popen() failed!");
    }
}

std::string CodeEvaluation::executeAndGetFromCmd(std::string cmd) {
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
