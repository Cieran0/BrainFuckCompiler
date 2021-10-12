using System.Collections.Generic;
using System.IO;
using static BrainFuckCompiler.Program;

namespace BrainFuckCompiler
{
    class Compiler
    {
        public static void Compile(List<Token> tokens, string outputPath) 
        {
            List<string> output = new List<string>();
            output.AddRange(GenHeader(tokens));
            foreach (Token t in tokens) 
            {
                switch (t.type) 
                {
                    case TokenType.INC:
                        output.AddRange(INC(t.count));
                        break;
                    case TokenType.DEC:
                        output.AddRange(DEC(t.count));
                        break;
                    case TokenType.NEXT:
                        output.AddRange(NEXT(t.count));
                        break;
                    case TokenType.PREV:
                        output.AddRange(PREV(t.count));
                        break;
                    case TokenType.READ:
                        output.AddRange(READ());
                        break;
                    case TokenType.WRITE:
                        output.AddRange(WRITE());
                        break;
                    case TokenType.LOOPSTART:
                        output.AddRange(LOOPSTART(t.count));
                        break;
                    case TokenType.LOOPEND:
                        output.AddRange(LOOPEND(t.count));
                        break;
                }
            }
            output.Add("        mov     rax, 60                 ");
            output.Add("        xor     rdi, rdi                ");
            output.Add("        syscall                         ");

            File.WriteAllLines(outputPath, output);
        }

        private static List<string> GenHeader(List<Token> tokens) 
        {
            List<string> header = new List<string>();
            bool hasRead  = false;
            bool hasWrite = false;
            foreach (Token t in tokens) { if (t.type == TokenType.READ) { hasRead = true; } else if (t.type == TokenType.WRITE) { hasWrite = true; } }

            header.Add("BITS 64                                 ");
            header.Add("                                        ");
            header.Add("section .data                           ");
            header.Add("                                        ");
            header.Add("section .bss                            ");
            header.Add("    buf:    resb 1                      ");
            header.Add("    mem:    resq 3750                   ");
            header.Add("                                        ");
            header.Add("section .text                           ");
            header.Add("    global _start                       ");
            header.Add("                                        ");
            if (hasRead)
            {
            header.Add("    read:                               ");
            header.Add("        xor     rax,rax                 ");
            header.Add("        xor     rdi,rdi                 ");
            header.Add("        mov     rsi,buf                 ");
            header.Add("        mov     rdx, 1                  ");
            header.Add("        syscall                         ");
            header.Add("        mov     byte[mem+r9], byte [buf]");
            header.Add("        ret                             ");
            header.Add("                                        ");
            }
            if (hasWrite)
            {
            header.Add("    write:                              ");
            header.Add("        mov     rax,1                   ");
            header.Add("        mov     rdx,1                   ");
            header.Add("        mov     rdi,1                   ");
            header.Add("        mov     r10b, byte[mem+r9]      ");
            header.Add("        mov     byte [buf], r10b        ");
            header.Add("        mov     rsi,buf                 ");
            header.Add("        syscall                         ");
            header.Add("        ret                             ");
            header.Add("                                        ");
            }                                                   
            header.Add("    check:                              ");
            header.Add("        cmp     r9, 3750                ");
            header.Add("        jl      c1                      ");
            header.Add("        xor     r9,r9                   ");
            header.Add("    c1:                                 ");
            header.Add("        cmp     r9, 0                   ");
            header.Add("        jnl     c2                      ");
            header.Add("        mov     r9, 3749                ");
            header.Add("    c2:                                 ");
            header.Add("        ret                             ");
            header.Add("                                        ");
            header.Add("    _start:                             ");

            return header;
        }


        private static List<string> READ() => new List<string>()  { "        call    read               " };
        private static List<string> WRITE() => new List<string>() { "        call    write              " };
                                                                                                        
        private static List<string> NEXT(int count) => new List<string>()  { $"        add     r9, {count}              ", "        call    check              " };
        private static List<string> PREV(int count) => new List<string>()  { $"        sub     r9, {count}              ", "        call    check              " };
                                                                             
        private static List<string> INC(int count) => new List<string>()   { $"        add     byte [mem + r9], {count} " };
        private static List<string> DEC(int count) => new List<string>()   { $"        sub     byte [mem + r9], {count} " };

        private static List<string> LOOPSTART(int num) 
        {
            List<string> text = new List<string>();
            text.Add("        mov     al, byte [mem + r9]");
            text.Add("        cmp     al, 0              ");
            text.Add($"        je     LpE{num}           ");
            text.Add($"     LpS{num}:                      ");
            return text;
        }

        private static List<string> LOOPEND(int num)
        {
            List<string> text = new List<string>();
            text.Add("        mov     al, byte [mem + r9]");
            text.Add("        cmp     al, 0              ");
            text.Add($"        jne     LpS{num}           ");
            text.Add($"     LpE{num}:                      ");
            return text;
        }
    }
}
