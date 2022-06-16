using Cameras;
using Geometry;
using OrthoNormalBasis;
using PFMlib;
using RandomNumber;

namespace BRDF;

public interface IBRDF
{
    public bool IsDiffused();
    public Color Eval(Normal n, Vector inw, Vector outw, Vector2D uv);
    public Ray ScatterRay(PCG pcg, Vector IncDirection, Point IntPoint, Normal normal, int depth);
    public IPigment GetPigment();
}

public interface IPigment
{
    public Color GetColor(Vector2D vec);
}

public class DiffusedBRDF : IBRDF
{
    //MEMBERS
    public IPigment P;

    public DiffusedBRDF()
    {
        this.P = new UniformPigment(new Color(255,255,255));
    }
    
    public DiffusedBRDF(IPigment pigment)
    {
        this.P = pigment;
    }

    //METHODS
    public Color Eval(Normal n, Vector inw, Vector outw, Vector2D uv)
    {
        return (1 / (float) Math.PI) * this.P.GetColor(uv);
    }

    public bool IsDiffused()
    {
        return true;
    }
    
    public Ray ScatterRay(PCG pcg, Vector IncDirection, Point IntPoint, Normal normal, int depth)
    {
        var basis = new ONB(normal);
        var cosThetaSq = pcg.RandomFloat();
        var cosTheta = (float)Math.Sqrt(cosThetaSq);
        var sinTheta = (float)Math.Sqrt(1 - cosThetaSq);
        var phi = 2 * Math.PI * pcg.RandomFloat();
        var dir = (float)(Math.Cos(phi) * cosTheta)*basis.e1 + (float)(Math.Sin(phi) * cosTheta)*basis.e2 + sinTheta*basis.e3;
        return new Ray(origin: IntPoint, direction: dir, tMin: 1e-3f, tMax: float.PositiveInfinity, depth: depth);
    }

    public IPigment GetPigment()
    {
        return P;
    }
}

public class SpecularBRDF : IBRDF
{
    //MEMBERS
    public IPigment P;
    public float ThresholdAngleRad;

    public SpecularBRDF(IPigment p, float thresholdAngleRad)
    {
        P = p;
        ThresholdAngleRad = thresholdAngleRad;
    }
    
    //METHODS
    public Color Eval(Normal n, Vector inw, Vector outw, Vector2D uv)
    {
        throw new NotImplementedException();
    }

    public bool IsDiffused()
    {
        return false;
    }
    public Ray ScatterRay(PCG pcg, Vector IncDirection, Point IntPoint, Normal normal, int depth)
    {
        var direction = IncDirection;
        direction.Normalize();
        var norm = normal.ToVector();
        norm.Normalize();
        var scalar = norm * direction;

        return new Ray(origin:IntPoint,direction:direction-2*scalar*norm,tMin:1e-5f,tMax:float.PositiveInfinity,depth:depth);
    }
    
    public IPigment GetPigment()
    {
        return P;
    }
}

public class UniformPigment : IPigment
{
    //MEMBERS
    public Color color;
    
    //CTOR
    public UniformPigment(Color color)
    {
        this.color = color;
    }
    
    public UniformPigment()
    {
        this.color = new Color();
    }
        
    //METHODS
    public Color GetColor(Vector2D vec)
    {
        return this.color;
    }
}

public class CheckeredPigment : IPigment
{
    public Color color1;
    public Color color2;
    public int steps;

    public CheckeredPigment(Color color1, Color color2, int steps = 10)
    {
        this.color1 = color1;
        this.color2 = color2;
        this.steps = steps;
    }
    
    public Color GetColor(Vector2D vec)
    {
        int u=(int)Math.Floor(vec.GetU()*steps);
        int v=(int)Math.Floor(vec.GetV()*steps);

        if (u % 2 == v % 2)
            return color1;
        return color2;
    }
}

public class ImagePigment : IPigment
{
    public HdrImage img;
    
    public ImagePigment(HdrImage img)
    {
        this.img = img;
    }
    
    public Color GetColor(Vector2D vec)
    {
        var col = (int)(vec.GetU() * img.w);
        var row = (int)(vec.GetV() * img.h);

        if (col >= img.w)
            col = img.w - 1;
        
        if (row >= img.h)
            row = img.h - 1;

        return img.GetPixel(row, col);
    }
}

public struct Material
{
    //MEMBERS
    public IBRDF brdf;
    public IPigment EmittedRadiance;

    //CTOR
    public Material(IPigment EmittedRadiance, IBRDF brdf)
    {
        this.brdf = brdf;
        this.EmittedRadiance = EmittedRadiance;
    }

    public Material(IBRDF brdf)
    {
        this.brdf = brdf;
        this.EmittedRadiance = new UniformPigment(new Color(0,0,0));
    }
    
    public Material()
    {
        this.brdf = new DiffusedBRDF();
        this.EmittedRadiance = new UniformPigment(new Color(0, 0, 0));
    }
}