using System.Globalization;
using System.Numerics;
using PFMlib;
using test;

internal static partial class Program
{
    private static void Main(string[] args)
    {
        {//test
            var a = Matrix4x4.Identity;
            var b = Matrix4x4.Identity;
            TestGeometry.IsClose(a, b);

            TestGeometry.TestPoint();
            TestGeometry.TestVec();
            TestGeometry.TestPointOps();
            TestGeometry.TestTransfOps();
            
            testCamera.testOrthogonalCamera();
            testCamera.testPerspectiveCamera();
            //fallisce!
            testCamera.testOrthogonalCameraTransformation();
            
            TestRay.TestRayClose();
            TestRay.TestAt();
        }

        Parameters myParams = new Parameters(args);

        HdrImage myImage = new HdrImage(path:myParams.input_file_name);
        
        HdrImage converted = myImage.TosRGB(factor: myParams.factor, gamma: myParams.gamma);
        converted.SaveLdrImg(myParams.output_file_name);
    }
    
    public class Parameters
    {
        public string input_file_name = "";
        public float factor = 0.2f;
        public float gamma = 1;
        public string output_file_name = "";

        public Parameters(string[] args)
        {
            try
            {
                input_file_name = args[0];
                factor = float.Parse(args[1],CultureInfo.InvariantCulture);//altrimenti pensa che debba usare la virgola!
                gamma = float.Parse(args[2],CultureInfo.InvariantCulture);
                output_file_name = args[3];
            }
            catch (Exception)
            {
                Console.WriteLine("usage: raytracer.exe \"input_file_name\" factor gamma \"output_file_name\"");
                Environment.Exit(1);
            }
        }
    }
}