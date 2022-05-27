using BRDF;
using Geometry;
using Cameras;

namespace Shapes;

public interface IShape
{
    public Material GetMaterial();
    public HitRecord? RayIntersection(Ray ray);
}

public class Plane : IShape
{
    //MEMBERS
    private Material _material;
    private Transformation _tr;
    
    //CONSTRUCTOR
    public Plane (Transformation? tr = null, Material? mat = null)
    {
        this._tr = tr ?? new Transformation();
        this._material = mat ?? new Material();
    }

    public Material GetMaterial()
    {
        return _material;
    }
    
    public HitRecord? RayIntersection(Ray rayId)
    {
        this._tr.Inverse();
        var ray = rayId;
        rayId = this._tr * rayId;
        this._tr.Inverse();

        if (Math.Abs(rayId.Direction.z) < 1e-5)
            return null;

        var t = -rayId.Origin.z / rayId.Direction.z;
        if (t <= rayId.TMin || t >= rayId.TMax)
            return null;

        var HitPoint = rayId.At(t);
        
        var z = 1;
        if (rayId.Direction.z > 0)
            z = -1;

        return new HitRecord(wPoint: _tr * HitPoint, n: _tr * new Normal(0, 0, z),
            sPoint: new Vector2D((float)(HitPoint.x - Math.Floor(HitPoint.x)), (float)(HitPoint.y - Math.Floor(HitPoint.y))), t: t,
            ray: ray, mat: _material);

    }
}

public class Sphere : IShape
{
    //MEMBERS
    public Material _material;
    private Transformation _tr;
    
    //CONSTRUCTOR
    public Sphere(Transformation? tr = null, Material? mat = null)
    {
        this._tr = tr ?? new Transformation();
        this._material = mat ?? new Material();
    }

    //METHODS
    public Material GetMaterial()
    {
        return _material;
    }
    
    public HitRecord? RayIntersection(Ray rayId)
    {
        //prep
        float firstHit;
        var hit = new HitRecord();
        this._tr.Inverse();
        rayId = this._tr * rayId;
        this._tr.Inverse();
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
    public Material Mat;
    
    //CONSTRUCTOR
    public HitRecord(Point? wPoint = null, Normal? n = null, Vector2D? sPoint = null, float? t = null, Ray? ray = null, Material? mat=null)
    {
        this.WPoint = wPoint ?? new Point();
        this.N = n ?? new Normal();
        this.SPoint = sPoint ?? new Vector2D();
        this.T = t ?? .0f;
        this.Ray = ray ?? new Ray();
        this.Mat = mat ?? new Material();
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

    public HitRecord? RIntersection(Ray ray)
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
                p.Mat = shape.GetMaterial();
                
                if (closest == null)
                    closest = p;
                else if (p.T < closest.Value.T)
                    closest = p;
            }
        }
        
        return closest;
    }
}
