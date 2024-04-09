using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace mtf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Начальная строка");
            string newStr = "bcabaaa";
            Console.WriteLine(newStr);
            List<char> newAlphabet = new List<char>();
            newAlphabet = alphabet(newStr);
            string result = "";
            string result2 = "";
            result = MTF(newStr, newAlphabet);
            Console.WriteLine("Преобразованная строка");
            Console.WriteLine(result);
            Console.ReadKey();
            result2 = reverse_MTF(result, newAlphabet);
            Console.WriteLine("Обратное преобразование");
            Console.WriteLine(result2);
            Console.ReadKey();
        }

        static List<char> alphabet(string s){
            List<char> chars = new List<char>();
            for (int i = 0; i < s.Length; i++)
            {
                if (!chars.Contains(s[i]))
                {
                    chars.Add(s[i]);
                }
            }
            chars.Sort((a, b) => a.CompareTo(b));
            return chars;
        }

        static string MTF(string s, List<char> alphabet)
        {
            string res = "";
            int ind;
            char symbol;
            string integerVal;
            char strEl;
            char removedChar;
            for (int i = 0; i < s.Length; i++)
            {
                symbol = s[i];
                ind = alphabet.IndexOf(symbol);
                integerVal = ind.ToString();
                strEl = (char)(ind+34);
                res += strEl.ToString() /*integerVal*/;
                removedChar = alphabet[ind];
                alphabet.RemoveAt(ind);
                alphabet.Insert(0, removedChar);
            }
            return res;
        }
        static string reverse_MTF(string s, List<char> alphabet)
        {
            string res = "";
            int ind;
            char removedChar;

            int[] s_inInt = new int[s.Length];
            s_inInt = toArr(s);

            for (int i = 0; i < s.Length; i++)
            {
                res += alphabet[s_inInt[i]];
                ind = s_inInt[i];
                removedChar = alphabet[ind];
                alphabet.RemoveAt(ind);
                alphabet.Insert(0, removedChar);
            }
            return res;
        }
        static int[] toArr (string s)
        {
            int[] num = new int[s.Length];
            int k = 0;
            for(int i = 0; i < s.Length; i++)
            {
                k = (int)s[i] - 34;
                num[i] = k;
            }
            return num;
        }
    }
}