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
        string s = "({this.x}, {this.y}, {this.z})";
        return s;
    }

    public Vector Sum(Vector v1, Vector v2)
    {
        var sum = new Vector();
        sum.x = v1.x + v2.x;
        sum.y = v1.y + v2.y;
        sum.z = v1.z + v2.z;

        return sum;
    }

    public Vector Diff(Vector v1, Vector v2)
    {
        var diff = new Vector();
        diff.x = v1.x + v2.x;
        diff.y = v1.y + v2.y;
        diff.z = v1.z + v2.z;

        return diff;
    }

    public Vector ScalProd(float a)
    {
        this.x = this.x * a;
        this.y = this.y * a;
        this.z = this.z * a;

        return this;
    }

    public Vector Neg()
    {
        return this.ScalProd(-1.0f);
    }

    public float DotProd(Vector v1, Vector v2)
    {
        float dotprod = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        return dotprod;
    }

    public Vector CrossProd(Vector v1, Vector v2)
    {
        var prod = new Vector();
        prod.x = v1.y * v2.z - v1.z * v2.y;
        prod.y = v1.z * v2.x - v1.x * v2.z;
        prod.z = v1.x * v2.y - v1.y * v2.x;

        return prod;
    }

    public float SqNorm()
    { 
        return DotProd(this,this);
    }

    public float Norm()
    {
        return (float)Math.Sqrt(DotProd(this, this));
    }

    public void Normalize()  
    {
        float norm = this.Norm();
        this.x = this.x / norm;
        this.y = this.y / norm;
        this.z = this.z / norm;
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
        string s = "({this.x}, {this.y}, {this.z})";
        return s;
    }
    
    public Normal ScalProd(float a)
    {
        this.x = this.x * a;
        this.y = this.y * a;
        this.z = this.z * a;

        return this;
    }

    public Normal Neg()
    {
        return this.ScalProd(-1.0f);
    }

    public float DotProd(Normal n1, Normal n2)
    {
        float dotprod = n1.x * n2.x + n1.y * n2.y + n1.z * n2.z;
        return dotprod;
    }
    public float VNDotProd(Vector v, Normal n)
    {
        float dotprod = v.x * n.x + v.y * n.y + v.z * n.z;
        return dotprod;
    }

    public Vector VNCrossProd(Vector v, Normal n)
    {
        var prod = new Vector();
        prod.x = v.y * n.z - v.z * n.y;
        prod.y = v.z * n.x - v.x * n.z;
        prod.z = v.x * n.y - v.y * n.x;

        return prod;
    }
    public Normal NCrossProd(Normal n1, Normal n2)
    {
        var prod = new Normal();
        prod.x = n1.y * n2.z - n1.z * n2.y;
        prod.y = n1.z * n2.x - n1.x * n2.z;
        prod.z = n1.x * n2.y - n1.y * n2.x;

        return prod;
    }
    
    public float SqNorm()
    { 
        return DotProd(this,this);
    }

    public float Norm()
    {
        return (float)Math.Sqrt(DotProd(this, this));
    }
    
    public void Normalize()  
    {
        float norm = this.Norm();
        this.x = this.x / norm;
        this.y = this.y / norm;
        this.z = this.z / norm;
    }
}


