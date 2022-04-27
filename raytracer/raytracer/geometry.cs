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

    public Point ToPoint()
    {
        var p = new Point();
        p.x = this.x;
        p.y = this.y;
        p.z = this.z;

        return p;
    }
    public bool isClose(Vector a)
    {
        return (IsClose(a.x, x) && IsClose(a.y, y) && IsClose(a.z, z));
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
        var sum = new Vector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        
        return sum;
    }

    public static Vector operator -(Vector v1, Vector v2)
    {
        var diff = new Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

        return diff;
    }

    public static Vector operator *(float a, Vector v)
    {
        var p = new Vector(v.x * a, v.y * a, v.z * a);
        
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
    
    public Vector ToVector()
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
        var sum = new Normal(n1.x + n2.x, n1.y + n2.y, n1.z + n2.z);
        
        return sum;
    }

    public static Normal operator -(Normal n1, Normal n2)
    {
        var diff = new Normal(n1.x - n2.x, n1.y - n2.y, n1.z - n2.z);

        return diff;
    }

    public static Normal operator *(float a, Normal n)
    {
        var p = new Normal(n.x * a, n.y * a, n.z * a);
        
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
    //MEMBRI
    public Matrix4x4 m;
    public Matrix4x4 invm;
    
    //COSTRUTTORE
    public Transformation()
    {
        m = Matrix4x4.Identity;
        invm = Matrix4x4.Identity;
    }
    
    //alternative ctor
    public Transformation(Matrix4x4 m)
    {
        this.m = m;
        Matrix4x4.Invert(m, out this.invm);
        
        try
        { 
            Matrix4x4.Invert(m, out this.invm);
        }
        catch (Exception)
        {
            Console.WriteLine("Exception: matrix is singular");
            Environment.Exit(10);
        }
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

    //METODI
    public static Transformation Translation(Vector v)
    {
        var m = Matrix4x4.Identity + new Matrix4x4(0, 0, 0, v.x, 0, 0, 0, v.y, 0, 0, 0, v.z, 0, 0, 0, 0);
        Matrix4x4 invm;
        Matrix4x4.Invert(m, out invm);
        return new Transformation(m,invm);
    }

    public static Transformation Rotation(float angle, char direction)
    {
        var t = new Transformation();
        switch (direction)
        {
            case 'x':
                t.m = Matrix4x4.CreateRotationX(angle);
                //t.invm = Matrix4x4.CreateRotationX(-angle);
                break;
            case 'y' :
                t.m = Matrix4x4.CreateRotationY(angle);
                //t.invm = Matrix4x4.CreateRotationY(-angle);
                break;
            case 'z':
                t.m = Matrix4x4.CreateRotationZ(angle);
                //t.invm = Matrix4x4.CreateRotationZ(-angle);
                break;
            default:
                Console.WriteLine("Invalid direction"); //sistemare eccezione
                Environment.Exit(11);
                break;
        }

        Matrix4x4.Invert(t.m, out t.invm);
        return t;
    }

    public static Transformation Scaling(Vector v)
    {
        var m = new Matrix4x4(v.x, 0, 0, 0, 0, v.y, 0, 0, 0, 0, v.z, 0, 0, 0, 0, 1);
        var invm = new Matrix4x4(1 / v.x, 0, 0, 0, 0, 1 / v.y, 0, 0, 0, 0, 1 / v.z, 0, 0, 0, 0, 1);
        return new Transformation(m, invm);
    }

    public Transformation Inverse()
    {
        return new Transformation(this.invm, this.m);
    }

    //OPERATORI
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
            x: p.x * mat.M11 + p.y*mat.M12 + p.z*mat.M13 + mat.M14,
            y: p.x * mat.M21 + p.y*mat.M22 + p.z * mat.M23 + mat.M24,
            z: p.x * mat.M31 + p.y*mat.M32 + p.z*mat.M33 + mat.M34);
        var w = p.x * mat.M41 + p.y * mat.M42 + p.z * mat.M43 + mat.M44;
        if (IsClose(w, 1.0f))
            return myPoint;
        return new Point(myPoint.x / w, myPoint.y / w, myPoint.z / w);
    }

    public static Normal operator *(Transformation A, Normal n)
    {
        var mat = A.invm;
        mat = Matrix4x4.Transpose(mat);
        
        return new Normal(
            x: n.x*mat.M11*n.y*mat.M12*n.z*mat.M13+mat.M14,
            y: n.x*mat.M21*n.y*mat.M22*n.z*mat.M23+mat.M24,
            z: n.x*mat.M31*n.y*mat.M32*n.z*mat.M33+mat.M34);
    }

    public static Vector operator *(Transformation A, Vector v)
    {
        var mat = A.m;
        var vec = new Vector(
            x: v.x * mat.M11 + v.y*mat.M12 + v.z*mat.M13 + mat.M14,
            y: v.x * mat.M21 + v.y*mat.M22 + v.z * mat.M23 + mat.M24,
            z: v.x * mat.M31 + v.y*mat.M32 + v.z*mat.M33 + mat.M34);
        
        return new Vector(vec.x, vec.y, vec.z);
    }
}

public struct Vector2D
{
    //MEMBRI
    private float x;
    private float y;

    //COSTRUTTORE DEFAULT
    public Vector2D()
    {
        x = .0f;
        y = .0f;
    }

    //COSTRUTTORE
    public Vector2D(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    //METODI
    public string ConvertVecToString()
    {
        string s = $"({this.x}, {this.y})";
        return s;
    }

    public float GetU()
    {
        return this.x;
    }
    
    public float GetV()
    {
        return this.y;
    }

    //OPERATORI
    public static Vector2D operator +(Vector2D v1, Vector2D v2)
    {
        var sum = new Vector2D(v1.x + v2.x, v1.y + v2.y);
        
        return sum;
    }

    public static Vector2D operator -(Vector2D v1, Vector2D v2)
    {
        var diff = new Vector2D(v1.x - v2.x, v1.y - v2.y);

        return diff;
    }

    public static Vector2D operator *(float a, Vector2D v)
    {
        var p = new Vector2D(v.x * a, v.y * a);
        
        return p;
    }

    public static float operator *(Vector2D v1, Vector2D v2)
    {
        float dotprod = v1.x * v2.x + v1.y * v2.y;
        return dotprod;
    }

}