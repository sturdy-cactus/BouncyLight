using Geometry;
using PFMlib;
using Cameras;
using Shapes;
using static Geometry.Transformation;

var scala = Scaling(new Vector(.1f, .1f, .1f));
World mioMondo = new World();


var ctrasf = Translation(new Vector(-1, 0, 0));
var cam = new PerspCamera(t:ctrasf);

var p = Translation(new Vector(-.5f, -.5f, -.5f));
mioMondo.Add(new Sphere(scala*p));

p=Translation(new Vector(.5f,-.5f,-.5f));
mioMondo.Add(new Sphere(scala*p));

p=Translation(new Vector(-.5f,.5f,-.5f));
mioMondo.Add(new Sphere(scala*p));

p=Translation(new Vector(-.5f,-.5f,.5f));
mioMondo.Add(new Sphere(scala*p));

p=Translation(new Vector(.5f,.5f,.5f));
mioMondo.Add(new Sphere(scala*p));

p=Translation(new Vector(.5f,.5f,-.5f));
mioMondo.Add(new Sphere(scala*p));

p=Translation(new Vector(.5f,-.5f,.5f));
mioMondo.Add(new Sphere(scala*p));

p=Translation(new Vector(-.5f,.5f,.5f));
mioMondo.Add(new Sphere(scala*p));

//sfere di controllo
p=Translation(new Vector(0,0,-.5f));
mioMondo.Add(new Sphere(scala*p));

p=Translation(new Vector(0,.5f,0));
mioMondo.Add(new Sphere(scala*p));

