using bwt___mft___rlf___ha;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace bwt___mtf___ha
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

    public class FileCompressionRatio
    {
        public static double GetCompressionRatio(string filePath1, string filePath2)
        {
            FileInfo fileInfo1 = new FileInfo(filePath1);
            FileInfo fileInfo2 = new FileInfo(filePath2);

            long size1 = fileInfo1.Length;
            long size2 = fileInfo2.Length;

            if (size1 == 0 || size2 == 0)
            {
                throw new ArgumentException("File size cannot be zero.");
            }

            double ratio = (double)size1 / size2;

            return ratio;
        }
    }
    internal class Program
    {

        public static int part = 50;
        static void Main(string[] args)
        {


            string graf = "enwik7.txt"; //"graph.txt"
            string graf_bwt = "bwt_graf.txt";
            string graf_res = "resgraph.txt";
            //bigBWT(graf, graf_bwt);
            BWTFast bwt = new BWTFast();
            string wholeText = "";
            string line = "";
            using (StreamWriter res = new StreamWriter(graf_bwt))
            {
                using (StreamReader reader = new StreamReader(graf, Encoding.UTF8))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        wholeText += line;

                    }
                }
                bwt.GetBWT(wholeText);
                res.Write(wholeText);
            }
            Console.WriteLine("+");

            List<char> newAlphabet = new List<char>();
            int index = 0;
            line = "";
            string result = "";

            while (index < wholeText.Length)
            {
                int k = Math.Min(100, wholeText.Length - index);
                line = wholeText.Substring(index, k);
                newAlphabet = alphabet(line);
                result += MTF(line, newAlphabet);
                index += k;

            }
            Console.WriteLine("+");
            result = rle(result);
            Console.WriteLine("+");
            using (FileStream fileStream = new FileStream(graf_res, FileMode.Create, FileAccess.Write))
            {
                index = 0;
                while (index < result.Length)
                {
                    int k = Math.Min(100, result.Length - index);
                    wholeText = result.Substring(index, k);
                    int[] p;
                    char[] symb;

                    textAlphabet(wholeText, out symb, out p);

                    string[] keys = new string[symb.Length];
                    char[] chars = new char[symb.Length];

                    HuffCodes(symb, p, out chars, out keys);
                    HcAlg(wholeText, keys, chars, fileStream);
                    index += k;
                }
            }

            double re = FileCompressionRatio.GetCompressionRatio(graf_res, graf);
            Console.WriteLine(re);
            Console.ReadKey();

        }
        static string rle(string gr)
        {
            string line = gr;
            string hlp = "";
            char currentChar = '0';
            int count = 1;
            int index = 0;
            string res = "";

            while (index < (line.Length - 2))
            {
                currentChar = line[index];
                if (currentChar == line[index + 1])
                {
                    count++;
                    index++;
                }
                else
                {

                    if (count == 1)
                    {
                        index++;
                        hlp += line[index - 1];
                        while (line[index] != line[index - 1] && index != line.Length - 1)
                        {
                            hlp += line[index];
                            index++;
                        }
                        hlp = hlp.Substring(0, hlp.Length - 1);
                        res += '&';
                        res +=hlp;
                        res +='&';
                        hlp = "";
                        index--;
                    }
                    else
                    {
                        res += (char)(count);
                        res += currentChar;
                        count = 1;
                        index++;
                    }
                }

                
            }
            res += (char)(count);
            res += currentChar;
            return res;
        }
        static void BWT(out string result, string inStr, ref int ind)
        {
            result = "";
            string[] bwt_matrix = new string[inStr.Length];
            string toAdd;
            string mainPart;
            string newStr = inStr;
            string part;
            int len = newStr.Length;
            for (int i = 0; i < len; i++)
            {
                toAdd = inStr.Substring(0, 1);
                mainPart = inStr.Substring(1, len - 1);
                inStr = mainPart + toAdd;
                bwt_matrix[i] = inStr;

            }
            string[] sorted_bwt = bwt_matrix.OrderBy(word => word).ToArray();
            for (int i = 0; i < len; i++)
            {
                if (sorted_bwt[i] == newStr)
                {
                    ind = i;
                }
            }
            for (int i = 0; i < len; i++)
            {
                part = sorted_bwt[i].Substring(len - 1);
                result += part;
            }

        }
        static List<char> alphabet(string s)
        {
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
                strEl = (char)(ind + 34);
                res += strEl.ToString() /*integerVal*/;
                removedChar = alphabet[ind];
                alphabet.RemoveAt(ind);
                alphabet.Insert(0, removedChar);
            }
            return res;
        }
        static void bigBWT(string inp, string outp)
        {
            string line;
            using (StreamReader reader = new StreamReader(inp, Encoding.UTF8))
            using (StreamWriter writer = new StreamWriter(outp))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    int len = line.Length;
                    int k = 0;

                    for (int i = len; i > 0; i -= part)
                    {
                        string textPart;
                        int partLen;

                        if (i - part < 0)
                        {
                            textPart = line.Substring(part * k);
                            partLen = line.Length - part * k;
                        }
                        else
                        {
                            textPart = line.Substring(part * k, part);
                            k++;
                            partLen = part;
                        }
                        string result;
                        int ind = 0;
                        BWT(out result, textPart, ref ind);
                        writer.Write(result);
                        writer.Write((char)(ind + part));

                    }
                    writer.Write("\n");
                }
            }
        }



        public static void HuffCodes(char[] s, int[] v, out char[] chars, out string[] keys)
        {
            chars = new char[s.Length];
            keys = new string[s.Length];

            var minHeap = new SortedSet<HeapNode>(Comparer<HeapNode>.Create((a, b) =>
            {
                int valComparison = a.Val.CompareTo(b.Val);
                if (valComparison != 0)
                {
                    return valComparison;
                }
                // если значения равны, сравниваем символы
                return a.Sym.CompareTo(b.Sym);
            }));

            int i = 0;
            for (i = 0; i < s.Length; i++)
            {
                minHeap.Add(new HeapNode(s[i], v[i]));
            }

            while (minHeap.Count > 1)
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
            if (chars.Length > 0)
            {
                CodesToArr(minHeap.First(), chars, keys, "", ref i);
            }

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


        public static void HcAlg(string main, string[] keys, char[] chars, Stream outputStream)
        {
            BitArray res = new BitArray(main.Length * 8);
            int bitIndex = 0;

            for (int i = 0; i < main.Length; i++)
            {
                char tmp = main[i];
                int index = Array.IndexOf(chars, tmp);
                string key = "";
                if (index != -1)
                {
                    key = keys[index];
                }

                foreach (char keyChar in key)
                {
                    if (keyChar == '1')
                    {
                        res[bitIndex] = true;
                    }
                    else if (keyChar == '0')
                    {
                        res[bitIndex] = false;
                    }
                    bitIndex++;
                }
            }

            if (bitIndex < res.Length)
            {
                BitArray trimmedRes = new BitArray(bitIndex);
                for (int i = 0; i < bitIndex; i++)
                {
                    trimmedRes[i] = res[i];
                }
                res = trimmedRes;
            }

            byte[] bytes = new byte[(res.Length - 1) / 8 + 1];
            res.CopyTo(bytes, 0);

            outputStream.Write(bytes, 0, bytes.Length);
        }

        public static void textAlphabet(string s, out char[] chars, out int[] pos)
        {
            var alphabet = s.Where(c => char.IsLetter(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c)).Distinct().OrderBy(c => c).ToArray();
            Array.Sort(alphabet);
            chars = alphabet;
            int[] freq = new int[chars.Length];
            pos = new int[chars.Length];
            int k = 0;
            foreach (char ch in s)
            {
                for (int i = 0; i < chars.Length; i++)
                {
                    if (ch == alphabet[i])
                    {
                        freq[i] += 1;
                        k++;
                        break;
                    }
                }
            }
            for (int i = 0; i < chars.Length; i++)
            {
                pos[i] += freq[i];
            }
        }
    }
}