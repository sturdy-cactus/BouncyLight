/*
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Transactions;
using mialibreria;
using static test.TestGeometry;
using geometry;
using Vector = geometry.Vector;

namespace geometry2;

public struct Point
{
    public float x, y, z;

    public Point()
    {
        x = 0;
        y = 0;
        z = 0;
    }
    public Point(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public string toString()
    {
        string s = $"(x = {x.ToString()}, y = {y.ToString()}, z = {z.ToString()})";
        return s;
    }

    public bool isClose(Point p)
    {
        return (IsClose(x,p.x) && IsClose(y,p.y) && IsClose(z,p.z));
        Point a = new Point(1, 2, 3);
        Point b = new Point(2, 3, 4);
        Debug.Assert(a.isClose(b));
    }
    
    public static Point operator *(Point p,float a)
    {
        Point temp = p;
        temp.x *= a;
        temp.y *= a;
        temp.z *= a;
        return temp;
    }

    public static Point operator *(float a, Point p)
    {
        return p * a;
    }

    public static Point operator +(Point p, Vector v)
    {
        return new Point(p.x + v.x, p.y + v.y, p.z + v.z);
    }

    public static Vector operator -(Point p,Point p2)
    {
        return new Vector(p.x - p2.x, p.y - p2.y, p.z - p2.z);
    }

    public Vector toVector()
    {
        Vector v = new Vector();
        v.x = x;
        v.y = y;
        v.z = z;

        return v;
    }
}


public struct Transformation
{
    public Matrix4x4 m, invm;
    public Transformation()
    {
        m=Matrix4x4.Identity;
        invm=Matrix4x4.Identity;
    }

    public Transformation(Matrix4x4 m, Matrix4x4 invm)
    {
        this.m = m;
        this.invm = invm;
        try
        {
            Debug.Assert(IsClose(m*invm,Matrix4x4.Identity));
        }
        catch (Exception)
        {
            Console.WriteLine("Exception: matrix was not given with inverse");
            Environment.Exit(10);
        }
    }

    public static Transformation Traslation(Vector v)
    {
        var m = Matrix4x4.Identity + new Matrix4x4(0, 0, 0, v.x, 0, 0, 0, v.y, 0, 0, 0, v.z, 0, 0, 0, 0);
        Matrix4x4 invm;
        Matrix4x4.Invert(m, out invm);
        return new Transformation(m,invm);
    }

    public Transformation Inverse()
    {
        return new Transformation(m:invm, invm:m);
    }

    public static Transformation operator *(Transformation A, Transformation B)
    {
        var m = A.m * B.m;
        Matrix4x4 invm;
        Matrix4x4.Invert(m, out invm);
        
        return new Transformation(m, invm);
    }

    public static Point operator *(Transformation A, Point p)
    {
        var mat = A.m;
        var myPoint = new Point(
            x: p.x*mat.M11*p.y*mat.M12*p.z*mat.M13+mat.M14,
            y: p.x*mat.M21*p.y*mat.M22*p.z*mat.M23+mat.M24,
            z: p.x*mat.M31*p.y*mat.M32*p.z*mat.M33+mat.M34);
        var w = p.x * A.m.M41 * p.y * A.m.M42 * p.z * A.m.M43 + A.m.M44;
        if (IsClose(w, 1.0f))
            return myPoint;
        return new Point(myPoint.x / w, myPoint.y / w, myPoint.z / w);
    }

    public static Normal operator *(Transformation A, Normal n)
    {
        var mat = A.invm;
        return new Normal(
            x: n.x*mat.M11*n.y*mat.M12*n.z*mat.M13+mat.M14,
            y: n.x*mat.M21*n.y*mat.M22*n.z*mat.M23+mat.M24,
            z: n.x*mat.M31*n.y*mat.M32*n.z*mat.M33+mat.M34);
    }
}
*/