using System.Diagnostics;
using System.Reflection.PortableExecutable;
using mialibreria;

internal static partial class Program
{
    private static void Main()
    {
        Inizio:
        Color a = new Color(1, 2, 3);
        a.Add(2, 4, 5);
        Console.WriteLine("le componenti della somma sono {0} {1} {2}", a.r, a.g, a.b);
        a.Cmult(a);
        Console.WriteLine("le componenti del prodotto sono {0} {1} {2}", a.r, a.g, a.b);

        check.AssertColor();
        Console.WriteLine("Inserire il percorso del file PFM da aprire:");
        
        HdrImage img = new HdrImage(Console.ReadLine());

        img.PrintImg();
        
        Copia:
        Console.WriteLine("Come vuoi chiamare il file copia?");
        var path = Console.ReadLine();
        if (File.Exists(path))
        {        
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Il file esiste già. Sovrascriverlo? (s/n)");
            Console.ResetColor();
            if (Console.ReadLine() != "s")
                goto Copia;
            File.Delete(path);
        }

        using (FileStream stream = File.Create(path))
        {
            img.SavePFM(stream);
            stream.Close();
            stream.Dispose();
        }

        HdrImage img2 = new HdrImage(path);
        img2.PrintImg();

        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("Scrivi qualunque cosa per riavviare il programma, oppure premi invio per chiudere il programma.");
        Console.ResetColor();
        if (Console.ReadLine() != "")
            goto Inizio;
    }

}