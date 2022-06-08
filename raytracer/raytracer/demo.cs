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
                    mioMondo.Add(new Sphere(p * scala,new Material(arancione,new DiffusedBRDF())));
                }
            }
        }

        //check spheres
        var q = Translation(new Vector(0, 0, -.5f));
        var scacchi1 = new CheckeredPigment(new Color(79, 121, 66), new Color(255, 255, 0));
        mioMondo.Add(new Sphere(q * scala, new Material(scacchi1,new DiffusedBRDF())));

        q = Translation(new Vector(0, .5f, 0));
        var scacchi2 = new CheckeredPigment(new Color(132, 60, 20), new Color(120, 12, 255),2);
        mioMondo.Add(new Sphere(q * scala, new Material(scacchi2, new DiffusedBRDF())));


        mioMondo.Add(new Plane(tr: Rotation((float)(Math.PI),'x')*Translation(new Vector(0,0,1)),mat: new Material(scacchi1, new DiffusedBRDF())));
        
        var myimg = new HdrImage(width, height);
        var myTracer = new ImgTracer(myimg, cam);

        var world = new World();
        world.Add(new Sphere(Scaling(new Vector(.1f,.1f,.1f)), new Material(new UniformPigment(new Color(1000000,1000000,1000000)),brdf:new DiffusedBRDF())));
        //world.Add(new Plane(Translation(new Vector(0, 0, -.7f)),
        //    new Material(new SpecularBRDF(new UniformPigment(new Color(100,100,100)),1))));
        world.Add(new Sphere(Translation(new Vector(-1,.5f,0))*Scaling(new Vector(0.2f,0.2f,0.2f)),new Material(new DiffusedBRDF(new CheckeredPigment(new Color(100,50,300),new Color(200,200,10),10)))));
        world.Add(new Plane(Translation(new Vector(0, 0, -1)),
            new Material(
                new DiffusedBRDF(new CheckeredPigment(new Color(100, 200, 50), new Color(200, 40, 90), 3)))));
        world.Add(new Plane(Translation(new Vector(0,2,0))*Rotation((float)Math.PI/2f,'x'),new Material(new DiffusedBRDF(new UniformPigment(new Color(300,200,300))))));
        world.Add(new Plane(Translation(new Vector(0,-2,0))*Rotation((float)Math.PI/2f,'x'),new Material(new DiffusedBRDF(new UniformPigment(new Color(300,200,300))))));
        world.Add(new Plane(Translation(new Vector(0,0,1.5f)),new Material(new DiffusedBRDF(new UniformPigment(new Color(300,100,100))))));
        world.Add(new Plane(Translation(new Vector(3,0,0))*Rotation((float)Math.PI/2f,'y'),new Material(new DiffusedBRDF(new UniformPigment(new Color(100,200,300))))));

        
        
        //myTracer.FlatRenderer(world);
        myTracer.PathRenderer(world, new PCG(60,200), 100, 2, 4, new Color());

        myimg.SaveLdrImg(outfile);

        //mioMondo = new World();
        //myTracer.OnAndOffRenderer(mioMondo,background:new Color(1,2,3));
        //myimg.SaveLdrImg("testvuoto.png");
    }
}