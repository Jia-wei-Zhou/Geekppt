import re

inputFileName = "input.md"

with open(inputFileName, "r", encoding = "utf-8") as f:
    sourceText = f.read()

codeList = re.findall(r"```[a-z]*\n[\s\S]*?\n```", sourceText)

print (f"{len(codeList)} piece(s) of code detected in file {inputFileName}")
for i in range(0, len(codeList)):
    print(f"the {i+1}th code piece is \n {codeList[i]} \n")