using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace exchange_BWT
{
    struct substitut
    {
        public char val1;
        public int val2;
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            string fuul = "рдакраааабб";
            Console.WriteLine(ExRevBWT(2, fuul));
            Console.ReadKey();
        }
        static string ExRevBWT(int ind, string s)
        {
            substitut[] data = new substitut[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                data[i] = new substitut { val1 = s[i], val2 = i };
            }
            Array.Sort(data, (a, b) => a.val1.CompareTo(b.val1));
            string res = "";
            for (int i = 0; i < s.Length; i++)
            {
                ind = data[ind].val2;
                res += s[ind];
            }
            return res;
        }
    }
}

