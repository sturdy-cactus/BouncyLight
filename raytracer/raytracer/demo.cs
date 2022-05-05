using Geometry;
using PFMlib;
using Cameras;
using Shapes;
using static Geometry.Transformation;

namespace demo;

struct demo
{
    public static void test()
    {
        var scala = Scaling(new Vector(.1f, .1f, .1f));
        World mioMondo = new World();
        
        var ctrasf = Translation(new Vector(-1, 0, 0));
        var cam = new PerspCamera(t: ctrasf);

        var p = Translation(new Vector(-.5f, -.5f, -.5f));
        mioMondo.Add(new Shapes.Sphere(scala * p));

        p = Translation(new Vector(.5f, -.5f, -.5f));
        mioMondo.Add(new Sphere(scala * p));

        p = Translation(new Vector(-.5f, .5f, -.5f));
        mioMondo.Add(new Sphere(scala * p));

        p = Translation(new Vector(-.5f, -.5f, .5f));
        mioMondo.Add(new Sphere(scala * p));

        p = Translation(new Vector(.5f, .5f, .5f));
        mioMondo.Add(new Sphere(scala * p));

        p = Translation(new Vector(.5f, .5f, -.5f));
        mioMondo.Add(new Sphere(scala * p));

        p = Translation(new Vector(.5f, -.5f, .5f));
        mioMondo.Add(new Sphere(scala * p));

        p = Translation(new Vector(-.5f, .5f, .5f));
        mioMondo.Add(new Sphere(scala * p));

        //sfere di controllo
        p = Translation(new Vector(0, 0, -.5f));
        mioMondo.Add(new Sphere(scala * p));

        p = Translation(new Vector(0, .5f, 0));
        mioMondo.Add(new Sphere(scala * p));

        var myimg = new HdrImage(200, 200);
        var myTracer = new ImgTracer(myimg, cam);

        myTracer.FireAllRays(mioMondo);

        myimg.SaveLdrImg("sfere.jpg");
    }
}