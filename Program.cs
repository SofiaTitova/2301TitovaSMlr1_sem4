using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suffix1
{
    public class Program
    {
        public static void Main()
        {
            string text = "абракадабра";
            Console.WriteLine("Строка: " + text);
            int[] suffixArray = suffArr(text);
            Console.WriteLine("Cуффиксный массив");
            foreach (int i in suffixArray)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
            string newStr = "абракадабра";
            int[] suffArray = { 10, 7, 0, 5, 3, 8, 1, 6, 4, 9, 2 };

            string lastColumn = getLastCol(newStr, suffArray);
            Console.WriteLine("Последний столбец: " + lastColumn);
            //string str = "banana$";
            string str = "aaaaaaab$";
            Console.WriteLine("Для слова: {0}", str);
            char[] suf = new char[str.Length];
            suf = getSuf(str);
            Console.WriteLine("Суффиксный массив:");
            foreach (char c in suf)
            {
                Console.Write("{0} ",(char)c);
            }
            Console.ReadKey();
        }

        public static char[] getSuf(string s)
        {
            char[] res = new char[s.Length];
            res[s.Length - 1] = 'S';
            res[s.Length - 2] = 'L';
            for (int i = s.Length - 3; i >=0; i--)
            {
                if (s[i] < s[i + 1])
                {
                    res[i] = 'S';
                }
                else if (s[i] > s[i + 1])
                {
                    res[i] = 'L';
                }
                else
                {
                    res[i] = res[i + 1];
                }
            }
            return res;
        }
        public static string getLastCol(string s, int[] suffixArray)
        {
            int len = s.Length;
            int ind = 0;
            StringBuilder lastColumn = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                ind = suffixArray[i];
                if (ind == 0)
                {
                    lastColumn.Append(s[len - 1]);
                }
                else
                {
                    lastColumn.Append(s[ind - 1]);
                }
            }
            return lastColumn.ToString();
        }



        public static int[] suffArr(string s)
        {
            int len = s.Length;
            string[] suffixes = new string[len];
            for (int i = 0; i < len; i++)
            {
                suffixes[i] = s.Substring(i);
            }
            Array.Sort(suffixes);
            int[] suffixArray = new int[len];
            for (int i = 0; i < len; i++)
            {
                suffixArray[i] = len - suffixes[i].Length;
            }
            return suffixArray;
        }
    }
}


