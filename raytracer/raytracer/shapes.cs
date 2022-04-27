using System;
using geometry;
using ray;

namespace shapes;

interface IShape
{
    public HitRecord RayIntersection(Ray ray);
}

class Sphere : IShape
{
    //MEMBERS
    public Transformation tr;
    
    //CONSTRUCTOR
    public Sphere(Transformation tr)
    {
        this.tr = tr;
    }

    //METHODS
    public HitRecord RayIntersection(Ray ray)
    {
        var o = new Vector();
        o = ray.origin.ToVector();
        float dotprod = (float) Math.Pow(o * ray.direction, 2);

        float deltared = dotprod - ray.direction.SqNorm() * (o.SqNorm() - 1.0f);
        
        return HitRecord
        
    }
}

struct HitRecord
{
    //MEMBERS
    public Point world_point;
    public Normal n;
    public Vector2D surface_point;
    public float t;
    public Ray ray;
}