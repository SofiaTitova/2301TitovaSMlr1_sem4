using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace huffman_canonical_code
{
    /*Можно сделать по-другому, например, массив строк перевести в int, работать с интом, прибавляя единицы, в потом с помощью встроенной функции вернуть в бинарный вид
     int decimalNumber = 42;
    string binaryNumber = ConvertToBinary(decimalNumber);
    Console.WriteLine(binaryNumber); // Выведет: "101010"  
    например такая функция
    */
    internal class Program
    {
        static void Main(string[] args)
        {
            //string[] huffCode = { "0", "11", "101", "100"};
            //string[] huffCode = { "0", "100", "101", "111", "1100", "1101" };
            string[] huffCode = { "00", "01", "11", "100", "1010", "1011" };
            int[] huffLen = { 2, 2, 2, 3, 4, 4};
            Console.WriteLine("1. По кодам");
            Console.WriteLine("2. По длинам");
            int choice = 0;
            choice = Convert.ToInt32(Console.ReadLine()); 
            if (choice == 1)
            {
                Console.WriteLine("Код Хаффмана");
                for (int i = 0; i < huffCode.Length; i++)
                {
                    Console.WriteLine(huffCode[i]);
                }
                string[] res = new string[huffCode.Length];
                res = huff_canon(huffCode);
                Console.WriteLine("Канонический код Хаффмана");
                for (int i = 0; i < res.Length; i++)
                {
                    Console.WriteLine(res[i]);
                }
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Длины кодов Хаффмана");
                for (int i = 0; i < huffLen.Length; i++)
                {
                    Console.WriteLine(huffLen[i]);
                }
                string[] res = new string[huffCode.Length];
                res = lenToCode(huffLen);
                Console.WriteLine("Канонический код Хаффмана");
                for (int i = 0; i < res.Length; i++)
                {
                    Console.WriteLine(res[i]);
                }
                Console.ReadKey();
            }

        }
        static string[] lenToCode(int[] huffLen)
        {
            string[] code = new string[huffLen.Length];
            string curr = "";
            string tmp = "";
            int oneVal = 0;
            int zeroInd = 0;
            for (int i = 0; i < huffLen.Length; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < huffLen[0]; j++)
                    {
                        curr += "0";
                    }
                    tmp = curr;
                    code[i] = curr;
                }
                else
                {
                    if (tmp[tmp.Length - 1] == '0')
                    {
                        tmp = tmp.Substring(0, tmp.Length - 1);
                        tmp += "1";
                    }
                    else
                    {
                        for (int j = 0; j < tmp.Length; j++)
                        {
                            if (tmp[j] == '1')
                            {
                                oneVal++;
                            }
                            else
                            {
                                zeroInd = j;
                            }
                        }
                        if (oneVal == tmp.Length)
                        {
                            tmp = "1";
                            for (int k = 0; k < code.Length; k++)
                            {
                                tmp += "0";
                            }
                        }
                        else
                        {
                            tmp = tmp.Substring(0, zeroInd);
                            tmp += "1";
                            int len = tmp.Length - zeroInd;
                            for (int k = 0; k < len; k++)
                            {
                                tmp += "0";
                            }
                        }
                    }
                    if (huffLen[i] > tmp.Length)
                    {
                        int k = huffLen[i] - tmp.Length;
                        for (int p = 0; p < k; p++)
                        {
                            tmp += "0";
                        }
                    }
                    code[i] = tmp;
                }
            }

            return code;
        }
        static string[] huff_canon(string[] huffCode)
        {
            string[] code = new string[huffCode.Length];
            string tmp = "";
            string curr = "";
            int oneVal = 0;
            int zeroInd = 0;
            for (int i = 0; i < huffCode.Length; i++)
            {
                if (i == 0) { 
                    for (int j = 0; j < huffCode[0].Length; j++) {
                        curr += "0";
                    }
                    tmp = curr;
                    code[i] = curr;
                }
                else
                {
                    if (tmp[tmp.Length - 1] == '0')
                    {
                        tmp = tmp.Substring(0, tmp.Length - 1);
                        tmp += "1";
                    }
                    else
                    {
                        for (int j = 0; j < tmp.Length; j++)
                        {
                            if (tmp[j] == '1')
                            {
                                oneVal++;
                            }
                            else
                            {
                                zeroInd = j;
                            }
                        }
                        if (oneVal == tmp.Length)
                        {
                            tmp = "1";
                            for (int k = 0; k < code.Length; k++)
                            {
                                tmp += "0";
                            }
                        }
                        else
                        {
                            tmp = tmp.Substring(0, zeroInd);
                            tmp += "1";
                            int len = tmp.Length - zeroInd;
                            for (int k = 0; k < len; k++)
                            {
                                tmp += "0";
                            }
                        }
                    }
                    if (huffCode[i].Length > tmp.Length)
                    {
                        int k = huffCode[i].Length - tmp.Length;
                        for (int p = 0; p< k; p++)
                        {
                            tmp += "0";
                        }
                    }
                    code[i] = tmp;
                }
                

            }
            return code;

        }
    }
}
