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
                    var hitr = hr.Value;
                    color = hitr.Mat.EmittedRadiance.GetColor(hitr.SPoint)+hitr.Mat.brdf.GetPigment().GetColor(hitr.SPoint);
                }

                this.img.SetPixel(color, row, col);
            }
        }
    }
    
    public void PathRenderer(World world, PCG pcg, int N, int maxDepth, int iterLimit, Color background)
    {
        var tracer = new PathTracer(world, pcg, N, maxDepth, iterLimit, background);
        var progress = (float)img.w / 40f;
        var halfprog = progress / 2;
        Console.WriteLine("\ngenerazione dell'immagine in corso...\n________________________________________");
        var colnum = 0;
        float lastrest = 0;
        float rest;
        float a;
        /*Parallel.For(0, img.w, col =>
        {
            /*colnum++;
            a = (float)colnum / progress;
            rest = a - (float)Math.Floor(a);
            if (lastrest > halfprog && rest < halfprog)
                Console.Write("*");
            lastrest = rest;
            
            for (var row = 0; row < img.h; row++)
            {
                ray = FRay(col, row);
                color = tracer.Trace(ray);
                img.SetPixel(color, row, col);
            }
        });*/
        var counter = 0;
        var tot = img.w * img.h;
        Parallel.For(0, img.w, col =>
        {
            /*colnum++;
            if (colnum % progress == 0)
                Console.Write('*');*/
            Parallel.For(0, img.h, row =>
            {
                if (++counter % 100 == 0)
                    Console.WriteLine($"calculating pixel {counter} of {tot}");

                var ray = FRay(col, row);
                var color = tracer.Trace(ray);
                img.SetPixel(color, row, col);
            });
        });


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
    private int _rayNumber;
    private int _maxDepth;
    private int _rouletteLimit;
    private Color _background;

    //CTOR
    public PathTracer(World world, PCG pcg, int numberOfRays, int maxDepth, int Limit, Color background)
    {
        _world = world;
        _pcg = pcg;
        _rayNumber = numberOfRays;
        _maxDepth = maxDepth;
        _rouletteLimit = Limit;
        _background = background;
    }

    public PathTracer(World world)
    {
        _world = world;
        _pcg = new PCG();
        _rayNumber = 10;
        _maxDepth = 10;
        _rouletteLimit = 3;
        _background = new Color(0, 0, 0);
    }

    //METHODS
    public Color Trace(Ray ray)
    {
        if (ray.Depth > _maxDepth)
            return new Color(0, 0, 0);

        HitRecord? record = _world.RIntersection(ray);

        if (record == null)
            return _background;

        var hitRecord = record.Value;
        var hitMaterial = hitRecord.Mat;
        var hitColor = hitMaterial.brdf.GetPigment().GetColor(hitRecord.SPoint);
        var emission = hitMaterial.EmittedRadiance.GetColor(hitRecord.SPoint);

        var hitLum = Math.Max(hitColor.r, Math.Max(hitColor.g, hitColor.b));

        //roulette here
        if (ray.Depth >= _rouletteLimit)
        {
            var q = (float)Math.Max(0.05, 1 - hitLum);
            if (_pcg.RandomFloat() > q)
                hitColor = (float)(1f / (1f - q)) * hitColor;
            else return emission;
        }

        var radianceSum = new Color(0, 0, 0);
        if (hitLum > 0f)
            for (int rayIndex = 0; rayIndex < _rayNumber; rayIndex++)
            {
                var newRay = hitMaterial.brdf.ScatterRay(_pcg, hitRecord.Ray.Direction, hitRecord.WPoint, hitRecord.N,
                    ray.Depth + 1);
                var newRadiance = Trace(newRay);

                radianceSum += newRadiance * hitColor;
            }

        return (1f / _rayNumber) * radianceSum + emission;

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
            var color = ptracer.Trace(ray);

            var expected = emittedRad / (1 - reflectance);
            Debug.Assert(IsClose(expected, color.r));
            Debug.Assert(IsClose(expected, color.g));
            Debug.Assert(IsClose(expected, color.b));
        }
    }
}


