using System.Text;

namespace metodiImmagini;

public class metodi
{
    public static StreamReader Read = new StreamReader("reference_le.pfm");
    string line = Read.ReadLine();

    public bool magic()
    {
        Console.WriteLine(this.line);
        return true;
        /*if (line == "PF")
        {
            return true;
        }
        return false;*/
    }
}

static IEnumerable<string> Split(string str, int chunkSize) {
    for (int i = 0; i < str.Length; i += chunkSize) 
        yield return str.Substring(i, chunkSize);
}
        
Split(binario,4)



