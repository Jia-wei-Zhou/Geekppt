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

std::string CodeEvaluation::readTxt(std::string address) const {
    std::ifstream ifile(address);
    std::ostringstream buf;
    char ch;
    while (buf && ifile.get(ch))
        buf.put(ch);
    return buf.str();
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
int main()
{
    std::string address = "cfileff.txt";
    std::string str;
    enum LanguageType m = PYTHON;

    CodeEvaluation c = CodeEvaluation(m, address);
    str = c.readTxt(address);
    c.createAndWriteFile();
    std::cout << str << std::endl;


}