using System;
using Geometry;
using Cameras;

namespace Shapes;

interface IShape
{
    public HitRecord? RayIntersection(Ray ray);
}

class Sphere : IShape
{
    //MEMBERS
    public Transformation tr;
    
    //CONSTRUCTOR
    public Sphere(Transformation? tr = null)
    {
        this.tr = tr ?? new Transformation();
    }

    //METHODS
    public HitRecord? RayIntersection(Ray ray)
    {
        //prep
        float firstHit;
        var hit = new HitRecord();
        var trsf = this.tr.Inverse();
        ray = trsf * ray;
        var o = ray.origin.ToVector();

        //body
        float dotprod = (float) Math.Pow(o * ray.direction, 2);
        float delta = dotprod - ray.direction.SqNorm() * (o.SqNorm() - 1.0f);
        if (delta <= .0f)
            return null;
        
        float t1 = -(o * ray.direction + (float)Math.Sqrt(delta)) / ray.direction.SqNorm();
        float t2 = (-(o * ray.direction) + (float)Math.Sqrt(delta)) / ray.direction.SqNorm();

        if (t1 > ray.tMin && t1 < ray.tMax)
            firstHit = t1;
        else if (t2 > ray.tMin && t2 < ray.tMax)
            firstHit = t2;
        else
            return null;
        
        //end 
        hit.WPoint = ray.At(firstHit); 
        //review
        hit.N = Sphere.SphereNormal(hit.WPoint, ray);
        hit.SPoint = SpherePointToUv(hit.WPoint);
        
        hit.T = firstHit;
        hit.Ray = ray;
        
        return hit;
    }
    
    //PRIVATE METHODS
    private static Normal SphereNormal(Point p, Ray ray)
    {
        var n = new Normal(p.x, p.y, p.z);
        var vec = new Vector(p.x, p.y, p.z);
        if (ray.direction * vec < .0f)
            return n;
        else
            return new Normal(-p.x, -p.y, -p.z);
    }

    private static Vector2D SpherePointToUv(Point p)
    {
        var vec = new Vector2D();
        vec.SetU((float)Math.Atan2(p.y, p.x)/(2.0f * (float)Math.PI));
        vec.SetV((float)Math.Acos(p.z)/((float)Math.PI));

        return vec;
    }

}

struct HitRecord
{
    //MEMBERS
    public Point WPoint;
    public Normal N;
    public Vector2D SPoint;
    public float T;
    public Ray Ray;
    
    //CONSTRUCTOR
    public HitRecord(Point? wPoint = null, Normal? n = null, Vector2D? sPoint = null, float? t = null, Ray? ray = null)
    {
        this.WPoint = wPoint ?? new Point();
        this.N = n ?? new Normal();
        this.SPoint = sPoint ?? new Vector2D();
        this.T = t ?? .0f;
        this.Ray = ray ?? new Ray();
    }
}
/*
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
*/