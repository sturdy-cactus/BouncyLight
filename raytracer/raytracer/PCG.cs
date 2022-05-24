using System.Diagnostics;

namespace RandomNumber;


    public struct PCG
    {
        //MEMBERS
        public ulong state;
        public ulong inc;

        //CONSTURCTORS
        //dovrebbe essere ridondante, ma non lo è!
        public PCG()
        {
            ulong initState = 42;
            ulong initSeq = 54;
            state = 0;
            inc = (initSeq << 1) | 1;

            this.Random();
            state += initState;
            this.Random();
        }

        public PCG(ulong initSt = 42, ulong initSq = 54)
        {
            ulong initState = initSt;
            ulong initSeq = initSq;
            state = 0;
            inc = (initSeq << 1) | 1;

            this.Random();
            state += initState;
            this.Random();
        }
        
        //METHODS   
        public uint Random()
        {
            ulong oldstate = state;
            state = (ulong) (inc + oldstate * 6364136223846793005);

            uint xorshifted = (uint) (((oldstate >> 18) ^ oldstate) >> 27);

            //rot dovrebbe essere uint. Controlla qui se c'è problema. Lo shift di 59 bit dovrebbe permettere di adottare un int
            int rot = (int) (oldstate >> 59);

            uint result = (uint) ((xorshifted >> rot) | (xorshifted << ((-rot) & 31)));
            return result;
        }

        public float RandomFloat()
        {
            return (float)Random() / 0xffffffff;
        }
    }

public struct test
{
    public static void testRand()
    {
        var pcg = new PCG();
        Debug.Assert(pcg.state==1753877967969059832);
        Debug.Assert(pcg.inc==109);
        
        Debug.Assert(pcg.Random()==2707161783);
        Debug.Assert(pcg.Random()==2068313097);
        Debug.Assert(pcg.Random()==3122475824);
        Debug.Assert(pcg.Random()==2211639955);
        Debug.Assert(pcg.Random()==3215226955);
        Debug.Assert(pcg.Random()==3421331566);

    }
}