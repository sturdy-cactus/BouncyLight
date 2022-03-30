using System.Diagnostics;
using static mialibreria.check;

namespace geometry;

struct Point
{
    public float x, y, z;

    public Point()
    {
        x = 0;
        y = 0;
        z = 0;
    }
    public Point(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public string toString()
    {
        string s = $"(x = {x.ToString()}, y = {y.ToString()}, z = {z.ToString()})";
        return s;
    }

    public bool isClose(Point p)
    {
        return (IsClose(x,p.x) && IsClose(y,p.y) && IsClose(z,p.z));
        Point a = new Point(1, 2, 3);
        Point b = new Point(2, 3, 4);
        Debug.Assert(a.isClose(b));
    }

    public Point Sum(Vector v)
    {
        x += v.x;
        y += v.y;
        z += v.z;
        return this;
    }

    public Vector Diff(Point p)
    {
        Vector v = new Vector();
        v.x = x - p.x;
        v.y = y - p.y;
        v.z = z - p.z;
        
        return v;
    }

    public Vector toVector()
    {
        Vector v = new Vector();
        v.x = x;
        v.y = y;
        v.z = z;

        return v;
    }
}