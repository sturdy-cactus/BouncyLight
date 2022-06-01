using System.Diagnostics;
using BRDF;
using Geometry;
using PFMlib;
using Shapes;
using RandomNumber;
using test;
using static test.isClose;
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
    public void OnAndOffRenderer(World world, Color? background = null)
    {
        var color = background ?? new Color();
        var colorOff = color;
        var colorOn = new Color(1, 1, 1);
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
                    color = colorOff;
                else
                    color = colorOn;

                this.img.SetPixel(color, row, col);
            }
        }
    }

    public void FlatRenderer(World world, Color? background = null)
    {
        var color = background ?? new Color();
        var colorOff = color;
        var colorOn = new Color(1, 1, 1);
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
                HitRecord? hr = world.RIntersection(ray);
                if (!hr.HasValue)
                    color = colorOff;
                else
                {
                    var mat = hr.Value.Mat;
                    color = mat.EmittedRadiance.GetColor(hr.Value.SPoint);
                }

                this.img.SetPixel(color, row, col);
            }
        }
    }

    //il numero di raggi N non corrisponde al numero di pixel?
    public void PathRenderer(World world, PCG pcg, int N, int maxDepth, int iterLimit, Color background)
    {
        var tracer = new PathTracer(world, pcg, N, maxDepth, iterLimit, background);
        var progress = img.w / 40;
        Console.WriteLine("\ngenerazione dell'immagine in corso...\n________________________________________");
        for (int col = 0; col < img.w; col++)
        {
            if (col % progress == 0)
                Console.Write("*");
            for (int row = 0; row < img.h; row++)
            {
                var ray = FRay(col, row);
                var color = tracer.Roulette(ray);
                img.SetPixel(color, row, col);
                Console.WriteLine($"{row}, {col}");
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

public struct PathTracer
{
    //MEMBERS
    private World _world;
    private PCG _pcg;
    private int _n;
    private int _maxDepth;
    private int _iterLimit;
    private Color _background;

    //CTOR
    public PathTracer(World world, PCG pcg, int N, int maxDepth, int iterLimit, Color background)
    {
        _world = world;
        _pcg = pcg;
        _n = N;
        _maxDepth = maxDepth;
        _iterLimit = iterLimit;
        _background = background;
    }
    
    //METHODS
    public Color Roulette(Ray ray)
    {
        //prep
        if (ray.Depth > _maxDepth)
            return new Color();
                
        HitRecord? hr = _world.RIntersection(ray);
        if (hr == null)
            return _background;

        var hR = hr.Value;
        var hitMaterial = hR.Mat;
        var hitBRDF = hitMaterial.brdf;
        var hitPigment = hitBRDF.GetPigment();
        var surfacepoint = hR.SPoint;
        var hitColor = hitPigment.GetColor(surfacepoint);
        var emittedRad = hitMaterial.EmittedRadiance.GetColor(hR.SPoint);
        var hitColorLum = Math.Max(Math.Max(hitColor.r, hitColor.g), hitColor.b);
        
        
        //execution
        if (ray.Depth >= _iterLimit)
        {
            if (_pcg.RandomFloat() > hitColorLum) 
                hitColor = (1.0f / (1 - hitColorLum)) * hitColor;
            else
                return emittedRad;
        }
        
        var cumRad = new Color();
                
        if (hitColorLum > 0) 
            for (int i = 0; i < _n; i++)
            {
                var newRay =
                    hitMaterial.brdf.ScatterRay(_pcg, hR.Ray.Direction, hR.WPoint, hR.N,hR.Ray.Depth + 1); //depth? giusto
                var newRad = Roulette(newRay);
                cumRad = cumRad + newRad * hitColor;
            }
        
        return emittedRad + (1.0f / _n) * cumRad;
    }
    
}

public struct test
{
    public static void FurnaceTest()
    {
        var pcg = new PCG();

        for (int i = 0; i < 5; i++)
        {
            float reflectance = pcg.RandomFloat();
            float emittedRad = 0.9f * pcg.RandomFloat();
        
            var world = new World();
            var brdf = new DiffusedBRDF();
            brdf.P = new UniformPigment(reflectance * new Color(1,1,1));
            var matRad = new UniformPigment(emittedRad * new Color(1, 1, 1));

            var encMat = new Material(matRad, brdf);
            var s = new Sphere(material: encMat);
            world.Add(s);

            var ptracer = new PathTracer(world, pcg, 1, 100, 101, new Color());
            var ray = new Ray(origin: new Point(), direction: new Vector(1, 0, 0));
            var color = ptracer.Roulette(ray);

            var expected = emittedRad / (1 - reflectance);
            Debug.Assert(IsClose(expected, color.r));
            Debug.Assert(IsClose(expected, color.g));
            Debug.Assert(IsClose(expected, color.b));
        }
    }
}


