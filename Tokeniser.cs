using System.Collections.Generic;
using static BrainFuckCompiler.Program;

namespace BrainFuckCompiler
{
    class Tokeniser
    {
        public static Token[] Tokenise(string input)
        {
            Stack<int> countStack = new Stack<int>();
            List<Token> tokens = new List<Token>();
            int opened = 0;
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '+':
                        tokens.Add(new Token(TokenType.INC));
                        break;
                    case '-':
                        tokens.Add(new Token(TokenType.DEC));
                        break;
                    case '>':
                        tokens.Add(new Token(TokenType.NEXT));
                        break;
                    case '<':
                        tokens.Add(new Token(TokenType.PREV));
                        break;
                    case ',':
                        tokens.Add(new Token(TokenType.READ));
                        break;
                    case '.':
                        tokens.Add(new Token(TokenType.WRITE));
                        break;
                    case '[':
                        tokens.Add(new Token(TokenType.LOOPSTART, opened));
                        countStack.Push(opened);
                        opened++;
                        break;
                    case ']':
                        tokens.Add(new Token(TokenType.LOOPEND, countStack.Pop()));
                        break;
                }
            }
            return tokens.ToArray();

        }
    }
}
