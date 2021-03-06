#include "CodeEvaluation.h"
#include <string>

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

    std::string project_name = "Chess";
    std::filesystem::path main_file = "./users/ChessMain.cpp";
    std::filesystem::path cmake_filename = "../CMakeLists.txt";
    std::vector<std::filesystem::path> libs = {"./Bishop.cpp", "./Queen.cpp", "./Rook.cpp", "./King.cpp",
                                               "./Pawn.cpp", "Knight.cpp"};

    std::vector<std::string> preferred_libs;
    std::for_each(libs.begin(), libs.end(),
                  [&](auto&& path) { preferred_libs.push_back(path.make_preferred().string()); });

    CodeEvaluation::generateCmakeFile(project_name, main_file.make_preferred().string(), 
                                      cmake_filename.make_preferred().string(),
                                      preferred_libs, 20, "3.10");

    std::cout << "CMakeLists.txt has been generated, the path is ";
    std::cout << std::quoted(absolute(cmake_filename).make_preferred().string()) << std::endl;

    std::cout << "\n\n\n";

    /* ===================== test 4 - executeInCmdLine() and executeAndGetFromCmd() ======================*/
    std::cout << "----------------------- test 4 ---------------------------\n\n";

    myCode.executeInCmdLine("mkdir input"); // this should create a folder 'input'
    std::string result = myCode.executeAndGetFromCmd((PLATFORM == WINDOWS ? "dir" : "ls"));
    std::cout << "Result for command 'dir' is: \n\n" << result;

    std::cout << "\n\n\n";

    /* ========================= test 5 - runCode() =========================*/
    std::cout << "----------------------- test 5 ---------------------------\n\n";

    myCode.set_address("sampleCode1.txt");
    std::string code_result_1 = myCode.runCode("");
    std::cout << "Result for sampleCode1 is: " << code_result_1 << "\n\n";
    
    std::string code_result_2 = myCode.runCode("sampleCode2.txt", "1234");
    std::cout << "Result for sampleCode2 is: " << code_result_2 << "\n\n";

    int a;
    std::cin >> a;
    return 0;
}