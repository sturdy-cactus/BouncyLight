using System;
using System.Drawing;
using System.Runtime.InteropServices;
using geometry;
using static test.TestGeometry;
using Point = geometry.Point;

namespace ray;
struct Ray
{
    //MEMBRI
    public Point origin;
    public Vector direction;
    public float tMin;
    public float tMax;
    public int depth;
    
    //COSTRUTTORE
    public Ray(Point origin, Vector direction, float? tMin=null, float? tMax=null, int? depth=null)
    {
        this.origin = origin;
        this.direction = direction;
        this.tMin = tMin ?? 1e-5f;
        this.tMax = tMax ?? float.PositiveInfinity;
        this.depth = depth ?? 0;
    }
    
    //METODI
    public Point At(float t)
    {
        return this.origin + t * this.direction;
    }

    public bool isClose(Ray r)
    {
        return (this.direction.isClose(r.direction) && this.origin.isClose(r.origin));
    }
    
    public static Ray operator*(Transformation t, Ray r)
    {
        Ray temp = r;
        temp.direction = t*r.direction;
        temp.origin = t * r.origin;
        return temp;
    }

}