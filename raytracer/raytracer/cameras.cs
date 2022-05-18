using Geometry;
using PFMlib;
using Shapes;
using Point = Geometry.Point;

namespace Cameras;

public interface ICamera
{
    public Ray FireRay(float u, float v);
    public ICamera SetCamera(Transformation t);
}

public class OrthCamera : ICamera
{
    //MEMBERS
    private float _a;
    private Transformation _t;
    
    //CONSTRUCTOR
    public OrthCamera(float? a = null, Transformation? t = null)
    {
        this._a = a ?? 1.0f;
        this._t = t ?? new Transformation();
    }
    
    //METODI
    public Ray FireRay(float u, float v)
    {
        var o = new Point(-1.0f, -(2.0f * u - 1.0f) * this._a, 2.0f * v - 1.0f);
        var dir = new Vector(1.0f, .0f, .0f);
        var ray = new Ray(o, dir, tMin: .0f); //    tMin changed

        return this._t * ray;
    }

    public ICamera SetCamera(Transformation t)
    {
        this._t = t;
        return this;
    }
}

public class PerspCamera : ICamera
{
    //MEMBERS
    private float _a;
    private float _d;
    private Transformation _t;

    //CTOR
    public PerspCamera(float? a = null, float? d = null, Transformation? t = null)
    {
        this._a= a ?? 1.0f;
        this._d = d ?? 1.0f;
        this._t = t ?? new Transformation();

    }
    
    //METODI
    public Ray FireRay(float u, float v)
    {
        var o = new Point(-this._d, .0f, .0f);
        var dir = new Vector(this._d, -(2.0f * u - 1.0f) * this._a, 2.0f * v - 1.0f);
        var ray = new Ray(o, dir, tMin: .0f);

        return this._t * ray;
    }
    
    public ICamera SetCamera(Transformation t)
    {
        this._t = t;
        return this;
    }
}

public class ImgTracer
{
    //MEMBERS
    private HdrImage img;
    private ICamera cam;
    
    //CTOR
    public ImgTracer(HdrImage img, ICamera cam)
    {
        this.img = img;
        this.cam = cam;
    }
    
    //METODI
    public void FireAllRays(World world)
    {
        var color= new Color();
        var ray = new Ray();
        var progress = img.w / 40;
        Console.WriteLine("\ngenerazione dell'immagine in corso...\n________________________________________");
        for (int col = 0; col < img.w; col++)
        {
            if (col % progress == 0)
                Console.Write("*");
            
            for (int row = 0; row < img.h; row++)
            {
                ray = this.FRay(col, row);
                var hr = world.RIntersection(ray);
                if (hr == null)
                    color = new Color();
                else
                    color = new Color(1, 1, 1);

                this.img.SetPixel(color, row, col);
            }
        }
    }
    
    //INTERNAL METHODS
    public Ray FRay(int col, int row, float uPix = .5f, float vPix = .5f)
    {
        float u = (col + uPix) / (this.img.w); 
        float v = 1 - (row + vPix) / (this.img.h); 

        return cam.FireRay(u, v);
    }
}

public struct Ray
{
    //MEMBERS
    public Point Origin;
    public Vector Direction;
    public float TMin;
    public float TMax;
    public int Depth;
    
    //CTOR
    public Ray(Point origin, Vector direction, float? tMin = null, float? tMax = null, int? depth = null)
    {
        this.Origin = origin;
        this.Direction = direction;
        this.TMin = tMin ?? 1e-5f;
        this.TMax = tMax ?? float.PositiveInfinity;
        this.Depth = depth ?? 0;
    }
    
    //METODI
    public Point At(float t)
    {
        return this.Origin + t * this.Direction;
    }

    public bool isClose(Ray r)
    {
        return (this.Direction.isClose(r.Direction) && this.Origin.isClose(r.Origin));
    }
    
    public static Ray operator *(Transformation t, Ray r)
    {
        Ray temp = r;
        temp.Direction = t * r.Direction;
        temp.Origin = t * r.Origin;
        return temp;
    }
}

