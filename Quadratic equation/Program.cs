using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quadratic_equation
{
    internal class Program
    {        
        static private int origRow = 0;    
        static public IDictionary<string, string> dict = new Dictionary<string, string>();
        static private Dictionary<string, int> values = new Dictionary<string, int>()
        {
            {"a",0},
            {"b",0},
            {"c",0}
        };
        public enum Severity
        {
            Error,
            Warning
        };

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Write("a * x^2 + b * x + c = 0\nВведите значение\n>a: \n b: \n c: \nДля завершения ввода значения нажмите пробел\nДля решения нажмите ENTER");
            Console.SetCursorPosition(4, 2);
            int rowSum = 0;              
            while (true)
            {
                //KeyPressEventArgs e = new KeyPressEventArgs(Console.ReadKey().KeyChar);
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    rowSum += 1;
                    if (rowSum + origRow > 4)
                        rowSum = 2;
                    if (rowSum + origRow < 2)
                        rowSum = 2;
                    //WriteAt($"{GetString(rowSum + origRow)}", 0, rowSum);                    
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    rowSum -= 1;
                    if (rowSum + origRow < 2)
                        rowSum = 4;
                    //WriteAt($"{GetString(rowSum + origRow)}", 0, rowSum);
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (values.Count < 3)
                        dict.Add("Не все переменные заполнены", "Решение не возможно");
                    break;
                }
                else
                {
                    string input = keyInfo.KeyChar.ToString();
                    ConsoleKeyInfo stoKey = Console.ReadKey();
                    while (stoKey.Key != ConsoleKey.Spacebar)
                    {
                        input += stoKey.KeyChar;
                        stoKey = Console.ReadKey();
                    }
                    if(rowSum == 0)
                        rowSum = 2;
                    ReadValue(input, rowSum + origRow);                    
                }                    
                WriteAt($"{GetString(rowSum + origRow)}", rowSum, 0);
            }
            Console.SetCursorPosition(0, 7);
            Console.WriteLine("===================================");
            if (dict.Count > 0)
            {
                FormData("Во время считывания данных произошли ошибки:", Severity.Error, dict);
                Console.WriteLine("Дальнейшее решение не возможно");
                Console.WriteLine("Для выхода нажмите любую клавишу");
                Console.Read();
                return;
            }
            try
            { 
                var res = Discrimenamt(); 
                for(int i = 0; i < res.Count; i++)
                    Console.WriteLine(res[i]);
            }
            catch { }

            Console.Read();
        }

        static void ReadValue(string inputString, int row)
        {
            var argValue = 0;
            switch (row)
            {
                case 2:
                    try { 
                        argValue = int.Parse(inputString);
                        try
                        { dict.Remove("a"); }
                        catch { }
                    }
                    catch
                    {
                        try { dict.Add("a", inputString); }
                        catch { dict["a"] = $"{dict["a"]}, {inputString}"; }
                        argValue = 0;
                    }
                    values["a"] = argValue;                    
                    break;
                case 3:
                    try { 
                        argValue = int.Parse(inputString);
                        try
                        { dict.Remove("b"); }
                        catch { }
                    }
                    catch
                    {
                        try { dict.Add("b", inputString); }
                        catch { dict["b"] = $"{dict["b"]}, {inputString}"; }
                        argValue = 0;
                    }
                    values["b"] = argValue;                    
                    break;
                case 4:
                    try { 
                        argValue = int.Parse(inputString);
                        try { dict.Remove("c"); }
                        catch { }
                    }
                    catch
                    {
                        try { dict.Add("c", inputString); }
                        catch { dict["c"] = $"{dict["c"]}, {inputString}"; }
                        argValue = 0;
                    }
                    values["c"] = argValue;
                    
                    break;
                default:
                    break;
            }
        }
        static void WriteAt(string s, int x, int y)
        {
            Console.Clear();
            int position = origRow + x;
            if(position == 0)
            {
                position = 2;
            }
            try
            {
                Console.Write(s);
                if (position >= 2 && position < 5)
                {
                    Console.SetCursorPosition(4, position);
                }             
            }
            catch
            {
            }
        }
        static StringBuilder GetString(int row)
        {
            StringBuilder sb = new StringBuilder();
            switch (row)
            {
                case 2:
                    sb.Append($"a * x^2 + b * x + c = 0\nВведите значение\n>a: {values["a"]}\n b: {values["b"]}\n c: {values["c"]}\nДля завершения ввода значения нажмите пробел\nДля решения нажмите ENTER");
                    break;
                case 3:
                    sb.Append($"a * x^2 + b * x + c = 0\nВведите значение\n a: {values["a"]}\n>b: {values["b"]}\n c: {values["c"]}\nДля завершения ввода значения нажмите пробел\nДля решения нажмите ENTER");
                    break;
                case 4:
                    sb.Append($"a * x^2 + b * x + c = 0\nВведите значение\n a: {values["a"]}\n b: {values["b"]}\n>c: {values["c"]}\nДля завершения ввода значения нажмите пробел\nДля решения нажмите ENTER");
                    break;
                default:
                    break;
            }
            
            return sb;
        } 
        public static void FormData(string message, Severity severity, IDictionary<string, string> data)
        {
            if (severity == Severity.Warning)
                Console.BackgroundColor = ConsoleColor.Yellow;
            else
                Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"{message}");
            var keys = data.Keys.ToList();
            for(int i = 0; i < keys.Count; i++)
            {
                Console.WriteLine($"{keys[i]} = {data[keys[i]]}");
            }
        }

        static List<double> Discrimenamt()
        {
            List<double> result = new List<double>();
            double x1 = 0, x2 = 0;
            var discriminant = Math.Pow(values["b"], 2) - 4 * values["a"] * values["c"];
            if (discriminant < 0)
            {
                dict.Add("х1", "решений нет");
                dict.Add("х2", "решений нет");
                throw new NonDecision("Вещественных значений не найдено");
            }
            else
            {
                if (discriminant == 0) //квадратное уравнение имеет два одинаковых корня
                {                    
                    result.Add(x1 = -values["b"] / (2 * values["a"]));
                }
                else //уравнение имеет два разных корня
                {
                    result.Add(x1 = (-values["b"] + Math.Sqrt(discriminant)) / (2 * values["a"]));
                    result.Add(x2 = (-values["b"] - Math.Sqrt(discriminant)) / (2 * values["a"]));      
                }                
            }
            
            return result;
        }
    }

    public class NonIntValue : Exception
    {
        public NonIntValue(string message) : base(message) { }
    }

    public class NonDecision : Exception
    {
        public NonDecision(string msg) : base(msg) 
        {
            Program.FormData(msg, Program.Severity.Warning, Program.dict);
        }
    }
}
