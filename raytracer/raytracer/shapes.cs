using Geometry;
using Cameras;

namespace Shapes;

public interface IShape
{
    public HitRecord? RayIntersection(Ray ray);
}

public class Sphere : IShape
{
    //MEMBERS
    private Transformation _tr;
    
    //CONSTRUCTOR
    public Sphere(Transformation? tr = null)
    {
        this._tr = tr ?? new Transformation();
    }

    //METHODS
    public HitRecord? RayIntersection(Ray rayId)
    {
        //prep
        float firstHit;
        var hit = new HitRecord();
        rayId = this._tr.Inverse() * rayId;
        var o = rayId.Origin.ToVector();

        //body
        float prod = (float) Math.Pow(o * rayId.Direction, 2);
        float delta = prod - rayId.Direction.SqNorm() * (o.SqNorm() - 1.0f);
        if (delta <= .0f)
            return null;
        
        float t1 = -(o * rayId.Direction + (float)Math.Sqrt(delta)) / rayId.Direction.SqNorm();
        float t2 = (-(o * rayId.Direction) + (float)Math.Sqrt(delta)) / rayId.Direction.SqNorm();

        if (t1 > rayId.TMin && t1 < rayId.TMax)
            firstHit = t1;
        else if (t2 > rayId.TMin && t2 < rayId.TMax)
            firstHit = t2;
        else
            return null;
        
        //end 
        hit.WPoint = this._tr * rayId.At(firstHit);
        hit.N = this._tr * Sphere.SphereNormal( rayId.At(firstHit), rayId); 
        hit.SPoint = SpherePointToUv(rayId.At(firstHit));
        
        hit.T = firstHit;
        hit.Ray = rayId;
        
        return hit;
    }
    
    //PRIVATE METHODS
    private static Normal SphereNormal(Point p, Ray ray)
    {
        var n = new Normal(p.x, p.y, p.z);
        var vec = new Vector(p.x, p.y, p.z);
        if (ray.Direction * vec < .0f)
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

public struct HitRecord
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

public struct World
{
    private List<IShape> Shapes;

    public World()
    {
        Shapes = new List<IShape>();
    }
    
    public void Add(IShape shape)
    {
        Shapes.Add(shape);
    }

    public HitRecord? RayIntersection(Ray ray)
    {
        HitRecord? closest = null;
        
        foreach (var shape in Shapes)
        {
            HitRecord? intersection = shape.RayIntersection(ray);

            if (intersection == null)
                continue;
            else
            {
                var p = intersection.Value;
                
                if (closest == null)
                    closest = intersection;
                else if (p.T < closest.Value.T)
                    closest = intersection;
            }
        }
        
        return closest;
    }
}
