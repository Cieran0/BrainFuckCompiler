using System;
using System.Collections.Generic;
using System.Text;
using static BrainFuckCompiler.Program;

namespace BrainFuckCompiler
{
    class Condenser
    {
        public static List<Token> Condense(List<Token> tokens) 
        {
            bool condensed = true;

            while (condensed)
            {
                condensed = false;
                for (int i = 0; i < tokens.Count-1; i++)
                {
                    if (tokens[i].type == tokens[i + 1].type && (int)tokens[i].type < 4) {
                        condensed = true; 
                        tokens[i] = new Token(tokens[i].type, tokens[i].count + tokens[i + 1].count);
                        tokens.RemoveAt(i + 1);
                        break; 
                    }
                }
            }
            return tokens;
        }
    }
}
