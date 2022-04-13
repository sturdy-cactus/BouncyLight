using geometry;
using Transformation = geometry.Transformation;
using ray;

interface Camera
{
    Ray fireRay(float u, float v);
}

struct PerspectiveCamera: Camera
{
    private float aspectRatio;
    private Transformation transformation;

    public PerspectiveCamera()
    {
        aspectRatio = 1;
        transformation = new Transformation();
    }
    
    public Ray fireRay(float u, float v)
    {
        throw new NotImplementedException();
    }
}

struct OrthogonalCamera : Camera
{
    private float aspectRatio;
    private Transformation transformation;

    public OrthogonalCamera(float? aspectRatio=null, Transformation? transformation=null)
    {
        this.aspectRatio = aspectRatio ?? 1.0f;
        this.transformation = transformation ?? new Transformation();
    }
    
    public Ray fireRay(float u, float v)
    {
        var origin = new Point(-1, (1.0f - 2 * u) * aspectRatio, 2 * v - 1);
        var direction = new Vector(1, 0, 0);
        return transformation * new Ray(origin: origin, direction: direction, tmin: 1);
    }
}