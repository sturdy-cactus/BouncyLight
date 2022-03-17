using System.Diagnostics;
using System.Runtime.InteropServices;
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
                Console.WriteLine("Il file " + path + " è stato aperto");
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Il percorso non è corretto, il file non esiste oppure non è accessibile!");
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
                if (width * height < 0) throw new Exception("negative resolution of image");
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

            isBigEndian = (float.Parse(line) > 0);
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
            for (int j = this.h - 1; j >= 0; j--)
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

    //include contrbuti da stackoverflow, https://stackoverflow.com/a/43321133
    //CC-BY-SA-4.0 Alexei Shcherbakov, Olivier Jacot-Descombes
    [DllImport( "kernel32.dll", SetLastError = true )]
    public static extern bool SetConsoleMode( IntPtr hConsoleHandle, int mode );
    [DllImport( "kernel32.dll", SetLastError = true )]
    public static extern bool GetConsoleMode( IntPtr handle, out int mode );

    [DllImport( "kernel32.dll", SetLastError = true )]
    public static extern IntPtr GetStdHandle( int handle );
    public void PrintImg()
    {

        var handle = GetStdHandle( -11 );
        int mode;
        GetConsoleMode( handle, out mode );
        SetConsoleMode( handle, mode | 0x4 );
        
        Console.WriteLine("I pixel dell'immagine sono:");
        float max = 0;
        //questo primo ciclo calcola il massimo valore di RGB
        for (int i = 0; i < this.h; i++)
        {
            for (int j = 0; j < this.w; j++)
            {
                var pixel = GetPixel(i, j);
                var temp = pixel;
                max = temp.r;
                if (temp.g > max)
                    max = temp.g;
                if (temp.b > max)
                    max = temp.b;
            }
        }
        //adesso posso normalizzare i valori di RGB per stamparli a video
        for (int i = 0; i < this.h; i++)
        {
            for (int k = 0; k < 9; k++)

            {
                for (int j = 0; j < this.w; j++)

                {
                    var pixel = GetPixel(i, j);
                    var temp = pixel;

                    temp.r = (int) (temp.r * 255 / max);
                    temp.g = (int) (temp.g * 255 / max);
                    temp.b = (int) (temp.b * 255 / max);
                    if (k == 4)
                    {
                        Console.Write(
                            "\x1b[48;2;" + temp.r + ";" + temp.g + ";" + temp.b + "m ( {0,4} {1,4} {2,4} ) ",
                            pixel.r, pixel.g, pixel.b);
                    }
                    else
                        Console.Write(
                            "\x1b[48;2;" + temp.r + ";" + temp.g + ";" + temp.b + "m   {0,4} {1,4} {2,4}   ",
                            null, null, null);
                }
                
                Console.ResetColor();
                Console.Write("\n");
            }

            Console.ResetColor();
        }

    }

    public void SavePFM(Stream stream)
    {
        Console.WriteLine("Scrittura dello stream in corso...");
        float endianness = 1.0f;
        if (BitConverter.IsLittleEndian)
            endianness = -1.0f;

        var headString = "PF\n"+this.w.ToString()+" "+this.h.ToString()+"\n"+endianness.ToString()+"\n";
        byte[] head = new UTF8Encoding(true).GetBytes(headString);
        stream.Write(head, 0, head.Length);

        for (int i = this.h-1; i >=0; i--)
        {
            for (int j = 0; j < this.w; j++)
            {
                var myColor = GetPixel(i,j);
                
                byte[] red = BitConverter.GetBytes(myColor.r);
                stream.Write(red, 0, red.Length);
                
                byte[] green = BitConverter.GetBytes(myColor.g);
                stream.Write(green, 0, green.Length);
                
                byte[] blue = BitConverter.GetBytes(myColor.b);
                stream.Write(blue, 0, blue.Length);
            }
        }

        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("L'immagine è stata scritta correttamente nello stream!");
        Console.ResetColor();
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
