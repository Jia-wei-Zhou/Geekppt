import asyncio
from argparse import ArgumentParser

def parse_args():
    parser = ArgumentParser()
    parser.add_argument('input', help='input file')
    parser.add_argument('output', help='output file')
    args = parser.parse_args()
    return args
    


if __name__ == '__main__':
    args = parse_args()
    print(args.input)
    print(args.output)

