using Cameras;
using Geometry;
using OrthoNormalBasis;
using PFMlib;
using RandomNumber;

namespace BRDF;

public interface IBRDF
{
 public Color Eval(Normal n, Vector inw, Vector outw, Vector2D uv);
 public Ray ScatterRay(PCG pcg, Vector IncDirection, Point IntPoint, Normal normal, int depth);
}

public interface IPigment
{
    public Color GetColor(Vector2D vec);
}


public class DiffusedBRDF : IBRDF
{
    //MEMBERS
    public IPigment P;
    public float Reflectance;

    //METHODS
    public Color Eval(Normal n, Vector inw, Vector outw, Vector2D uv)
    {
        return (this.Reflectance / (float) Math.PI) * this.P.GetColor(uv);
    }

    public Ray ScatterRay(PCG pcg, Vector IncDirection, Point IntPoint, Normal normal, int depth)
    {
        var basis = new ONB(normal);
        var cosThetaSq = pcg.randomFloat();
        var cosTheta = (float)Math.Sqrt(cosThetaSq);
        var sinTheta = (float)Math.Sqrt(1 - cosThetaSq);
        var phi = 2 * Math.PI * pcg.randomFloat();

        var dir = (float)(Math.Cos(phi) * cosTheta)*basis.e1 + (float)(Math.Sin(phi) * cosTheta)*basis.e2 + sinTheta*basis.e3;

        return new Ray(origin: IntPoint, direction: dir, tMin: 1e-3f, tMax: float.PositiveInfinity, depth: depth);
    }
}

public class SpecularBRDF : IBRDF
{
    //MEMBERS
    public IPigment P;
    public float ThresholdAngleRad;

    //METHODS


    public Color Eval(Normal n, Vector inw, Vector outw, Vector2D uv)
    {
        throw new NotImplementedException();
    }

    public Ray ScatterRay(PCG pcg, Vector IncDirection, Point IntPoint, Normal normal, int depth)
    {
        var direction = IncDirection;
        direction.Normalize();
        var norm = normal.ToVector.Normalize();
        var scalar = norm * direction;
    }
}

public class UniformPigment : IPigment
{
    public Color color;
    
    public UniformPigment(Color color)
    {
        this.color = color;
    }
        
    public Color GetColor(Vector2D vec)
    {
        return color;
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
    public IBRDF brdf = new DiffusedBRDF();
    public IPigment emitted_radiance = new UniformPigment(new Color(0, 0, 0));

    public Material(IPigment emittedRadiance, IBRDF? brdf = null)
    {
        this.brdf = brdf ?? new DiffusedBRDF();
        this.emitted_radiance = emittedRadiance;
    }
}