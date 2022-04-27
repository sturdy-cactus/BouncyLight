using System;
using geometry;
using ray;

namespace shapes;

interface IShape
{
    public HitRecord? RayIntersection(Ray ray);
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
    public HitRecord? RayIntersection(Ray ray)
    {
        //prep
        float firstHit;
        var hit = new HitRecord();
        this.tr = this.tr.Inverse();
        ray = this.tr * ray;
        var o = new Vector();
        o = ray.origin.ToVector();
        
        //body
        float dotprod = (float) Math.Pow(o * ray.direction, 2);
        float deltared = dotprod - ray.direction.SqNorm() * (o.SqNorm() - 1.0f);
        if (deltared <= .0f)
        {
            return null;
        }

        float t1 = -(o * ray.direction + (float)Math.Sqrt(deltared)) / ray.direction.SqNorm();
        float t2 = (-(o * ray.direction) + (float)Math.Sqrt(deltared)) / ray.direction.SqNorm();

        if (t1 > ray.tMin && t1 < ray.tMax)
            firstHit = t1;
        else if (t2 > ray.tMin && t2 < ray.tMax)
            firstHit = t2;
        
        
        

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
    
    //CONSTRUCTOR
    public HitRecord(Point? world_point = null, Normal? n = null, Vector2D? surface_point = null, float? t = null, Ray? ray = null)
    {
        this.world_point = world_point ?? new Point();
        this.n = n ?? new Normal();
        this.surface_point = surface_point ?? new Vector2D();
        this.t = t ?? .0f;
        this.ray = ray ?? new Ray();
    }
}

struct World
{
    private List<IShape> Shapes;

    public void Add(IShape myshape)
    {
        Shapes.Add(myshape);
    }

    public HitRecord? RayIntersection(Ray ray)
    {
        HitRecord? closest = null;

        foreach (var shape in Shapes)
        {
            HitRecord? intersection = shape.RayIntersection(ray);
            if (intersection != null)
                continue;
            else if (closest == null || intersection.t < closest.t)
                closest = intersection;
        }
        
        return closest;
    }
}