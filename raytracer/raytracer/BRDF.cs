using Geometry;
using PFMlib;

namespace BRDF;

public interface IBRDF
{
 public Color Eval(Normal n, Vector inw, Vector outw, Vector2D uv);
}

public class DiffusedBRDF : IBRDF
{
 //MEMBERS
 public IPigment P;
 public float Reflectance;
 
 //METHODS
 public Color Eval(Normal n, Vector inw, Vector outw, Vector2D uv)
 {
  return this.Reflectance * this.P.GetColor(uv) / (float)Math.PI;
 }
}
