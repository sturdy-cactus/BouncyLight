using System.Diagnostics;
using System.Numerics;
using geometry;
using Vector = geometry.Vector;

namespace test;

public class TestGeometry
{
    public static void TestVec()
    {
        var a = new Vector(1.0f, 2.0f, 3.0f);
        var b = new Vector(4.0f, 6.0f, 8.0f);
        
        Debug.Assert(a.isClose(a));
        Debug.Assert(!a.isClose(b));
    }

    public static void TestVecOps()
    {
        var a = new Vector(1.0f, 2.0f, 3.0f);
        var b = new Vector(4.0f, 6.0f, 8.0f);

        Debug.Assert((a + b).isClose(new Vector(5.0f, 8.0f, 11.0f)));
        Debug.Assert((b-a).isClose(new Vector(3.0f, 4.0f, 5.0f)));
        Debug.Assert((2*a).isClose(new Vector(2.0f, 4.0f, 6.0f)));
        Debug.Assert(IsClose(40.0f, a*b));
        Debug.Assert(IsClose(14.0f, a.SqNorm()));
        Debug.Assert(IsClose(14.0f, (float)Math.Pow((double)a.Norm(), 2.0)));
        Debug.Assert((Vector.CrossProd(a,b)).isClose(new Vector(-2.0f, 4.0f, -2.0f)));
        Debug.Assert((Vector.CrossProd(a,b)).isClose(new Vector(2.0f, -4.0f, 2.0f)));
    }
    
    public static void TestPoint()
    {
        Point a = new Point(1, 2, 3);
        Point b = new Point(0.5f * 2, 1.0f * 2.0f,3.0f/1.0f);
        Point c = new Point(3.0f, 1.0f, 9000.3f);
        Debug.Assert(a.isClose(a));
        Debug.Assert(a.isClose(b));
        Debug.Assert(!a.isClose(c));
    }

    public static void TestPointOps()
    {
        Point p1 = new Point(1, 2, 3);
        Vector v2 = new Vector(4.0f, 6.0f, 8.0f);
        Point p2 = new Point(12, -3, 0);
        Debug.Assert((p1 * 2).isClose(new Point(2, 4, 6)));
        Debug.Assert((p1 + v2).isClose(new Point(5, 8, 11)));
        Debug.Assert((p1 - p2).isClose(new Vector(-11, 5, 3)));
        Debug.Assert((p1 + (p1 - p2)).isClose(new Point(0, 0, 0)));
    }

    public static void TestTransfOps()
    {
        var a = Matrix4x4.Identity;
        var b = new Matrix4x4(1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1);
        Debug.Assert(IsClose(a, b));
    }

    
    public static bool IsClose(float a, float b)
    {
        const float epsilon = 1e-4f; //fallisce a e-5!
        return Math.Abs(a - b) < epsilon;
    }
    
    public static bool IsClose(Matrix4x4 A, Matrix4x4 B)
    {
        bool mybool = true;
        for (int i = 1; i <= 4; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                float a = (float)A.GetType().GetField("M"+i+j).GetValue(A);
                float b = (float)B.GetType().GetField("M"+i+j).GetValue(B);
                mybool = mybool && IsClose(a, b);
            }
        }
        return mybool;
    }
}