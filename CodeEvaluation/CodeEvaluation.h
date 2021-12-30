#include <string>
#include <fstream>

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

public:
    CodeEvaluation(LanguageType language, std::string address): 
    language_(language), address_(address) {
        code_ = readTxt(address_);
    }

    /* read from a txt file and return its content as string
    */
    std::string readTxt(std::string address) const;

    /* create a file with a certain language type suffix
       and write code_ inside the file 
       e.g. address_ filename is 1.txt, language_ is CPP
       should write to 1.cpp file in the same directory */
    bool createAndWriteFile();

    /* change suffix .txt to certain type */
    bool changeSuffix(LanguageType language);

    /* if not use cmake, generate compile command from a certain language and compiler
    e.g. compiler = "g++" should return "g++ filename.cpp -o filename"
    */
    std::string generateCompileCommand(std::string compiler);

    // /* if use cmake, HH's cmake is poor, Luo will write this */ 
    std::string generateCmakeFile(LanguageType language);

    /* This is for unix env (defined by macro)
    e.g. filename = "main", input = "ZhaiQianhao" 
    should generate "./main < &ZhaiQianhao" 
     */
    std::string generateRunCommand(std::string filename, std::string input);

    /* execute certain command in command line */
    void executeInCmdLine(std::string cmd);

    /* already finished by Zhou */
    std::string executeAndGetFromCmd(std::string cmd);

    /* use the above methods */
    bool runCode();


};