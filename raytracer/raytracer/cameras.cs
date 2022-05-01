using System;
using System.Runtime.InteropServices;
using Geometry;
using static test.TestGeometry;
using PFMlib;
using Point = Geometry.Point;

namespace Cameras;

interface ICamera
{
    public Ray FireRay(float u, float v);
}

class OrthCamera : ICamera
{
    //MEMBRI
    private float a;
    private Transformation t;
    
    //COSTRUTTORE
    public OrthCamera(float? a = null, Transformation? t = null)
    {
        this.a = a ?? 1.0f;
        this.t = t ?? new Transformation();
    }
    
    //METODI
    public Ray FireRay(float u, float v)
    {
        var o = new Point(-1.0f, -(2.0f * u - 1.0f) * this.a, 2.0f * v - 1.0f);
        var dir = new Vector(1.0f, .0f, .0f);
        var ray = new Ray(o, dir, tMin: 1.0f);

        return this.t * ray;
    }
}

class PerspCamera : ICamera
{
    //MEMBRI
    private float a;
    private float d;
    private Transformation t;

    //COSTRUTTORE
    public PerspCamera(float? a = null, float? d = null, Transformation? t = null)
    {
        this.a= a ?? 1.0f;
        this.d = d ?? 1.0f;
        this.t = t ?? new Transformation();
    }
    public Ray FireRay(float u, float v)
    {
        var o = new Point(-this.d, .0f, .0f);
        var dir = new Vector(this.d, -(2.0f * u - 1.0f) * this.a, 2.0f * v - 1.0f);
        var ray = new Ray(o, dir, tMin: dir.SqNorm());

        return this.t * ray;
    }
}

class ImgTracer
{
    //MEMBRI
    public HdrImage img;
    public ICamera cam;
    
    //COSTRUTTORE
    public ImgTracer(HdrImage img, ICamera cam)
    {
        this.img = img;
        this.cam = cam;
    }
    
    //METODI
    public Ray FireRay(int a, int b, float uPix = .5f, float vPix = .5f)
    {
        float u = (a + uPix) / (this.img.w - 1); //forse l'errore e' il -1
        float v = (b + uPix) / (this.img.h - 1); 

        return cam.FireRay(u, v);
    }

    public void FireAllRays()
    {
        var ray = new Ray();
        var color = new Color();
        for (int i = 0; i < this.img.h; i++)
            for(int j = 0; j < this.img.w; j++)
            {
                ray = this.FireRay(i, j);
                //qui colore come funzione del raggio
                this.img.SetPixel(color, i, j);
            }
    }
}

struct Ray
{
    //MEMBRI
    public Point origin;
    public Vector direction;
    public float tMin;
    public float tMax;
    public int depth;
    
    //COSTRUTTORE
    public Ray(Point origin, Vector direction, float? tMin = null, float? tMax = null, int? depth = null)
    {
        this.origin = origin;
        this.direction = direction;
        this.tMin = tMin ?? 1e-5f;
        this.tMax = tMax ?? float.PositiveInfinity;
        this.depth = depth ?? 0;
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
    
    public static Ray operator *(Transformation t, Ray r)
    {
        Ray temp = r;
        temp.direction = t * r.direction;
        temp.origin = t * r.origin;
        return temp;
    }
}

