using System.Globalization;
using System.Numerics;
using PFMlib;
using test;

internal static partial class Program
{
    private static void Main(string[] args)
    {
        {
            //test
            var a = Matrix4x4.Identity;
            var b = Matrix4x4.Identity;
            TestGeometry.IsClose(a, b);

            TestGeometry.TestPoint();
            TestGeometry.TestVec();
            TestGeometry.TestPointOps();
            TestGeometry.TestTransfOps();

            TestCamera.TestOrthCamera();
            TestCamera.TestPerspCamera();
            //fallisce!
            TestCamera.TestOrthCameraTransformation();

            TestRay.TestRayClose();
            TestRay.TestAt();
        }

        Parameters myParams = new Parameters(args);

        switch (myParams.mode)
        {
            case "PFMtoimg":
                HdrImage myImage = new HdrImage(path: myParams.input_file_name);

                HdrImage converted = myImage.TosRGB(factor: myParams.factor, gamma: myParams.gamma);
                converted.SaveLdrImg(myParams.output_file_name);
                break;
            
            case "demo":
                break;
        }
    }

    public class Parameters
    {
        public string mode = "";
        public string input_file_name = "";
        public float factor = 0.2f;
        public float gamma = 1;
        public string output_file_name = "";

        public Parameters(string[] args)
        {
            try
            {
                switch (args[0])
                {
                    case "PFMtoimg":
                    {
                        try
                        {
                            mode = args[0];
                            input_file_name = args[1];
                            factor = float.Parse(args[2],
                                CultureInfo.InvariantCulture); //altrimenti pensa che debba usare la virgola!
                            gamma = float.Parse(args[3], CultureInfo.InvariantCulture);
                            output_file_name = args[4];
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(
                                "usage: raytracer.exe PFMtoimg \"input_file_name\" factor gamma \"output_file_name\"");
                            Environment.Exit(1);

                        }

                        break;
                    }

                    case "demo":
                    {
                        mode = args[0];
                        break;
                    }

                    default:
                    {
                        Console.WriteLine("Commands: PFMtoimg or demo");
                        break;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Commands: PFMtoimg or demo");
                Environment.Exit(3);
            }
        }
    }
}