using System.Diagnostics;
using System.IO;
using static BrainFuckCompiler.Tokeniser;

namespace BrainFuckCompiler
{
    class Program
    {
 
        static void Main(string[] args)
        {
            if (args.Length > 0) 
            {
                if (File.Exists(args[0])) 
                {
                    string outputPath = args[0].Split('.')[0] + ".asm";
                    Compiler.Compile(Condenser.Condense(Tokenise(File.ReadAllText(args[0]))), outputPath);
                    Process.Start("nasm", $"-f elf64 {outputPath}");
                    Process.Start("ld", $"-o {outputPath.Replace(".asm","")} {outputPath.Replace(".asm", ".o")}");
                }
            }
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

            public Token(TokenType type, int count = 1) 
            {
                this.type = type;
                this.count = count;
            }
        }
    }
}
