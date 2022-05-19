using Geometry;
using PFMlib;
using Cameras;
using Shapes;
using static Geometry.Transformation;

namespace demo;

public struct demo
{
    public static void test(int width, int height, float angle, string outfile)
    {
        var scala = Scaling(new Vector(.1f, .1f, .1f));
        World mioMondo = new World();
        float ar = (float) width / height;
        
        var ctrasf = Rotation(angle * (float) Math.PI/180.0f, 'z') * Translation(new Vector(-1, 0, 0));
        var cam = new PerspCamera(a: ar, t: ctrasf);

        for (var i = -.5f; i < 1; i++)
        {
            for (var j = -.5f; j < 1; j++)
            {
                for (var k = -.5f; k < 1; k++)
                {
                    var p = Translation(new Vector(i, j, k));
                    mioMondo.Add(new Sphere(p * scala));
                }
            }
        }

        //check spheres
        var q = Translation(new Vector(0, 0, -.5f));
        mioMondo.Add(new Sphere(q * scala));

        q = Translation(new Vector(0, .5f, 0));
        mioMondo.Add(new Sphere(q * scala));
        
        var myimg = new HdrImage(width, height);
        var myTracer = new ImgTracer(myimg, cam);

        myTracer.FireAllRays(mioMondo);

        myimg.SaveLdrImg(outfile);
        
    }
}