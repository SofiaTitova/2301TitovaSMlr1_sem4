using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huffman
{
    public class HeapNode
    {
        public char Sym { get; set; }
        public int Val { get; set; }
        public HeapNode LeftChild { get; set; }
        public HeapNode RightChild { get; set; }

        public HeapNode(char sym, int val)
        {
            Sym = sym;
            Val = val;
        }
    }

    public class Program
    {
        public static void Main()
        {
            char[] symbols = { 'a', 'b', 'c', 'd', 'e' };
            int[] vals = { 10, 5, 2, 14, 15 };
            string[] keys = new string[symbols.Length];
            char[] chars = new char[symbols.Length];

            Console.WriteLine("Initial conditions");
            for (int j = 0; j < symbols.Length; j++)
            {
                Console.WriteLine(symbols[j] + ": " + vals[j]);
            }

            HuffCodes(symbols, vals, chars, keys);

            Console.WriteLine("Huffman codes");
            for (int i = 0; i < chars.Length; i++)
            {
                Console.WriteLine(chars[i] + ": " + keys[i]);
            }

            string main = "abcde";
            string res = HcAlg(main, keys, chars);
            Console.WriteLine("Direct conversion of: " + main + " is " + res);
            main = RevHcAlg(res, keys, chars);
            Console.WriteLine("Reverse conversion: " + main);
            Console.ReadKey();
        }

        public static void HuffCodes(char[] s, int[] v, char[] chars, string[] keys)
        {
            var minHeap = new SortedSet<HeapNode>(Comparer<HeapNode>.Create((a, b) => a.Val.CompareTo(b.Val)));
            int i = 0;
            for (i = 0; i < s.Length; i++)
            {
                minHeap.Add(new HeapNode(s[i], v[i]));
            }

            while (minHeap.Count != 1)
            {
                var leftChild = minHeap.First();
                minHeap.Remove(leftChild);

                var rightChild = minHeap.First();
                minHeap.Remove(rightChild);

                var tmp = new HeapNode('+', (leftChild.Val + rightChild.Val));
                tmp.LeftChild = leftChild;
                tmp.RightChild = rightChild;

                minHeap.Add(tmp);
            }

            i = 0;
            CodesToArr(minHeap.First(), chars, keys, "", ref i);
        }

        public static void CodesToArr(HeapNode root, char[] symb, string[] freq, string str, ref int i)
        {
            if (root == null)
            {
                return;
            }

            if (root.Sym != '+')
            {
                symb[i] = root.Sym;
                freq[i] = str;
                i++;
            }

            CodesToArr(root.LeftChild, symb, freq, str + "0", ref i);
            CodesToArr(root.RightChild, symb, freq, str + "1", ref i);
        }

        public static string HcAlg(string main, string[] keys, char[] chars)
        {
            string res = "";
            for (int i = 0; i < main.Length; i++)
            {
                char tmp = main[i];
                int index = Array.IndexOf(chars, tmp);
                res += keys[index];
            }

            return res;
        }

        public static string RevHcAlg(string res, string[] keys, char[] chars)
        {
            string main = "";
            string tmp = "";
            int i = 0;
            while (i < res.Length)
            {
                tmp += res[i];
                int ind = Array.IndexOf(keys, tmp);
                if (ind != -1)
                {
                    main += chars[ind];
                    tmp = "";
                }

                i++;
            }

            return main;
        }
    }
}

