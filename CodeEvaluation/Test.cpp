#include "CodeEvaluation.h"

int main() {
    /* ================= test 1 - readTxt(), createAndWriteFile() and changeSuffix() =======================*/
    std::cout << "----------------------- test 1 ---------------------------\n\n";

    CodeEvaluation myCode(CPP, "sampleText1.txt");

    std::string read_filename("sampleText1.txt");
    std::cout << "Result for readTxt() from file " << read_filename << " is: \n\n";
    std::cout << myCode.readTxt(read_filename);

    std::cout << "\n\n\n";

    std::string write_filename = myCode.createAndWriteFile();
    std::cout << "Content in output file " << write_filename << " is: \n\n";
    std::cout << myCode.readTxt(write_filename);

    std::cout << "\n\n\n";

    read_filename = "sampleText2.txt";
    myCode.set_address(read_filename);

    std::cout << "Result for readTxt() from file " << read_filename << " is: \n\n";
    std::cout << myCode.readTxt(read_filename);

    std::cout << "\n\n\n";

    /* =============== test 2 - generateCompileCommand(), generateRunCommand() and generateInputCommand() =================*/
    std::cout << "----------------------- test 2 ---------------------------\n\n";

    std::string compile_command = myCode.generateCompileCommand("g++");
    std::cout << "Compile command generated is: " << compile_command << "\n\n";

    std::string input_command = myCode.generateInputCommand("1234");

    std::string run_command = myCode.generateRunCommand(myCode.get_filename(), "3456");
    std::cout << "Run command generated is: " << run_command << "\n\n";

    /* ========================== test 3 - generateCmakeFile() ================================ */
    std::cout << "----------------------- test 3 ---------------------------\n\n";
    std::cout << "\n\n\n";

    // lwx ziji laixie

    /* ===================== test 4 - executeInCmdLine() and executeAndGetFromCmd() ======================*/
    std::cout << "----------------------- test 4 ---------------------------\n\n";

    myCode.executeInCmdLine("mkdir input"); // this should create a folder 'input'
    std::string result = myCode.executeAndGetFromCmd("dir");
    std::cout << "Result for command 'dir' is: \n\n" << result;

    std::cout << "\n\n\n";

    /* ========================= test 5 - runCode() =========================*/
    std::cout << "----------------------- test 5 ---------------------------\n\n";

    myCode.set_address("sampleCode1.txt");
    std::string code_result_1 = myCode.runCode("");
    std::cout << "Result for sampleCode1 is: " << code_result_1 << "\n\n";

    std::string code_result_2 = myCode.runCode("sampleCode2.txt", "1234");
    std::cout << "Result for sampleCode2 is: " << code_result_2 << "\n\n";

    return 0;
}