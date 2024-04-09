using bwt_rle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace burrows_wheeler_transform
{
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

            string graf = "text.txt"; /*"output2.txt"; //"graph.txt"*/
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
                       wholeText+= line;
                        
                    }
                }
                bwt.GetBWT(wholeText);
                res.Write(wholeText);
            }
           Console.WriteLine("+");

            rle(graf_bwt , graf_res);
            double re = FileCompressionRatio.GetCompressionRatio(graf_res, graf);
            Console.WriteLine(re);
            Console.ReadKey();

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
        static void rle(string gr, string grRes)
        {
            using (StreamReader reader = new StreamReader(gr, Encoding.UTF8)) //чтение построчно
            {
                using (StreamWriter res = new StreamWriter(grRes, false))
                {
                    string line;
                    string hlp = "";
                    char currentChar = '0';
                    while ((line = reader.ReadLine()) != null) // пптриааа
                    {
                        int count = 1;
                        int index = 0;

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
                                    res.Write('&');
                                    res.Write(hlp);
                                    res.Write('&');
                                    hlp = "";
                                    index--;
                                }
                                else
                                {
                                    res.Write((char)(count));
                                    res.Write(currentChar);
                                    count = 1;
                                    index++;
                                }
                            }

                        }

                        res.Write((char)(count));
                        res.Write(currentChar);
                        res.Write('\n');
                    }
                }
            }
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

    }
}
