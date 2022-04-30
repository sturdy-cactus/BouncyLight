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
        hit.wPoint = ray.At(firstHit); 
        //rivedere
        hit.n = Sphere.SphereNormal(hit.wPoint, ray);
        hit.sPoint = SpherePointToUv(hit.wPoint);
        
        hit.t = firstHit;
        hit.ray = ray;
        
        return hit;
    }
    
    //PRIVATE METHODS
    private static Normal SphereNormal(Point p, Ray ray)
    {
        var n = new Normal(p.x, p.y, p.z);
        var vec = new Vector(p.x, p.y, p.z);
        if (ray.direction * vec < 0)
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
    public Point wPoint;
    public Normal n;
    public Vector2D sPoint;
    public float t;
    public Ray ray;
    
    //CONSTRUCTOR
    public HitRecord(Point? wPoint = null, Normal? n = null, Vector2D? sPoint = null, float? t = null, Ray? ray = null)
    {
        this.wPoint = wPoint ?? new Point();
        this.n = n ?? new Normal();
        this.sPoint = sPoint ?? new Vector2D();
        this.t = t ?? .0f;
        this.ray = ray ?? new Ray();
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