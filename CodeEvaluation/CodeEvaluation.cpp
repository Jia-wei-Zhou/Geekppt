#include "CodeEvaluation.h"

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