using System.Diagnostics;
using geometry;
using PFMlib;
namespace test;

public class Test
{
    public static void Point()
    {
        Point a = new Point(1, 2, 3);
        Point b = new Point(0.5f * 2, 1.0f * 2.0f,3.0f/1.0f);
        Point c = new Point(3.0f, 1.0f, 9000.3f);
        Debug.Assert(a.isClose(a));
        Debug.Assert(a.isClose(b));
        Debug.Assert(!a.isClose(c));
    }

    public static void Point_ops()
    {
        Point p1 = new Point(1, 2, 3);
        Point p2 = new Point(4.0f, 6.0f, 8.0f);
        
    }
    
    
}