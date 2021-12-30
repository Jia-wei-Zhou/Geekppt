#ifdef _WIN32
#define popen _popen
#define pclose _pclose
#endif

#include <cstdio>
#include <iostream>
#include <memory>
#include <stdexcept>
#include <string>
#include <array>

void exec(const char* cmd) {
    std::array<char, 128> buffer;
    std::string result;
    std::unique_ptr<FILE, decltype(&pclose)> pipe(popen(cmd, "r"), pclose);
    if (!pipe) {
        throw std::runtime_error("popen() failed!");
    }
}

std::string exec_and_get(const char* cmd) {
    std::array<char, 128> buffer;
    std::string result;
    std::unique_ptr<FILE, decltype(&pclose)> pipe(popen(cmd, "r"), pclose);
    if (!pipe) {
        throw std::runtime_error("popen() failed!");
    }
    while (fgets(buffer.data(), buffer.size(), pipe.get()) != nullptr) {
        result += buffer.data();
    }
    return result;
}

int main() {
    std::string compile_cmd = "g++ cmdTry.cpp -o cmdTry.exe";
    std::string run_cmd = "cmdTry.exe";
    std::string input = " < input.txt";
    run_cmd = run_cmd + input;
    // compile
    exec(&compile_cmd[0]);
    // run
    std::string result = exec_and_get(&run_cmd[0]);
    //std::string result2 = exec(&input[0]);
    std::cout << result << std::endl;
    //std::cout << "cmdTry is over" << std::endl;

    return 0;
}