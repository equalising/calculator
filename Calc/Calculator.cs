using System;
using System.Linq;

namespace Calc
{
    // A class containing the processing logic for the statement
    public class Calculator
    {
        // Field
        private string statement;

        // Constructor
        public Calculator(string arg)
        {
            statement = arg;
        }

        // Methods

        // Calculate statement by order of precedence
        public void CalculateStatement()
        {
            statement = Calculate(Constants.HIGH_PRECEDENCE_OPERATORS);
            statement = Calculate(Constants.PLUS_MINUS);
        }

        // Print solution on console
        // Statement is parsed to eliminate unary operators from one number arguments
        public void PrintSolution()
        {
            Console.WriteLine(ParseNumber((statement)));
        }

        // Calculate statement based on specified operations
        // operations = operations to be processed
        private string Calculate(string operations)
        {
            // process statement if specified operators exist
            // if found, read the left and right numbers and calculate
            // recurse until no specified operators are left and return answer
            if (StatementChecker.OperatorCount(statement, operations) != 0)
            {
                for (int x = 0; x < statement.Length - 1; x++)
                {
                    if (operations.Contains(statement[x]) 
                        && !StatementChecker.IsUnaryOperator(statement[x], statement, x))
                    {
                        statement = CalculateLeftandRight(statement, statement[x], x);
                        statement = Calculate(operations);
                        break;
                    }
                }
            }

            return statement;
        }

        // Reads numbers to the left and right of operator, then performs the calculation
        // Function is called when a valid operator has been found
        // The answer then replaces the substring and the adjusted statement is returned
        private string CalculateLeftandRight(string statement, char operation, int operatorIndex)
        {
            string left = ReadLeft(statement, operatorIndex);
            string right = ReadRight(statement, operatorIndex);
            string sub = left + operation + right;

            return ConsumeOperator(statement, sub, Calculate(left, right, operation)); ;
        }

        // Reads the statement preceding the index location of the NON-UNARY operator
        // SyntaxException is thrown if the character to the IMMEDIATE left is an operator
        private string ReadLeft(string statement, int operatorIndex)
        {
            if (StatementChecker.IsOperator(statement[operatorIndex - 1]))
            {
                throw new SyntaxException();
            }

            int counter = 1;

            for (int index = operatorIndex - 1; index > -1; index--)
            {
                // Checks for beginning of statement, OR the appearance of the next NON-UNARY operator
                // Returns the number BEFORE the operator

                if (index == 0)
                {
                    return statement.Substring(index, counter);
                }
                else if (StatementChecker.IsArithmeticOperator(statement[index], statement, index))
                {
                    return statement.Substring(index + 1, counter - 1);
                }

                counter++;
            }

            return null;
        }

        // Reads the statement following index location of the operator
        // Checks for the end of the statement, OR the appearance of the next NON-unary operator
        // Returns the next number AFTER the operator
        private string ReadRight(string statement, int operatorIndex)
        {
            int counter = 1;

            for (int index = operatorIndex + 1; index < statement.Length; index++)
            {
                if (index == statement.Length - 1)
                {
                    return statement.Substring(operatorIndex + 1, counter);
                }
                else if (StatementChecker.IsArithmeticOperator(statement[index], statement, index))
                {
                    return statement.Substring(operatorIndex + 1, counter - 1);
                }

                counter++;
            }

            return null;
        }

        // A function that replaces a specified subsection of the statement with its answer
        // E.g. "8+5*2+42" returns "8+10+42" 
        public string ConsumeOperator(string statement, string sub, string answer)
        {
            int pos = statement.IndexOf(sub);
            if (pos < 0)
            {
                return statement;
            }

            return statement.Substring(0, pos) + answer + statement.Substring(pos + sub.Length);
        }

        // Calculates a section of the original statement by parsing the supplied arguments
        // left = left hand side of the expression
        // right = right hand side of the expression
        // operation = the operation to be performed on the left and right values
        // A generic exception is thrown if no answer can be calculated
        // An OverflowException is handled at the top level if thrown
        public string Calculate(string left, string right, char operation)
        {
            int? answer = null;
            int x = ParseNumber(left);
            int y = ParseNumber(right);

            switch (operation)
            {
                case '+':
                    answer = checked(x + y);
                    break;
                case '-':
                    answer = checked(x - y);
                    break;
                case '*':
                    answer = checked(x * y);
                    break;
                case '/':
                    answer = checked(x / y);
                    break;
                case '%':
                    answer = checked(x % y);
                    break;
            }

            if (answer != null)
            {
                return answer.ToString();
            }
            else
            {
                throw new Exception("An unknown error when calculating: " + left + operation + right);
            }
        }

        // Parse string into int
        private static int ParseNumber(string num)
        {
            return checked(Int32.Parse(num));
        }
    }
}
