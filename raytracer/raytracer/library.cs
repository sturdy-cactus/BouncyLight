﻿using System.Diagnostics;
using System.Text;
using extra;

namespace mialibreria;

public struct check
{
    public static bool IsClose(float a, float b)
    {
        const float epsilon = 1e-4f; //fallisce a e-5!
        return Math.Abs(a - b) < epsilon;
    }
    public static void AssertColor()
    {
        Color c = new Color(1.0f, 2.1f, 3f);
        Color b = new Color(256.3f, 90.1f, 3.56f);
        Color d = c;
        c.Add(b);
        Console.WriteLine("\ninizio assert...\n{0} {1} {2}", c.r, c.g, c.b);
        Debug.Assert(check.IsClose(c.r, 257.3f));
        d.Cmult(b);
        Console.WriteLine("{0} {1}", d.g, 189.21f);
        Debug.Assert(check.IsClose(d.g, 189.21f));
        Console.WriteLine("il test ha avuto successo!");
    }

    public static void AssertHdrImage()
    {
       return; 
    }
}
public struct Color
{
    public float r = 0, g = 0, b = 0;
    
    public Color(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }
    
    public Color Add(float a, float b, float c)
    {
        this.r += a;
        this.g += b;
        this.b += c;
        return this;
    }
    public Color Add(Color a)
    {
        this.r += a.r;
        this.g += a.g;
        this.b += a.b;
        return this;
    }

    public Color Smult(float a)
    {
        this.r *= a;
        this.g *= a;
        this.b *= a;
        return this;
    }
    
    public Color Cmult(Color a)
    {
        this.r *= a.r;
        this.g *= a.g;
        this.b *= a.b;
        return this;
    }

}

public class HdrImage
{
    public int w = 0, h = 0;
    public Color[] pixels;

    public HdrImage(int w, int h)
    {
        this.w = w;
        this.h = h;
        this.pixels = new Color[this.w * this.h];
    }

    public HdrImage(string path)
    {
        while (true)
        {
            Inizio:
            FileStream stream = null;
            try
            {
                stream = File.Open(path, FileMode.Open);
                Console.WriteLine("Il file è stato aperto");
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Il percorso non è corretto oppure il file non esiste!");
                Console.ResetColor();
                Console.WriteLine("Inserire il percorso corretto:");
                path = Console.ReadLine();
                continue;
            }

            BinaryReader Read = new BinaryReader(stream);
            string line = Read.ReadLine();
            if (line != "PF")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Non hai fornito un file PFM!");
                Console.ResetColor();
                Console.WriteLine("Inserire il percorso del file corretto:");
                path = Console.ReadLine();
                continue;
            }

            line = Read.ReadLine();
            var res = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            int width;
            int height;
            try
            {
                width = int.Parse(res[0]);
                height = int.Parse(res[1]);
                if (width*height < 0) throw new Exception("negative resolution of image");
                Console.WriteLine("La risoluzione è {0}x{1}", width, height);
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Errore nella lettura della risoluzione!");
                Console.ResetColor();
                Console.WriteLine("Inserire il percorso del file corretto:");
                path = Console.ReadLine();
                continue;
            }

            this.w = int.Parse(res[0]);
            this.h = int.Parse(res[1]);
            this.pixels = new Color[this.w * this.h];

            line = Read.ReadLine();
            bool isBigEndian;
            try
            {
                var temp = float.Parse(line);
                if (temp == 0)
                {
                    throw new Exception("0 endianness value");
                }
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Errore nella lettura della endianness!");
                Console.ResetColor();
                Console.WriteLine("Inserire il percorso del file corretto:");
                path = Console.ReadLine();
                continue;
            }
            isBigEndian = (float.Parse(line)>0);
            if (isBigEndian)
            {
                Console.WriteLine("Il file ha una codifica big endian");
            }
            else
            {
                Console.WriteLine("Il file ha una codifica little endian");
            }

            byte[] pixelArray = new byte[4];
            
            var i = 1;
            //lettura da sinistra a destra, dal basso in alto:
            for (int j = this.h-1; j >= 0; j--)
            {
                for (int k = 0; k < this.w; k++)
                {
                    try
                    {
                        pixelArray = Read.ReadBytes(4);
                        if (BitConverter.IsLittleEndian == isBigEndian)
                            Array.Reverse(
                                pixelArray); //se la codifica è big e la lettura little o viceversa, inverti solo se ti aspetti little endian.
                        float r = BitConverter.ToSingle(pixelArray, 0);

                        pixelArray = Read.ReadBytes(4);
                        if (BitConverter.IsLittleEndian == isBigEndian)
                            Array.Reverse(
                                pixelArray); //se la codifica è big e la lettura little o viceversa, inverti solo se ti aspetti little endian.
                        float g = BitConverter.ToSingle(pixelArray, 0);

                        pixelArray = Read.ReadBytes(4);
                        if (BitConverter.IsLittleEndian == isBigEndian)
                            Array.Reverse(
                                pixelArray); //se la codifica è big e la lettura little o viceversa, inverti solo se ti aspetti little endian.
                        float b = BitConverter.ToSingle(pixelArray, 0);

                        Color pixel = new Color(r, g, b);

                        SetPixel(k, j, pixel);
                    }
                    catch (ArgumentException e)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("I pixel presenti non corrispondono alla risoluzione!");
                        Console.ResetColor();
                        Console.WriteLine("Inserire il percorso del file corretto:");
                        path = Console.ReadLine();
                        goto Inizio;
                    }
                }
            }
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("L'immagine è stata letta correttamente!");
            Console.ResetColor();

            return;
        }
    }

    public void PrintImg()
    {
        Console.WriteLine("I pixel dell'immagine sono:");
        for (int i = 0; i < this.h; i++)
        {
            for (int j = 0; j < this.w; j++)
            {
                var pixel = GetPixel(i, j);
                Console.Write("(" + pixel.r.ToString() + ", " + pixel.g.ToString() + ", " + pixel.b.ToString() + ") ");

            }

            Console.Write("\n");
        }
    }

    public Color GetPixel(int row, int column)
    {
        int pos = row * this.w + column;
        return this.pixels[pos];
    }

    public void SetPixel(int row, int column, Color c)
    {
        int pos = column * this.w + row;
        this.pixels[pos] = c;
    }

}
