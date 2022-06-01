using BRDF;
using Geometry;
using PFMlib;
using Cameras;
using RandomNumber;
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
        var arancione = new UniformPigment(new Color(255, 165, 0));
        
        for (var i = -.5f; i < 1; i++)
        {
            for (var j = -.5f; j < 1; j++)
            {
                for (var k = -.5f; k < 1; k++)
                {
                    var p = Translation(new Vector(i, j, k));
                    mioMondo.Add(new Sphere(p * scala,new Material(new DiffusedBRDF(arancione,0))));
                }
            }
        }

        //check spheres
        var q = Translation(new Vector(0, 0, -.5f));
        var scacchi1 = new CheckeredPigment(new Color(79, 121, 66), new Color(255, 255, 0));
        mioMondo.Add(new Sphere(q * scala, new Material(new UniformPigment(new Color(255,255,255)),new DiffusedBRDF(scacchi1,0))));

        q = Translation(new Vector(0, .5f, 0));
        var scacchi2 = new CheckeredPigment(new Color(132, 60, 20), new Color(120, 12, 255),2);
        mioMondo.Add(new Sphere(q * scala, new Material(new DiffusedBRDF(scacchi2,0))));


        mioMondo.Add(new Plane(tr: Rotation((float)(Math.PI),'x')*Translation(new Vector(0,0,1)),mat: new Material(scacchi1, new DiffusedBRDF())));
        
        var myimg = new HdrImage(width, height);
        var myTracer = new ImgTracer(myimg, cam);

        var world = new World();
        world.Add(new Sphere(Translation(new Vector(0,1,0)), new Material(new UniformPigment(new Color(100,200,300)),brdf:new DiffusedBRDF())));
        world.Add(new Sphere(Translation(new Vector(0,-1,0)), new Material(brdf:new DiffusedBRDF(new UniformPigment(new Color(123,43,98)),2))));

        myTracer.PathTracer(mioMondo, new PCG(), 1000000, 2, 4, new Color());
        myimg.SaveLdrImg(outfile,lum:20);

        //mioMondo = new World();
        //myTracer.OnAndOffRenderer(mioMondo,background:new Color(1,2,3));
        //myimg.SaveLdrImg("testvuoto.png");
        
        
    }
}