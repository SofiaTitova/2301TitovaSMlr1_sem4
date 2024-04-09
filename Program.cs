using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace lz77_huff
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
    public class Program
    {
        public static void Main()
        {

            /* string text = "text.txt";
             string line;
             string wholeText = "";
             using (StreamReader reader = new StreamReader(text, Encoding.UTF8))
             {
                 while ((line = reader.ReadLine()) != null)
                 {
                     wholeText += line + " ";
                 }
             }

             int [] p;
             char [] symb;

             textAlphabet(wholeText, out symb, out p);

             string[] keys = new string [symb.Length];
             char[] chars = new char[symb.Length];

             HuffCodes(symb, p, out chars, out keys);


             string res = HcAlg(wholeText, keys, chars);
             string reverse_text = "reverse_text.txt";
             List<object> list_Res = lz77(res, 10);
             res = listToStr(list_Res);
             using (StreamWriter writer = new StreamWriter(reverse_text))
             {

                 writer.Write(res);

             }


             Console.WriteLine("Conversion in file");
             Console.ReadKey();*/
            string text = "rusTolstoy.txt";///"enwik7.txt"
            string line;
            string wholeText = "";
            string reverse_text = "reverse_text.txt";
            using (StreamReader reader = new StreamReader(text, Encoding.UTF8))
            {
                using (FileStream fileStream = new FileStream(reverse_text, FileMode.Create, FileAccess.Write))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        wholeText = line;//+ 'ؐ'
                        int[] p;
                        char[] symb;
                        //List<object> list_Res = lz77(wholeText, 5);
                        //wholeText = listToStr(list_Res);
                        textAlphabet(wholeText, out symb, out p);

                        string[] keys = new string[symb.Length];
                        char[] chars = new char[symb.Length];

                        HuffCodes(symb, p, out chars, out keys);     
                        HcAlg(wholeText, keys, chars, fileStream);
                    }
                }
            }

            double re = FileCompressionRatio.GetCompressionRatio(reverse_text, text);

            Console.WriteLine(re);
            Console.WriteLine("Conversion in file");
            Console.ReadKey();

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

                if (key != null)
                {
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
            //var alphabet = s.Where(c => char.IsLetter(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c)).Distinct().OrderBy(c => c).ToArray();
            var alphabet = s.Distinct()
                .OrderBy(c => (int)c) // Сортировка по номеру символа в Unicode
                .ToArray();
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
        
        public struct Node
        {
            public int offset; // отступ влево
            public int length; // длина подстроки
            public char next; // следующий символ
            public Node(int offset, int length, char next)
            {
                this.offset = offset;
                this.length = length;
                this.next = next;
            }
            public void PrintNodeData() // для вывода
            {
                Console.WriteLine($"Offset: {offset}, Length: {length}, Next: {next}");
            }
        }

   
        static List<object> lz77(string data, int buff_size)
        {
            List<object> triplet_list = new List<object>(); //список в котором и одиночные символы и ноды
            string buf = ""; //буфер 
            int pos = 0;
            int offset = 0;
            int lengthNode = 0;
            char nextChar = '\0';
            while (pos < data.Length)//смотрим все элементы строки 
            {
                int max = Math.Max(pos - buff_size, 0);
                buf = data.Substring(max, pos - max); //вычисляем буфер 
                int index_substr = buf.Length;
                int length = 0;
                for (int l = 1; l < buf.Length + 1; l++)
                {
                    string k = data.Substring(pos, l); //отделяем от строки 
                    if ((buf.LastIndexOf(data.Substring(pos, l)) == -1)) //ищем эту строку в буфере
                    {
                        break;
                    }
                    else
                    {
                        index_substr = buf.LastIndexOf(data.Substring(pos, l)); //если нашли то запоминаем и пытаемся найти строку на элемент больше
                        length = l;
                    }
                }
                offset = buf.Length - index_substr; //сохраняем сдвиг
                lengthNode = length; //длина подстроки
                int count = 0;
                int pos_new = pos;
                int lengthNode_new = length;
                if (length != 0)
                {
                    while (data[pos_new] == data[pos_new + length]) //проверяем повторки, если они есть записываем как можно длиннее
                    {
                        pos_new++;
                        lengthNode_new++;
                        count++;
                    }
                    if (count > 1) // если повторка 1 то это не повтор
                    {
                        pos = pos_new;
                        lengthNode = lengthNode_new;
                    }
                }
                nextChar = data[pos + length]; // записывает следующий символ

                pos += length;
                pos++;
                zeroDel(offset, lengthNode, nextChar, triplet_list); //удаляем 0, если новая буква

            }
            return triplet_list; // возвращаем список нодд и букв
        }
        
        static public bool IsChar(object obj) // проверяем нода или буква
        {
            return obj is char;
        }
        static public void zeroDel(int offset, int lengthNode, char nextChar, List<object> triplet_list) // удаляем нули чтобы их не хранить
        {
            if (offset == 0 && lengthNode == 0)
            {
                triplet_list.Add(nextChar);
            }
            else
            {
                Node triplet = new Node(offset, lengthNode, nextChar);
                triplet_list.Add(triplet);
            }
        }
        
        static public string listToStr(List<object> res_list)
        {
            string res = "";
            foreach (object list in res_list) 
            {
                if (IsChar(list))
                {
                    res += list.ToString();
                }
                else
                {
                    Node node = (Node)list;
                    res += node.offset;
                    res += node.length;
                    res += node.next;
                }
            }
            return res;
        }
    }
}

