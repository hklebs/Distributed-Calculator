using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Client
{
    public partial class CalculatorForm : Form
    {
        private CalculatorClient client;
        private bool canEnterOp = false;
        private List<string> numbers = new List<string>();
        private List<string> operators = new List<string>();

        public CalculatorForm(CalculatorClient client)
        {
            this.client = client;
            InitializeComponent();
        }

        // Number button handling
        private void numberButton_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string num = b.Text;

            if (!canEnterOp) //|| (numbers.Count > 0 && operators.Count == 0))
            {
                numbers.Add(num);
            }
            else
            {
                numbers[numbers.Count - 1] +=  num;
            }
            inputBox.Text += num;
            canEnterOp = true;
        }

        // Operator button handling
        private void operatorButton_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string op = b.Text;
            
            if (canEnterOp)
            {
                operators.Add(op);
                // Do not add spaces if using the power operator
                if (op.Equals("^"))
                {
                    inputBox.Text += op;
                }
                else
                {
                    inputBox.Text += " " + op + " ";
                }
                canEnterOp = false;
            }
        }

        // Changes current number from positive to negative and vice-versa
        private void changeSignButton_Click(object sender, EventArgs e)
        {
            if (canEnterOp && IsStringNegative())
            {
                // Removes negative sign from beginning of most recently entered number
                numbers[numbers.Count - 1] = numbers.Last().Substring(1);
                // same but for inputBox
                inputBox.Text = inputBox.Text.Remove(inputBox.Text.LastIndexOf("-"), 1);
            }
            else if (canEnterOp)
            {
                numbers[numbers.Count - 1] = "-" + numbers.Last();
                int lastIndexOf = inputBox.Text.Length - (numbers.Last().Length - 1);
                inputBox.Text = inputBox.Text.Insert(lastIndexOf, "-");
            }
        }

        // Is this string representation of a double a negative number?
        private bool IsStringNegative()
        {
            return numbers.Count > 0 && numbers[numbers.Count - 1].StartsWith("-");
        }

        // CLEAR
        private void clearButton_Click(object sender, EventArgs e)
        {
            inputBox.Invalidate();
            inputBox.Clear();
            numbers.Clear();
            operators.Clear();
            canEnterOp = false;
        }

        // Evaluates all current math
        private void equalsButton_Click(object sender, EventArgs e)
        {
            if(canEnterOp)
            {
                List<double> doubles = numbers.ConvertAll(StringToDouble);
                // Evaluate calculations
                double answer = evaluateLists(doubles, operators);

                // Add calculations to history
                historyList.Items.Add(inputBox.Text + " = " + answer);

                // Clear input box and lists
                clearButton_Click(null, null);
            }
        }

        // Converts given string to a double
        public static double StringToDouble(string s)
        {
            return Convert.ToDouble(s);
        }

        // Evaluates a list of numbers and a list of commands to a single number
        private double evaluateLists(List<double> nums, List<string> ops)
        {
            while(operators.Count != 0)
            {
                if(ops.Contains("*") || ops.Contains("/") || ops.Contains("%") || ops.Contains("^"))
                {
                    string op = FindFirst(ops);
                    int i = ops.IndexOf(op);
                    nums[i] = EvaluateOperator(op, nums[i], nums[i+1]);
                    nums.RemoveAt(i+1);
                    ops.Remove(op);
                }
                else
                {
                    nums[0] = EvaluateOperator(ops[0], nums[0], nums[1]);
                    nums.RemoveAt(1);
                    ops.RemoveAt(0);
                }
            }
            return nums[0];
        }

        // Finds first instance of *, /, %, or ^ in a list, for order of operations
        private static string FindFirst(List<string> ops)
        {
            return ops.FirstOrDefault(op => op.Equals("*") || op.Equals("/") || op.Equals("%") || op.Equals("^"));
        }

        // Evaluates two numbers and an operator server side
        private double EvaluateOperator(string cmd, double n1, double n2)
        {
            double result = 0;

            switch (cmd)
            {
                case "+":
                    result = client.Add(n1, n2);
                    break;
                case "-":
                    result = client.Subtract(n1, n2);
                    break;
                case "*":
                    result = client.Multiply(n1, n2);
                    break;
                case "/":
                    result = client.Divide(n1, n2);
                    break;
                case "%":
                    result = client.Modulo(n1, n2);
                    break;
                case "^":
                    result = client.Power(n1, n2);
                    break;
            }
            return result;
        }
    }
}
