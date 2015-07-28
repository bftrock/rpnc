using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RpnCalculator
{
    class Program
    {
        static private double[] Stack = new double[10];
        static private int StackIndex = 0;

        static void Main(string[] args)
        {
            bool notQuitting = true;
            while (notQuitting)
            {
                Console.Write("> ");
                var x = Console.ReadLine().Trim();
                if (x.Length > 0)
                {
                    switch (x)
                    {
                        case "exit":
                        case "x":
                        case "quit":
                        case "q":
                            notQuitting = false;
                            break;
                        case "clear":
                        case "c":
                            ClearStack();
                            break;
                        case "print":
                        case "pr":
                            PrintStack();
                            break;
                        case "ph":
                            PrintStack(16);
                            break;
                        case "pb":
                            PrintStack(2);
                            break;
                        case "po":
                            PrintStack(8);
                            break;
                        case "help":
                        case "h":
                            PrintHelp();
                            break;
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                        case "^":
                            Arithmatic(x);
                            break;
                        case "!":
                            Factorial();
                            break;
                        case "pi":
                            Push(Math.PI);
                            break;
                        case "e":
                            Push(Math.E);
                            break;
                        case "pop":
                            Console.WriteLine(Pop().ToString());
                            PrintStack();
                            break;
                        default:
                            try
                            {
                                // Numeric input
                                if (new Regex(@"^-*[0-9]*\.*[0-9]*$").Matches(x).Count > 0)
                                {
                                    //Console.WriteLine("Numeric input");
                                    double xd = double.Parse(x);
                                    Push(xd);
                                    break;
                                }
                                // Convert length
                                MatchCollection mc = new Regex(@"cl\((\w+),(\w+)\)").Matches(x);
                                if (mc.Count > 0)
                                {
                                    string inUnits = mc[0].Groups[1].Value;
                                    string outUnits = mc[0].Groups[2].Value;
                                    //Console.WriteLine(string.Format("Convert length, {0} to {1}", inUnits, outUnits));
                                    double y = ConvertLength(inUnits, outUnits);
                                    Push(y);
                                    break;
                                }
                                // Convert angle
                                mc = new Regex(@"ca\(([dr]{1})\)").Matches(x);
                                if (mc.Count > 0)
                                {
                                    string units = mc[0].Groups[1].Value;
                                    //Console.WriteLine(string.Format("Convert angle to {0}", units));
                                    double y = ConvertAngle(units);
                                    Push(y);
                                    break;
                                }
                                // Trig functions
                                mc = new Regex(@"(sin|asin|cos|acos|tan|atan)\(([dr]{1})\)").Matches(x);
                                if (mc.Count > 0)
                                {
                                    string func = mc[0].Groups[1].Value;
                                    string units = mc[0].Groups[2].Value;
                                    double y = Trig(func, units);
                                    Push(y);
                                    break;
                                }
                                // Invalid input
                                Console.WriteLine("Invalid input");
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                    }
                }
            }
        }

        static void Push(double value)
        {
            for (int i = 9; i > 0; i--)
            {
                Stack[i] = Stack[i - 1];
            }
            Stack[0] = value;
            StackIndex++;
            if (StackIndex > 9)
                StackIndex = 9;
            PrintStack();
        }

        static double Pop()
        {
            double value = Stack[0];
            for (int i = 0; i < 9; i++)
            {
                Stack[i] = Stack[i + 1];
            }
            Stack[9] = 0;
            StackIndex--;
            if (StackIndex < 0)
                StackIndex = 0;
            return value;
        }

        static void PrintStack(int b = 10)
        {
            if (StackIndex > 0)
            {
                for (int i = 0; i < StackIndex; i++)
                {
                    Console.WriteLine("[{0}] {1}", i, Convert.ToString((int)Stack[i], b));
                }
            }
            else
            {
                Console.WriteLine("Stack empty");
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("This is a calculator that uses reverse Polish notation (RPN).");
            Console.WriteLine("Here are the supported operators and functions. All results are");
            Console.WriteLine("pushed onto the stack:");
            Console.WriteLine("+: x1 plus x0.");
            Console.WriteLine("-: x1 minus x0.");
            Console.WriteLine("*: x1 multiplied by x0.");
            Console.WriteLine("/: x1 divided by x0.");
            Console.WriteLine("^: x1 raised to the power of x0.");
            Console.WriteLine("cl(inUnits, outUnits): convert length x0 from inUnits to outUnits.");
            Console.WriteLine("  Supported units: in (inch), mm (millimeter), ft (foot), m (meter),");
            Console.WriteLine("  mi (mile), km (kilometer).");
            Console.WriteLine("ca(units): convert angle x0 to units (d (degrees) or r (radians).");
            Console.WriteLine("sin(units): calculate sine of x0 in units (d or r).");
            Console.WriteLine("cos(units): calculate cosine of x0 in units (d or r).");
            Console.WriteLine("tan(units): calculate tangent of x0 in units (d or r).");
            Console.WriteLine("asin(units): calculate arcsine of x0 in units (d or r).");
            Console.WriteLine("acos(units): calculate arccosine of x0 in units (d or r).");
            Console.WriteLine("atan(units): calculate arctangent of x0 in units (d or r).");
            Console.WriteLine("help or h: print this help message.");
            Console.WriteLine("print or p: print the stack contents.");
            Console.WriteLine("clear or c: clear the stack contents.");
            Console.WriteLine("pop: pop the first element off the stack and discard.");
        }

        static void ClearStack()
        {
            for (int i = 0; i < 10; i++)
            {
                Stack[i] = 0;
            }
            StackIndex = 0;
            Console.WriteLine("Stack cleared");
        }

        static void Arithmatic(string oper)
        {
            double x1 = Pop();
            double x2 = Pop();
            double y = 0;
            switch (oper)
            {
                case "+":
                    y = x2 + x1;
                    break;
                case "-":
                    y = x2 - x1;
                    break;
                case "*":
                    y = x2 * x1;
                    break;
                case "/":
                    y = x2 / x1;
                    break;
                case "^":
                    y = Math.Pow(x2, x1);
                    break;
            }
            Push(y);
        }

        static void Factorial()
        {
            int x1 = (int)Pop();
            double y = x1;
            for (int i = x1 - 1; i > 1; i--)
            {
                y *= i;
            }
            Push(y);
        }

        static double ConvertLength(string inUnits, string outUnits)
        {
            double x = Pop();
            double x_temp = 0;
            // Convert from input units to meters
            switch (inUnits)
            {
                case "in":
                    x_temp = 0.0254 * x;
                    break;
                case "mm":
                    x_temp = 0.001 * x;
                    break;
                case "ft":
                    x_temp = 0.3048 * x;
                    break;
                case "m":
                    x_temp = x;
                    break;
                case "mi":
                    x_temp = 1609.344 * x;
                    break;
                case "km":
                    x_temp = 1000 * x;
                    break;
                default:
                    throw new Exception("Invalid input units");
            }
            // Convert from meters to output units
            double y = 0;
            switch (outUnits)
            {
                case "in":
                    y = 39.37007874015748 * x_temp;
                    break;
                case "mm":
                    y = 1000 * x_temp;
                    break;
                case "ft":
                    y = 3.280839895013123 * x_temp;
                    break;
                case "m":
                    y = x_temp;
                    break;
                case "mi":
                    y = 6.21371192237334e-4 * x_temp;
                    break;
                case "km":
                    y = 0.001 * x_temp;
                    break;
                default:
                    throw new Exception("Invalid output units");
            }
            return y;
        }

        static double ConvertAngle(string units)
        {
            double x = Pop();
            double y = 0;
            if (units == "d")
            {
                y = x * 180.0 / Math.PI;
            }
            else
            {
                y = x * Math.PI / 180.0;
            }
            return y;
        }

        static double Trig(string func, string units)
        {
            double x = Pop();
            double y = 0;
            switch (func)
            {
                case "cos":
                    if (units == "d")
                        x = x * Math.PI / 180;
                    y = Math.Cos(x);
                    break;
                case "acos":
                    y = Math.Acos(x);
                    if (units == "d")
                        y = y * 180 / Math.PI;
                    break;
                case "sin":
                    if (units == "d")
                        x = x * Math.PI / 180;
                    y = Math.Sin(x);
                    break;
                case "asin":
                    y = Math.Asin(x);
                    if (units == "d")
                        y = y * 180 / Math.PI;
                    break;
                case "tan":
                    if (units == "d")
                        x = x * Math.PI / 180;
                    y = Math.Tan(x);
                    break;
                case "atan":
                    y = Math.Atan(x);
                    if (units == "d")
                        y = y * 180 / Math.PI;
                    break;
                default:
                    break;
            }
            return y;
        }
    }
}
