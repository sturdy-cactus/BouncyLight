public struct Vec
{
    public double x, y, z;

    public Vec(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vec Add(double a, double b, double c)
    {
        this.x += a;
        this.y += b;
        this.z += c;
        return this;
    }
}

class Program
{
    static void Main()
    {
        string[] lines = System.IO.File.ReadAllLines(
            @"C:\Users\giuli\Documents\magistrale\raytracing\advent of code\1\input"
        );

        {
            int counter = 0;
            var i = 0;
            foreach (string line in lines)
            {
                int k = Int32.Parse(line);
                if (k > i)
                {
                    counter++;
                }

                i = k;
            }

            Console.WriteLine("il numero di aumenti di profondità è {0}", counter - 1);

        }

//2:sliding window
        {
            var sum = 0;
            var counter = 0;
            for (var k = 0; k < lines.Length - 2; k++)
            {
                var sumTemp = 0;
                for (var j = 0; j < 3; j++)
                {
                    sumTemp += int.Parse(lines[k + j]);
                }

                if (sumTemp > sum)
                {
                    counter++;
                }

                sum = sumTemp;
            }

            Console.WriteLine("il numero di aumenti di profondità a somma di 3 è {0}", counter - 1);

        }



//2.1 movimento sottomarino
        {
            string[] directions =
                System.IO.File.ReadAllLines(
                    @"C:\Users\giuli\Documents\magistrale\raytracing\advent of code\1\directions");


            Vec pos = new Vec(0, 0, 0);
            foreach (string line in directions)
            {
                string[] parts = line.Split(' ');
                var q = int.Parse(parts[1]);
                if (parts[0] == "forward")
                {
                    pos.Add(q, 0, 0);
                }
                else if (parts[0] == "up")
                {
                    pos.Add(0, 0, q);
                }
                else if (parts[0] == "down")
                {
                    pos.Add(0, 0, -1 * q);
                }
            }

            Console.WriteLine("la posizione finale è {0} {1} {2}. Il prodotto è {3}", pos.x, pos.y, pos.z,
                pos.x * pos.z);
        }

        {//2.2 scoperta di aim
            string[] directions =
                System.IO.File.ReadAllLines(
                    @"C:\Users\giuli\Documents\magistrale\raytracing\advent of code\1\directions");


            Vec pos = new Vec(0, 0, 0);
            foreach (string line in directions)
            {
                string[] parts = line.Split(' ');
                var q = int.Parse(parts[1]);
                if (parts[0] == "forward")
                {
                    pos.Add(q, 0, pos.y*q);
                }
                else if (parts[0] == "up")
                {
                    pos.Add(0, q, 0);
                }
                else if (parts[0] == "down")
                {
                    pos.Add(0, -1*q, 0);
                }
            }

            Console.WriteLine("la posizione finale è {0} {2}, con aim {1}. Il prodotto è {3}", pos.x, pos.y, pos.z,
                pos.x * pos.z);            
        }
        {
            //3.1 power consumption
            string[] consumption =
                System.IO.File.ReadAllLines(
                    @"C:\Users\giuli\Documents\magistrale\raytracing\advent of code\1\binaryrate");
            int[] counter0 = new int[12];
            int[] counter1 = new int[12];
            for (int i = 0; i < 12; i++)
            {
                counter0[i] = 0;
                counter1[i] = 0;
            }
            foreach (string line in consumption)
            {
                char[] numeri = line.ToCharArray();
                for (int i = 0; i < 12; i++)
                {
                    if (numeri[i] == '1')
                    {
                        counter1[i]++;
                    }
                    else counter0[i]++;
                }
            }

            int gamma, epsilon;
            int[] gammaArray = new int[12];
            int[] epsilonArray = new int[12];
            for (int i = 0; i < 12; i++)
            {
                if (counter0[i] < counter1[i])
                {
                    gammaArray[i] = 1;
                    epsilonArray[i] = 0;
                }
                else
                {
                    gammaArray[i] = 0;
                    epsilonArray[i] = 1;
                }
            }
        }


    }
}