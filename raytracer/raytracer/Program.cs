using System.Diagnostics;
using mialibreria;

internal static partial class Program
{
    private static void Main()
    {
        bool IsClose(float a, float b)
        {
            const float epsilon = 1e-4f; //fallisce a e-5!
            return Math.Abs(a - b) < epsilon;
        }

        void AssertColor()
        {
            Color c = new Color(1.0f, 2.1f, 3f);
            Color b = new Color(256.3f, 90.1f, 3.56f);
            Color d = c;
            c.AddColor(b);
            Console.WriteLine("{0} {1} {2}", c.r, c.g, c.b);
            Debug.Assert(IsClose(c.r, 257.3f));
            d.Cmult(b);
            Console.WriteLine("{0} {1}", d.g, 189.21f);
            Debug.Assert(IsClose(d.g, 189.21f));
            Console.WriteLine("il test ha avuto successo!");
        }

        Color a = new Color(1, 2, 3);
        a.Add(2, 4, 5);
        Console.WriteLine("le componenti della somma sono {0} {1} {2}", a.r, a.g, a.b);
        a.Cmult(a);
        Console.WriteLine("le componenti del prodotto sono {0} {1} {2}", a.r, a.g, a.b);

        AssertColor();

    }
}