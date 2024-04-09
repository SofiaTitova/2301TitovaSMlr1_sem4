
using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Runtime.ConstrainedExecution;
using System.Text;

class Program
{
    static void Main()
    {
        string image = "output2.txt"; // файл с изображением 
        string result = "convertRes.txt"; //сжатие чб
        string toImage = "output_new.txt"; //разжатие чб
        string greyImage = "greyShades.txt"; // файл в оттенках   "greyShades_check.txt"
        string toGreyImage = "greyResult.txt"; //сжатие серого
        string greyRes = "greyConvertResult.txt"; //разжатие серого
        string colorImage = "colorShades.txt"; //"colorShades_check.txt";
        string toColorImage = "colorResult.txt"; //сжатие цветного
        string colorRes = "colorConvertResult.txt"; //разжатие цветного

        Console.WriteLine("Выберите оформат изображения:");
        Console.WriteLine("1. Черно - белое");
        Console.WriteLine("2. Оттенки серого");
        Console.WriteLine("3. Цветное");
        int choice = 0;
        choice = Convert.ToInt32(Console.ReadLine());

        if (choice == 1)
        {
            if (!File.Exists(image)) // проверка существования
            {
                Console.WriteLine("Файл не найден.");
                return;
            }

            using (StreamReader reader = new StreamReader(image)) //чтение построчно
            {
                using (StreamWriter res = new StreamWriter(result)) //запись в файл
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        char prevChar = line[0];
                        int count = 1;
                        if (prevChar == '1') // всегжа начинается с 0
                        {
                            count = 0;
                            prevChar = '0';
                            res.Write((char)(count));

                            prevChar = line[0];
                            count = 1;
                        }

                        for (int i = 1; i < line.Length; i++) //записываем кол-во подряд идущих
                        {
                            if (line[i] == prevChar)
                            {
                                count++;
                            }
                            else
                            {
                                res.Write((char)(count));

                                prevChar = line[i];
                                count = 1;
                            }
                        }
                        res.Write((char)(count));
                        res.Write('\n');
                    }
                }
            }

