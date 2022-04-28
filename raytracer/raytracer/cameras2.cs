using geometry;
using Transformation = geometry.Transformation;
using ray;
using test;

namespace cam;

interface Camera
{
    Ray fireRay(float u, float v);
}

struct PerspectiveCamera: Camera
{
    private float distance, aspectRatio;
    private Transformation transformation;

    public PerspectiveCamera(float? distance=null, float? aspectRatio=null, Transformation? transformation=null)
    {
        this.distance = distance ?? 1.0f;
        this.aspectRatio = aspectRatio ?? 1.0f;
        this.transformation = transformation ?? new Transformation();
    }
    
    public Ray fireRay(float u, float v)
    {
        var origin = new Point(-distance, 0, 0);
        var direction = new Vector(distance, (1.0f - 2 * u) * aspectRatio, 2 * v - 1);
        return transformation * new Ray(origin: origin, direction: direction, tMin: 1);    }
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
        var ray = new Ray(origin, direction);
        return transformation * ray;
        // new Ray(origin: origin, direction: direction, tMin: 1);
    }
}