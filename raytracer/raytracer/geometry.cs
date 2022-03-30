namespace geometry;

//fiore

public struct Vector
{
    //MEMBRI
    public float x;
    public float y;
    public float z;

    //COSTRUTTORE DEFAULT
    public Vector()
    {
        x = .0f;
        y = .0f;
        z = .0f;
    }

    //COSTRUTTORE
    public Vector(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    //METODI
    public string ConvertToString()
    {
        string s = "({this.x}, {this.y}, {this.z})";
        return s;
    }

    public Vector Sum(Vector v1, Vector v2)
    {
        var sum = new Vector();
        sum.x = v1.x + v2.x;
        sum.y = v1.y + v2.y;
        sum.z = v1.z + v2.z;

        return sum;
    }

    public Vector Diff(Vector v1, Vector v2)
    {
        var diff = new Vector();
        diff.x = v1.x + v2.x;
        diff.y = v1.y + v2.y;
        diff.z = v1.z + v2.z;

        return diff;
    }
}