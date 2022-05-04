﻿using System.Diagnostics;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static test.isClose;

using Geometry;
using static Geometry.Transformation;
using Cameras;
using Shapes;
using Vector = Geometry.Vector;

namespace test;

public class isClose
{
    public static bool IsClose(float a, float b)
    {
        const float epsilon = 1e-4f; //fallisce a e-5!
        return Math.Abs(a - b) < epsilon;
    }
}

[TestClass]
public class TestGeometry
{
    [TestMethod]
    public static void TestVec()
    {
        var a = new Vector(1.0f, 2.0f, 3.0f);
        var b = new Vector(4.0f, 6.0f, 8.0f);
        
        Debug.Assert(a.isClose(a));
        Debug.Assert(!a.isClose(b));
    }
    
    [TestMethod]
    public static void TestVecOps()
    {
        var a = new Vector(1.0f, 2.0f, 3.0f);
        var b = new Vector(4.0f, 6.0f, 8.0f);

        Debug.Assert((a + b).isClose(new Vector(5.0f, 8.0f, 11.0f)));
        Debug.Assert((b-a).isClose(new Vector(3.0f, 4.0f, 5.0f)));
        Debug.Assert((2*a).isClose(new Vector(2.0f, 4.0f, 6.0f)));
        Debug.Assert(IsClose(40.0f, a*b));
        Debug.Assert(IsClose(14.0f, a.SqNorm()));
        Debug.Assert(IsClose(14.0f, (float)Math.Pow(a.Norm(), 2.0)));
        Debug.Assert((Vector.CrossProd(a,b)).isClose(new Vector(-2.0f, 4.0f, -2.0f)));
        Debug.Assert((Vector.CrossProd(a,b)).isClose(new Vector(2.0f, -4.0f, 2.0f)));
    }
    
    [TestMethod]
    public static void TestPoint()
    {
        Point a = new Point(1, 2, 3);
        Point b = new Point(0.5f * 2, 1.0f * 2.0f,3.0f/1.0f);
        Point c = new Point(3.0f, 1.0f, 9000.3f);
        Debug.Assert(a.isClose(a));
        Debug.Assert(a.isClose(b));
        Debug.Assert(!a.isClose(c));
    }

    [TestMethod]
    public static void TestPointOps()
    {
        Point p1 = new Point(1, 2, 3);
        Vector v2 = new Vector(4.0f, 6.0f, 8.0f);
        Point p2 = new Point(12, -3, 0);
        Debug.Assert((p1 * 2).isClose(new Point(2, 4, 6)));
        Debug.Assert((p1 + v2).isClose(new Point(5, 8, 11)));
        Debug.Assert((p1 - p2).isClose(new Vector(-11, 5, 3)));
        Debug.Assert((p1 + (p1 - p2)).isClose(new Point(-10, 7, 6)));
    }

    [TestMethod]
    public static void TestTransfOps()
    {
        var a = Matrix4x4.Identity;
        var b = new Matrix4x4(1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1);
        Debug.Assert(IsClose(a, b));
    }
    
    public static bool IsClose(float a, float b)
    {
        const float epsilon = 1e-4f; //fallisce a e-5!
        return Math.Abs(a - b) < epsilon;
    }
    
    [TestMethod]
    public static bool IsClose(Matrix4x4 A, Matrix4x4 B)
    {
        bool mybool = true;
        for (int i = 1; i <= 4; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                float a = (float)A.GetType().GetField("M"+i+j).GetValue(A);
                float b = (float)B.GetType().GetField("M"+i+j).GetValue(B);
                mybool = mybool && IsClose(a, b);
            }
        }
        return mybool;
    }
}

[TestClass]
public class TestCamera
{
    
    [TestMethod]
    public static void TestOrthCamera()
    {
        var cam = new OrthCamera(2);
        var Ray1 = cam.FireRay(0, 0);
        var Ray2 = cam.FireRay(1, 0);
        var Ray3 = cam.FireRay(0, 1);
        var Ray4 = cam.FireRay(1, 1);
        
        Debug.Assert(IsClose(0,Vector.CrossProd(Ray1.direction, Ray2.direction).SqNorm()));
        Debug.Assert(IsClose(0,Vector.CrossProd(Ray1.direction, Ray3.direction).SqNorm()));
        Debug.Assert(IsClose(0,Vector.CrossProd(Ray1.direction, Ray4.direction).SqNorm()));

        Debug.Assert(Ray1.At(1).isClose(new Point(0, 2, -1)));
        Debug.Assert(Ray2.At(1).isClose(new Point(0, -2, -1)));
        Debug.Assert(Ray3.At(1).isClose(new Point(0, 2, 1)));
        Debug.Assert(Ray4.At(1).isClose(new Point(0, -2, 1)));
    }

    [TestMethod]
    public static void TestOrthCameraTransformation()
    {
        var vec = new Vector(.0f, -2.0f, .0f);
        var cam = new OrthCamera(t: Transformation.Translation(vec) * Transformation.Rotation(-(float)Math.PI/2, 'z'));
        var ray = cam.FireRay(.5f, .5f);
        
        Debug.Assert(ray.At(1.0f).isClose(new Point(0, -2, 0)));
    }

