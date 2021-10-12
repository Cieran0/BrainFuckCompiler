using System;
using static BrainFuckCompiler.Tokeniser;

namespace BrainFuckCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            Compiler.Compile(Tokenise(input), @"eee.asm");
        }

        public enum TokenType 
        { 
            INC,
            DEC,
            NEXT,
            PREV,
            READ,
            WRITE,
            LOOPSTART,
            LOOPEND
        }

        public struct Token 
        {
            public TokenType type;
            public int count;

            public Token(TokenType type, int count = 0) 
            {
                this.type = type;
                this.count = count;
            }
        }
    }
}
