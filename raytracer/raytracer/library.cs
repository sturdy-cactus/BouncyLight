﻿using System.Diagnostics;
namespace mialibreria;

public struct check
{
    public static bool IsClose(float a, float b)
    {
        const float epsilon = 1e-4f; //fallisce a e-5!
        return Math.Abs(a - b) < epsilon;
    }
    public static void AssertColor()
    {
        Color c = new Color(1.0f, 2.1f, 3f);
        Color b = new Color(256.3f, 90.1f, 3.56f);
        Color d = c;
        c.AddColor(b);
        Console.WriteLine("{0} {1} {2}", c.r, c.g, c.b);
        Debug.Assert(check.IsClose(c.r, 257.3f));
        d.Cmult(b);
        Console.WriteLine("{0} {1}", d.g, 189.21f);
        Debug.Assert(check.IsClose(d.g, 189.21f));
        Console.WriteLine("il test ha avuto successo!");
    }
}
public struct Color
{
    public float r = 0, g = 0, b = 0;
    
    public Color(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }
    
    public Color Add(float a, float b, float c)
    {
        this.r += a;
        this.g += b;
        this.b += c;
        return this;
    }
    public Color AddColor(Color a)
    {
        this.r += a.r;
        this.g += a.g;
        this.b += a.b;
        return this;
    }

    public Color Smult(float a)
    {
        this.r *= a;
        this.g *= a;
        this.b *= a;
        return this;
    }
    
    public Color Cmult(Color a)
    {
        this.r *= a.r;
        this.g *= a.g;
        this.b *= a.b;
        return this;
    }

}

public class HdrImage
{
    public int w = 0, h = 0;
    public Color[] pixels;

    public HdrImage(int w, int h)
    {
        this.w = w;
        this.h = h;
        this.pixels = new Color[this.w * this.h];
    }
    
    public Color GetPixel(int a, int b)
    {
        int pos = b * this.w + a;
        return this.pixels[pos];
    }
    
    public void SetPixel(int a, int b, Color c)
    {
        int pos = b * this.w + a;
        this.pixels[pos] = c;
    }
    
}