    [TestMethod]
    public static void TestPerspCamera()
    {
        var cam = new PerspCamera(d: 1, a: 2);
        
        var Ray1 = cam.FireRay(0, 0);
        var Ray2 = cam.FireRay(1, 0);
        var Ray3 = cam.FireRay(0, 1);
        var Ray4 = cam.FireRay(1, 1);
        
        Debug.Assert(Ray1.origin.isClose(Ray2.origin));
        Debug.Assert(Ray1.origin.isClose(Ray3.origin));
        Debug.Assert(Ray1.origin.isClose(Ray4.origin));
        
        Debug.Assert(Ray1.At(1).isClose(new Point(0, 2, -1)));
        Debug.Assert(Ray2.At(1).isClose(new Point(0, -2, -1)));
        Debug.Assert(Ray3.At(1).isClose(new Point(0, 2, 1)));
        Debug.Assert(Ray4.At(1).isClose(new Point(0, -2, 1)));
    }
    
}

[TestClass]
public class TestRay
{
    [TestMethod]
    public static void TestRayClose()
    {
        var a = new Ray(new Point(1.0f, 2.0f, 3.0f), new Vector(5.0f, 4.0f, -1.0f));
        var b = new Ray(new Point(5.0f, 1.0f, 4.0f), new Vector(3.0f, 9.0f, 4.0f));
        
        Debug.Assert(a.isClose(a));
        Debug.Assert(!a.isClose(b));
    }

    [TestMethod]
    public static void TestAt()
    {
        var a = new Ray(new Point(1.0f, 2.0f, 4.0f), new Vector(4.0f, 2.0f, 1.0f));
        Debug.Assert(a.At(.0f).isClose(a.origin));
        Debug.Assert(a.At(1.0f).isClose(new Point(5.0f, 4.0f, 5.0f)));
        //corretto
        Debug.Assert(a.At(2.0f).isClose(new Point(9.0f, 6.0f, 6.0f)));
    }
}

[TestClass]
public class TestSphere
{
    [TestMethod]
    public static void TestIntersect()
    {
        //#1
        var vec = new Vector(.0f, .0f, 1);
        var tr = Translation(vec) * Rotation(-.5f * (float) Math.PI, 'y');
        var cam = new OrthCamera(t: tr);
        var r = cam.FireRay(.5f, .5f);
        
        var s = new Sphere();
        var hr = s.RayIntersection(r);
        var p = hr.Value;

        Debug.Assert(p.WPoint.isClose(new Point(.0f, .0f, 1.0f)));
        Debug.Assert(IsClose(p.N * new Normal(.0f, .0f, 1.0f), p.N.Norm()));
        
        //#2
        tr = Translation(new Vector(2, 0, 0)) * Rotation((float)Math.PI, 'y');
        cam.SetCamera(tr);
        r = cam.FireRay(.5f, .5f);

        hr = s.RayIntersection(r);
        p = hr.Value;
        
        Debug.Assert(p.WPoint.isClose(new Point(1.0f, .0f, .0f)));
        Debug.Assert(IsClose(p.N * new Normal(1.0f, .0f, .0f), p.N.Norm()));
        
        //#3
        tr = Translation(new Vector(1.0f, .0f, .0f));
        cam.SetCamera(tr);
        r = cam.FireRay(.5f, .5f);
        
        hr = s.RayIntersection(r);
        p = hr.Value;
        
        Debug.Assert(p.WPoint.isClose(new Point(1.0f, .0f, .0f)));
        Debug.Assert(IsClose(p.N * new Normal(-1.0f, .0f, .0f), p.N.Norm()));
    }

    [TestMethod]
    public static void TestTransf()
    {
        var tr1 = Translation(new Vector(10.0f, .0f, 1.0f)) * Rotation(-.5f * (float) Math.PI, 'y');
        var cam = new OrthCamera(t: tr1);
        var r1 = cam.FireRay(.5f, .5f);
        Console.WriteLine(r1.direction.ConvertVecToString());
        Console.WriteLine(r1.origin.ConvertPointToString());
        
        var tr2 = Translation(new Vector(12.0f, .0f, 2.0f)) * Rotation((float) Math.PI, 'z');
        cam.SetCamera(tr2);
        var r2 = cam.FireRay(.5f, .5f);
        Console.WriteLine(r2.direction.ConvertVecToString());
        Console.WriteLine(r2.origin.ConvertPointToString());
        
        var tr3 = Translation(new Vector(10.0f, .0f, .0f));
        var s = new Sphere(tr3);
        var hr1 = s.RayIntersection(r1);
        var hr2 = s.RayIntersection(r2);
        
        if (hr1 != null)
        {
            var p1 = hr1.Value;
            Console.WriteLine("\nintersection 1:");
            Console.WriteLine(p1.WPoint.ConvertPointToString());
            Console.WriteLine(p1.N.ConvertNormToString());
        }

        if(hr2 != null)
        {
            var p2 = hr2.Value;
            Console.WriteLine("\nintersection 2:");
            Console.WriteLine(p2.WPoint.ConvertPointToString());
            Console.WriteLine(p2.N.ConvertNormToString());
        }
        

        //var p2 = hr2.Value;
        //Debug.Assert(p2.WPoint.isClose(new Point(11, 0 , 0)));

    }
}
