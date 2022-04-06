using System;
using System.IO;

namespace geometry;

//fiore

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
   
    public Vector Neg()
    {
        return -1.0f*this;
    }
    
    public float SqNorm()
    { 
        return this*this;
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
    } //esatto con numeri vicini a 1
    
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
    
    //METODI
    public string ConvertNormToString()
    {
        string s = $"({this.x}, {this.y}, {this.z})";
        return s;
    }
    
    public Normal Neg()
    {
        return -1.0f*this;
    }
    
    public float SqNorm()
    { 
        return this*this;
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

    public static Normal operator *(float a, Normal v)
    {
        var p = new Normal();
        p.x = v.x * a;
        p.y = v.y * a;
        p.z = v.z * a;

        return p;
    }

    public static float operator *(Normal n1, Normal n2)
    {
        float dotprod = n1.x * n2.x + n1.y * n2.y + n1.z * n2.z;
        return dotprod;
    }
}
