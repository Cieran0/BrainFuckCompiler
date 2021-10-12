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
                    string outputPath = args[0].Split('.')[0];
                    string nasmPath = outputPath + ".asm";
                    string ldPath = outputPath + ".o";
                    Compiler.Compile(Condenser.Condense(Tokenise(File.ReadAllText(args[0]))), nasmPath);
                    while (!Process.Start("nasm", $"-f elf64 {nasmPath}").HasExited) { }

                    while (!Process.Start("ld", $"-o {outputPath} {ldPath}").HasExited) { }

                    if (args.Length > 1)
                    {
                        if (args[1] == "-k")
                        {
                            System.Environment.Exit(0);
                        }
                    }
                    
                    File.Delete(nasmPath); File.Delete(ldPath);
                    }
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
