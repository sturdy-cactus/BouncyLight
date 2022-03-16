﻿using System.Diagnostics;
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
        Console.WriteLine("Inserire il percorso del file PFM da aprire:");
        
        HdrImage img = new HdrImage(Console.ReadLine());

        img.PrintImg();
        
        var path = "prova.pfm";
        if (File.Exists(path)) File.Delete(path);

        FileStream stream = File.Create(path);
        img.SavePFM(stream);
        stream.Close();

        HdrImage img2 = new HdrImage(path);
        img2.PrintImg();

    }

}