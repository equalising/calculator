using System;

namespace Calc
{
    // A custom exception class used to handle incorrect mathematical statements
    public class SyntaxException : Exception
    {
        public SyntaxException() : base()
        {
        }
    }
}
