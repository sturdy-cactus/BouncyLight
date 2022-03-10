namespace mialibreria;

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
