import asyncio
from argparse import ArgumentParser
import pypandoc
import re

def parse_args():
    parser = ArgumentParser()
    parser.add_argument('input', help='input file')
    parser.add_argument('output', help='output file')
    args = parser.parse_args()
    return args
    


if __name__ == '__main__':
    args = parse_args()
    print(f'input file is {args.input}')
    print(f'output file is {args.output}\n')

    with open(args.input, "r", encoding = "utf-8") as f:
        sourceText = f.read()
    codeList = re.findall(r"```[a-z]*\n[\s\S]*?\n```", sourceText)

    print (f"{len(codeList)} piece(s) of code detected in file {args.input}")
    for i in range(0, len(codeList)):
        print(f"the {i+1}th code piece is \n {codeList[i]} \n")
    output = pypandoc.convert_file(args.input, 'pptx', outputfile = args.output)


