using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using Geometry;
using RandomNumber;
using test;

namespace ONB;

struct ONB
{
    public Vector e1;
    public Vector e2;
    public Vector e3;

    public ONB(Vector normal)
    {
        var sign = -1;
        if (normal.z > 0)
            sign = 1;

        var a = -1 / (sign + normal.z);
        var b = normal.x * normal.y * a;

        e1 = new Vector(1 + sign * normal.x * normal.x * a, sign * b, -sign * normal.x);
        e2 = new Vector(b, sign + normal.y * normal.y * a, -normal.y);
        e3 = normal;
    }
    
    public ONB(Normal normal)
    {
        var sign = -1;
        if (normal.z > 0)
            sign = 1;

        var a = -1 / (sign + normal.z);
        var b = normal.x * normal.y * a;

        e1 = new Vector(1 + sign * normal.x * normal.x * a, sign * b, -sign * normal.x);
        e2 = new Vector(b, sign + normal.y * normal.y * a, -normal.y);
        e3 = new Vector(normal.x,normal.y,normal.z);
    }

}

struct test
{
    public static void TestONB()
    {
        var pcg = new PCG();
        for (int i = 0; i < 1000; i++)
        {
            var normvec = new Vector(pcg.RandomFloat(), pcg.RandomFloat(), pcg.RandomFloat());
            normvec.Normalize();
            var onb = new ONB(normvec);
            
            Debug.Assert(normvec.isClose(onb.e3));
            Debug.Assert(IsClose(onb.e1 * onb.e2, 0));
            Debug.Assert(IsClose(onb.e1 * onb.e3, 0));
            Debug.Assert(IsClose(onb.e2 * onb.e3, 0));
            
            Debug.Assert(IsClose(onb.e1.SqNorm(), 1));
            Debug.Assert(IsClose(onb.e2.SqNorm(), 1));
            Debug.Assert(IsClose(onb.e3.SqNorm(), 1));

        }
    }
    
    public static bool IsClose(float a, float b)
    {
        const float epsilon = 1e-4f; //fallisce a e-5!
        return Math.Abs(a - b) < epsilon;
    }
}