
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        string toImg = "BWimage.jpg"; // изображение
        string grImg = "greyImage.jpg";
        string outputFile = "output.txt";
        string outputFile2 = "output2.txt";
        string greyscale = "greyShades.txt";
        string colorImg = "colorImage.jpg";
        string colorscale = "colorShades.txt";
        
            Console.WriteLine("Выберите оформат изображения:");
            Console.WriteLine("1. Черно - белое");
            Console.WriteLine("2. Оттенки серого");
            Console.WriteLine("3. Цветное");
            int choice = 0;
            choice = Convert.ToInt32(Console.ReadLine());
        if (choice == 1) {
                using (Bitmap image = new Bitmap(toImg)) // использование изображения
                {
                    byte[,] colorGrid = new byte[image.Height, image.Width]; // массив для цвета пикселов

                    for (int i = 0; i < image.Height; i++)
                    {

                        for (int j = 0; j < image.Width; j++)
                        {

                            Color pixel = image.GetPixel(j, i);
                            byte shade = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                            colorGrid[i, j] = shade;
                        }
                    }



                    using (FileStream toOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                    {

                        for (int i = 0; i < image.Height; i++)
                        {

                            for (int j = 0; j < image.Width; j++)
                            {

                                toOutput.WriteByte(colorGrid[i, j]);
                            }
                        }
                    }


                    using (StreamWriter toOutput2 = new StreamWriter(outputFile2))
                    {

                        byte shade = (byte)(255 / 2);

                        for (int i = 0; i < image.Height; i++)
                        {

                            for (int j = 0; j < image.Width; j++)
                            {

                                if (colorGrid[i, j] > shade)
                                {

                                    toOutput2.Write("1");
                                }
                                else
                                {
                                    toOutput2.Write("0");
                                }
                            }

                            toOutput2.WriteLine();
                        }
                    }

                    Console.ReadLine();
                }
        }
        else if (choice == 2)
        {
                using (Bitmap grImage = new Bitmap(grImg)) // использование изображения
                {

                    int x, y;
                    //byte[,] greyGrid = new byte[image.Height, image.Width]; // массив для цвета пикселов
                    int[,] greyGrid = new int[grImage.Height, grImage.Width];
                    int red, green, blue;
                    for (x = 0; x < grImage.Height; x++)
                    {
                        for (y = 0; y < grImage.Width; y++)
                        {
                        //Color pixel = image.GetPixel(y, x);
                        //byte shade = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                        //int colorAsInt = (pixel.R << 16) | (pixel.G << 8) | pixel.B;
                            
                            ImagePixelColor.GetRGBComponents(grImage, y, x, out red, out green, out blue);

                            greyGrid[x, y] = (red + green + blue) / 3;
                        }
                    }
  
               
                int tmp;
                using (FileStream toOutput2 = new FileStream(greyscale, FileMode.Create))
                {
                    for (int i = 0; i < grImage.Height; i++)
                    {

                        for (int j = 0; j < grImage.Width; j++)
                        {
                            tmp = greyGrid[i, j];
                            
                            byte utByte = (byte)tmp;


                            toOutput2.WriteByte(utByte);


                        }
                    }
                    
                }
                Console.ReadLine();
                }
        }
        else if (choice == 3)
        {
            using (Bitmap colorImage = new Bitmap(colorImg)) // использование изображения
            {

                int x, y;
                int[,,] colorGrid = new int[3, colorImage.Height, colorImage.Width];
                int red, green, blue;
                for (int i = 0; i < 3; i++)
                {
                    for (x = 0; x < colorImage.Height; x++)
                    {
                        for (y = 0; y < colorImage.Width; y++)
                        {
                        
                            ImagePixelColor.GetRGBComponents(colorImage, y, x, out red, out green, out blue);
                            if (i == 0)
                            {
                                colorGrid[i, x, y] = red;
                            }
                            else if (i == 1)
                            {
                                colorGrid[i, x, y] = green;
                            }
                            else
                            {
                                colorGrid[i, x, y] = blue;
                            }
                        }
                    }
                }
                int tmp;
                using (FileStream toOutput2 = new FileStream(colorscale, FileMode.Create))
                {
                    for (int i = 0; i < colorImage.Height; i++)
                    {

                        for (int j = 0; j < colorImage.Width; j++)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                tmp = colorGrid[k, i, j];
                                byte utByte = (byte)tmp;
                                toOutput2.WriteByte(utByte);
                            }
                            
                        }

                       // toOutput2.WriteLine();
                    }
                }

                Console.ReadLine();
            }
        }
    }
   
}
public class ImagePixelColor
{
    public static void GetRGBComponents(Bitmap bitmap, int x, int y, out int red, out int green, out int blue)
    {
        if (bitmap == null)
        {
            throw new ArgumentNullException("bitmap");
        }

        if (x < 0 || x >= bitmap.Width || y < 0 || y >= bitmap.Height)
        {
            throw new ArgumentOutOfRangeException("The provided coordinates are out of the image bounds.");
        }

        Color pixelColor = bitmap.GetPixel(x, y);

        red = pixelColor.R;
        green = pixelColor.G;
        blue = pixelColor.B;
    }
}





//    using (StreamWriter toOutput2 = new StreamWriter(greyscale, false, Encoding.UTF8))
//    {
//    int h = 0;
//        for (int i = 0; i < grImage.Height; i++)
//        {   

//            for (int j = 0; j < grImage.Width; j++)
//            {
//                tmp = greyGrid[i, j];
//            if (tmp > 127)
//            {
//                L = tmp;
//                Console.WriteLine(L);
//            }
//                //ar tmpCH = ConvertCodeToChar(tmp, Encoding.UTF8);
//                char tmpCH = (char)tmp;
//                sbyte byt = (sbyte)tmpCH;
//                //if (tmpCH == '\n')
//                //{
//                //    toOutput2.Write('0');
//                //}

//                toOutput2.Write(tmpCH);
//                //toOutput2.Write(byt);
//                h++;
//            }

//           // toOutput2.WriteLine();
//        }
//    Console.WriteLine(h);
//    Console.WriteLine((char)L);
//}