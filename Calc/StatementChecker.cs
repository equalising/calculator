using System.Linq;

namespace Calc
{
    // A class containing utility functions for statement checking
    public class StatementChecker
    {
        // Checks whether the specified character is a number
        public static bool IsNumber(char token)
        {
            return char.IsDigit(token);
        }

        // Checks if the specified character is among:
        // '+' , '-' , '*', '/' or '%'
        public static bool IsOperator(char token)
        {
            return Constants.VALID_OPERATORS.Contains(token);
        }

        // Check if specified operation is an unary operator
        // token = operation character to test
        // statement = the statement containing the operator
        // index = index positon of operator
        public static bool IsUnaryOperator(char token, string statement, int index)
        {
            if (!IsPlusMinus(token))
            {
                return false;
            }
            else if (IsPlusMinus(token) && index == 0)
            {
                return IsNumber(statement[1]);
            }
            else if (IsPlusMinus(token) && index < statement.Length - 1)
            {
                return IsOperator(statement[index - 1])
                    && IsNumber(statement[index + 1]);
            }
            else
            {
                return false;
            }
        }

        // Check whether the token is an arithmetic operator
        public static bool IsArithmeticOperator(char token, string statement, int operatorIndex)
        {
            return IsOperator(token) && !IsUnaryOperator(token, statement, operatorIndex);
        }

        // Check whether the token is a plus or minus
        public static bool IsPlusMinus(char token)
        {
            return Constants.PLUS_MINUS.Contains(token);
        }

        // Initial error check of command line argument:
        // Returns true of there is only one argument,
        // the last character is not an operator,
        // is comprised of only operators and numbers, and
        // has no more than 3 operators in a row, or 2 operators at the beginning
        public static bool IsValidArgument(string[] args)
        {
            return args.Length == 1
                && !IsOperator(args[0].Last()) 
                && HasValidTerms(args[0]) 
                && HasValidOperations(args[0]);
        }

        // Check if the argument contains only numbers or operators
        private static bool HasValidTerms(string arg)
        {
            foreach (char c in arg)
            {
                if (!(IsNumber(c) || IsOperator(c)))
                {
                    return false;
                }
            }

            return true;
        }

        // Checks for valid operations
        // Returns true if there are less than 3 operators in sequence
        // SyntaxException thrown if first two characters are operators, OR
        // if the first is non-unary operator
        public static bool HasValidOperations(string arg)
        {
            if ((IsOperator(arg[0]) && IsOperator(arg[1]))
                || IsArithmeticOperator(arg[0], arg, 0))
            {
                throw new SyntaxException();
            }

            int max = 0, counter = 0;

            foreach (char c in arg)
            {
                if (IsOperator(c))
                {
                    counter++;
                }
                else
                {
                    if (counter > max)
                    {
                        max = counter;
                        counter = 0;
                    }
                    else
                    {
                        counter = 0;
                    }
                }
            }

            return max < 3;
        }

        // A function that returns the number of specified operators
        // within a mathematical statement that are NOT unary operators
        // statement = mathematical statement
        // operators = operators to look for
        public static int OperatorCount(string statement, string operators)
        {
            int counter = 0;

            for (int x = 0; x < statement.Length - 1; x++)
            {
                if (operators.Contains(statement[x]) && !IsUnaryOperator(statement[x], statement, x))
                {
                    counter++;
                }
            }

            return counter;
        }
    }
}
