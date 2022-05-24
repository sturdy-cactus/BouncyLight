﻿using Cameras;
using Geometry;
using PFMlib;

namespace BRDF;

public interface IBRDF
{
 public Color Eval(Normal n, Vector inw, Vector outw, Vector2D uv);
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