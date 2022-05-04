using Geometry;
using PFMlib;
using Cameras;
using Shapes;

var scala = Transformation.Scaling(new Vector(.1f, .1f, .1f));
var sfera = new Sphere(scala);
World mioMondo = new World();


mioMondo.Add(sfera);