using System.Diagnostics;
using System.Numerics;
using static test.TestGeometry;

namespace geometry;

public struct Vector
{
    //MEMBRI
    public float x;
    public float y;
    public float z;

    //COSTRUTTORE DEFAULT
    public Vector()
    {
        x = .0f;
        y = .0f;
        z = .0f;
    }

    //COSTRUTTORE
    public Vector(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    //METODI
    public string ConvertVecToString()
    {
        string s = $"({this.x}, {this.y}, {this.z})";
        return s;
    }

    public bool isClose(Vector a)
    {
        return (IsClose(a.x, this.x) && IsClose(a.y, this.y) && IsClose(a.z, this.z));
    }

    public Vector Neg()
    {
        return -1.0f * this;
    }

    public float SqNorm()
    {
        return this * this;
    }

    public float Norm()
    {
        return (float)Math.Sqrt(this.SqNorm());
    }

    public void Normalize()
    {
        float norm = this.Norm();
        this.x = this.x / norm;
        this.y = this.y / norm;
        this.z = this.z / norm;
    }

    public static Vector CrossProd(Vector v1, Vector v2)
    {
        var prod = new Vector();
        prod.x = v1.y * v2.z - v1.z * v2.y;
        prod.y = v1.z * v2.x - v1.x * v2.z;
        prod.z = v1.x * v2.y - v1.y * v2.x;

        return prod;
    }

    //OPERATORI
    public static Vector operator +(Vector v1, Vector v2)
    {
        var sum = new Vector();
        sum.x = v1.x + v2.x;
        sum.y = v1.y + v2.y;
        sum.z = v1.z + v2.z;

        return sum;
    }

    public static Vector operator -(Vector v1, Vector v2)
    {
        var diff = new Vector();
        diff.x = v1.x - v2.x;
        diff.y = v1.y - v2.y;
        diff.z = v1.z - v2.z;

        return diff;
    }

    public static Vector operator *(float a, Vector v)
    {
        var p = new Vector();
        p.x = v.x * a;
        p.y = v.y * a;
        p.z = v.z * a;

        return p;
    }

    public static float operator *(Vector v1, Vector v2)
    {
        float dotprod = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        return dotprod;
    }

}

public struct Point
{
    //MEMBRI E COSTRUTTORE
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

    //METODI
    public string toString()
    {
        string s = $"(x = {x.ToString()}, y = {y.ToString()}, z = {z.ToString()})";
        return s;
    }

    public bool isClose(Point p)
    {
        return (IsClose(x,p.x) && IsClose(y,p.y) && IsClose(z,p.z));
    }
    
    public Vector toVector()
    {
        Vector v = new Vector();
        v.x = x;
        v.y = y;
        v.z = z;

        return v;
    }
    
    public static bool IsClose(float a, float b)
    {
        const float epsilon = 1e-4f;
        return Math.Abs(a - b) < epsilon;
    }
    
    //OPERATORI
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
    
}

public struct Normal
{
    //MEMBRI
    public float x;
    public float y;
    public float z;

    //COSTRUTTORE DEFAULT
    public Normal()
    {
        x = .0f;
        y = .0f;
        z = .0f;
    }

    //COSTRUTTORE
    public Normal(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    //METODI
    public string ConvertNormToString()
    {
        string s = $"({this.x}, {this.y}, {this.z})";
        return s;
    }

    public bool isClose(Normal a)
    {
        return (IsClose(a.x, this.x) && IsClose(a.y, this.y) && IsClose(a.z, this.z));
    }

    public Normal Neg()
    {
        return -1.0f * this;
    }

    public float SqNorm()
    {
        return this * this;
    }

    public float Norm()
    {
        return (float)Math.Sqrt(this.SqNorm());
    }

    public void Normalize()
    {
        float norm = this.Norm();
        this.x = this.x / norm;
        this.y = this.y / norm;
        this.z = this.z / norm;
    }

    public static Normal CrossProd(Normal n1, Normal n2)
    {
        var prod = new Normal();
        prod.x = n1.y * n2.z - n1.z * n2.y;
        prod.y = n1.z * n2.x - n1.x * n2.z;
        prod.z = n1.x * n2.y - n1.y * n2.x;

        return prod;
    }

    public Vector VNCrossProd(Vector v, Normal n)
    {
        var prod = new Vector();
        prod.x = v.y * n.z - v.z * n.y;
        prod.y = v.z * n.x - v.x * n.z;
        prod.z = v.x * n.y - v.y * n.x;

        return prod;
    }

    //OPERATORI
    public static Normal operator +(Normal n1, Normal n2)
    {
        var sum = new Normal();
        sum.x = n1.x + n2.x;
        sum.y = n1.y + n2.y;
        sum.z = n1.z + n2.z;

        return sum;
    }

    public static Normal operator -(Normal n1, Normal n2)
    {
        var diff = new Normal();
        diff.x = n1.x - n2.x;
        diff.y = n1.y - n2.y;
        diff.z = n1.z - n2.z;

        return diff;
    }

    public static Normal operator *(float a, Normal n)
    {
        var p = new Normal();
        p.x = n.x * a;
        p.y = n.y * a;
        p.z = n.z * a;

        return p;
    }

    public static float operator *(Normal n1, Normal n2)
    {
        float dotprod = n1.x * n2.x + n1.y * n2.y + n1.z * n2.z;
        return dotprod;
    }

    public static float operator *(Vector v, Normal n)
    {
        float dotprod = v.x * n.x + v.y * n.y + v.z * n.z;
        return dotprod;
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