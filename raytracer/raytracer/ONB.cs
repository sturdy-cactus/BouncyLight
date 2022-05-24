using Geometry;

namespace ONB;

struct ONB
{
    public Vector e1;
    public Vector e2;
    public Vector e3;

    public ONB(Vector normal)
    {
        var sign = -1;
        if (normal.z > 0)
            sign = 1;

        var a = -1 / (sign + normal.z);
        var b = normal.x * normal.y * a;

        e1 = new Vector(1 + sign * normal.x * normal.x * a, sign * b, -sign * normal.x);
        e2 = new Vector(b, sign + normal.y * normal.y * a, -normal.y);
        e3 = normal;
    }
    
    public ONB(Normal normal)
    {
        var sign = -1;
        if (normal.z > 0)
            sign = 1;

        var a = -1 / (sign + normal.z);
        var b = normal.x * normal.y * a;

        e1 = new Vector(1 + sign * normal.x * normal.x * a, sign * b, -sign * normal.x);
        e2 = new Vector(b, sign + normal.y * normal.y * a, -normal.y);
        e3 = new Vector(normal.x,normal.y,normal.z);
    }

}

struct test
{
    
}