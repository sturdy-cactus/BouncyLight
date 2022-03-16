using System.Diagnostics;
using System.Reflection.PortableExecutable;
using mialibreria;

internal static partial class Program
{
    private static void Main()
    {
        Color a = new Color(1, 2, 3);
        a.Add(2, 4, 5);
        Console.WriteLine("le componenti della somma sono {0} {1} {2}", a.r, a.g, a.b);
        a.Cmult(a);
        Console.WriteLine("le componenti del prodotto sono {0} {1} {2}", a.r, a.g, a.b);

        check.AssertColor();
        
        HdrImage img = new HdrImage("zio.pdb");

    }

}