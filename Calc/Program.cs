using System;

namespace Calc
{
    // A simple program to calculate basic mathematical statements using 
    // +, -, *, /, and % operations
    // Command line argument is read from the user and checked before calculation
    // The result is then printed to the user
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (!StatementChecker.IsValidArgument(args))
                {
                    Console.WriteLine(Constants.INVALID);
                }
                else
                {
                    Calculator calculator = new Calculator(args[0]);
                    calculator.CalculateStatement();
                    calculator.PrintSolution();
                }
            }
            catch (SyntaxException)
            {
                Console.WriteLine(Constants.INVALID);
            }
            catch (OverflowException)
            {
                Console.WriteLine(Constants.OVERFLOW);
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine(Constants.ZERO_DIVISION);
            }
        }
    }
}