            using (StreamReader reader = new StreamReader(result)) //чтение построчно
            {
                using (StreamWriter toImg = new StreamWriter(toImage)) //запись разжатия
                {
                    string line;
                    char charecter = '0';
                    while ((line = reader.ReadLine()) != null) //считываем построчно
                    {
                        if (line[0] == '0') // так как всегда в начале ноль
                        {
                            charecter = '1';
                        }
                        else
                        {
                            charecter = '0';
                        }

                        for (int i = 0; i < line.Length; i++) // выписываем 0 и 1
                        {
                            int count = (int)(line[i]);

                            for (int j = 0; j < count; j++)
                            {
                                toImg.Write(charecter);
                            }

                            if (charecter == '0') // чтобы чередовать
                            {
                                charecter = '1';
                            }
                            else
                            {
                                charecter = '0';
                            }
                        }
                        toImg.Write('\n');
                    }
                }
            }
            checker(result, image);
        }
        else if (choice == 2)
        {
            if (!File.Exists(greyImage)) // проверка существования
            {
                Console.WriteLine("Файл не найден.");
                return;
            }



            using (StreamReader reader = new StreamReader(greyImage, Encoding.UTF8)) //чтение построчно
            {
                using (StreamWriter res = new StreamWriter(greyRes))
                {
                    int cur;
                    int k = 0;
                    char prevChar = '\0';
                    int count = 1;
                    while ((cur = reader.Read()) != -1)
                    {
                        k++;
                        char ch = (char)cur;


                        if (ch == prevChar)
                        {
                            count++;
                        }
                        else
                        {
                            if (prevChar != '\0')
                            {
                                res.Write((char)(count));
                                res.Write(prevChar);
                            }
                            prevChar = ch;
                            count = 1;
                        }

                    }
                    res.Write((char)(count));
                    res.Write(prevChar);
                }
            }

            using (StreamReader reader = new StreamReader(greyRes)) //чтение построчно
            {
                using (StreamWriter togrImg = new StreamWriter(toGreyImage))
                {
                    int cur;
                    int k = 0;

                    int count = 1;
                    char ch = '\0';
                    while ((cur = reader.Read()) != -1)
                    {
                        if (k > 0)
                        {
                            ch = (char)cur;
                        }
                        else
                        {
                            count = cur;
                        }
                        if (k > 0)
                        {
                            for (int j = 0; j < count; j++)
                            {
                                togrImg.Write(ch);

                            }
                            k = 0;
                        }
                        else k++;
                    }
                }
            }
            checker(greyRes, greyImage);
        }

        else if (choice == 3)
        {
            if (!File.Exists(colorImage)) // проверка существования
            {
                Console.WriteLine("Файл не найден.");
                return;
            }

            using (StreamReader reader = new StreamReader(colorImage, Encoding.UTF8)) //чтение построчно
            {
                using (StreamWriter res = new StreamWriter(colorRes))
                {
                    int cur;
                    int k = 0;
                    char prevChar = '\0';
                    int count = 1;
                    char ch1;
                    char ch2;
                    char ch3;
                    string line = "";
                    while ((cur = reader.Read()) != -1)
                    {
                        ch1 = (char)cur;
                        line += ch1;
                    }
                    int i = 0;
                    int i3 = 3;

                    while (i < line.Length - 6)
                    {
                        if ((line[i] == line[i + i3]) && (line[i + 1] == line[i + i3 + 1]) && (line[i + 2] == line[i + i3 + 2]))
                        {
                            count++;
                            i += 3;
                        }
                        else
                        {
                            res.Write((char)(count));
                            res.Write(line[i]);
                            res.Write(line[i + 1]);
                            res.Write(line[i + 2]);
                            count = 1;
                            i += 3;
                        }
                    }
                    res.Write((char)(count));
                    res.Write(line[i]);
                    res.Write(line[i + 1]);
                    res.Write(line[i + 2]);
                }
            }

            using (StreamReader reader = new StreamReader(colorRes)) //чтение построчно
            {
                using (StreamWriter toImg = new StreamWriter(toColorImage))
                {
                    int cur;
                    int k = 0;
                    char ch1;
                    string line = "";
                    while ((cur = reader.Read()) != -1)
                    {
                        ch1 = (char)cur;
                        line += ch1;
                    }
                    int count = 1;
                    int i = 0;
                    while (i < line.Length - 3)
                    {
                        k = (int)line[i];
                        for (int j = 0; j < k; j++)
                        {
                            toImg.Write(line[i + 1]);
                            toImg.Write(line[i + 2]);
                            toImg.Write(line[i + 3]);
                        }
                        i += 4;
                    }
                }
            }
            checker(colorRes, colorImage);
        }
    }


    static void checker(string compr, string decompr)
    {

        double fileSize1 = new FileInfo(compr).Length;
        double fileSize2 = new FileInfo(decompr).Length;

        if (fileSize1 > fileSize2)
        {
            Console.WriteLine("Размер СЖАТОГО файла: {0}\nРазмер ИСХОДНОГО файла: {1}", fileSize1, fileSize2);
            Console.WriteLine("Мощность сжатия: {0}", fileSize2 / fileSize1);

        }
        else if (fileSize1 < fileSize2)
        {
            Console.WriteLine("Размер СЖАТОГО файла: {0}\nРазмер ИСХОДНОГО файла: {1}", fileSize1, fileSize2);
            Console.WriteLine("Мощность сжатия: {0}", fileSize2 / fileSize1);
        }
        else
        {
            Console.WriteLine("Размер СЖАТОГО файла: {0}\nРазмер ИСХОДНОГО файла: {1}", fileSize1, fileSize2);
            Console.WriteLine("Мощность сжатия: {0}", fileSize2 / fileSize1);
        }

        Console.ReadLine();
    }
}