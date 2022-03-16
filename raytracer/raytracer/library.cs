using System.Diagnostics;
using System.Text;

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

    public HdrImage(string? path)
    {
        while (true)
        {
            StreamReader Read = null;
            try
            {
                Read = new StreamReader(path);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(
                    "il percorso non è corretto oppure il file non esiste:\n Inserire il percorso corretto:");
                path = Console.ReadLine();
                continue;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(
                    "il percorso non è corretto oppure il file non esiste:\n Inserire il percorso corretto:");
                path=Console.ReadLine();
                continue;
            }

            string line = Read.ReadLine();
            if (line != "PF")
            {
                Console.WriteLine("non hai fornito un file PFM!\nInserire il percorso del file corretto:");
                path = Console.ReadLine();
                continue;
            }

            line = Read.ReadLine();
            var res = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                this.w = Int32.Parse(res[0]);
                this.h = Int32.Parse(res[1]);
                Console.WriteLine("La risoluzione è {0} {1}",w, h);
            }
            catch (Exception e)
            {
                Console.WriteLine("C'è stato un errore durante la lettura della risoluzione.\nInserire il percorso del file corretto:");
                path = Console.ReadLine();
                continue;
            }
            
            break;
        }
    }
    
    public Color GetPixel(int a, int b)
    {
        int pos = b * this.w + a;
        return this.pixels[pos];
    }
    
    public void SetPixel(int a, int b, Color c)
    {
        int pos = b * this.w + a;
        this.pixels[pos] = c;
    }

}
