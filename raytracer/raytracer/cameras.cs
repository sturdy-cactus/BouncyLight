using System;
using System.Runtime.InteropServices;
using geometry;
using static test.TestGeometry;

namespace ray;
struct Ray
{
    //MEMBRI
    public Point origin;
    public Vector direction;
    public float tMin;
    public float tMax;
    public int depth;
    
    //COSTRUTTORE DEFAULT
    public Ray(Point origin, Vector direction)
    {
        this.origin = origin;
        this.direction = direction;
        this.tMin = 1e-5f;
        this.tMax = Single.PositiveInfinity;
        this.depth = 0;
    }
    
    //COSTRUTTORE
    public Ray(Point origin, Vector direction, float tMin, float tMax, int depth)
    {
        this.origin = origin;
        this.direction = direction;
        this.tMin = tMin;
        this.tMax = tMax;
        this.depth = depth;
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
    
}