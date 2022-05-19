using System.Globalization;
using System.Numerics;
using Microsoft.Extensions.CommandLineUtils;
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
            TestCamera.TestOrthCameraTransformation();

            TestRay.TestRayClose();
            TestRay.TestAt();
            
            TestSphere.TestIntersect();
            TestSphere.TestTransf();

            
            TestImgTracer.Test_uvSubmapping();
            //TestImgTracer.TestImageCoverage();
            TestImgTracer.TestOrientation();
            
            //RandomNumber.test.testRand();
        }

        
        
        /*var app = new CommandLineApplication();
        var myArgs = app.Arguments;
        Console.WriteLine(myArgs+"\n");*/
        
        var myParams = new Parameters(args);

        switch (myParams.Mode)
        {
            case "PFMtoimg":
                HdrImage myImage = new HdrImage(path: myParams.InputFileName);

                HdrImage converted = myImage.TosRGB(factor: myParams.Factor, gamma: myParams.Gamma);
                converted.SaveLdrImg(myParams.OutputFileName);
                break;
            
            case "demo":
                var res = myParams.Resolution.Split("x");
                var width = int.Parse(res[0]);
                var height = int.Parse(res[1]);
                demo.demo.test(width, height, myParams.Angle, myParams.OutputFileName);
                break;
        }
    }
    
    public class Parameters
    {
        public readonly string Mode = "";
        public readonly string InputFileName = "";
        public readonly float Factor = 0.2f;
        public readonly float Gamma = 1;
        public readonly string OutputFileName = "";
        public readonly string Resolution = "";
        public readonly float Angle = .0f;
        
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
                            Mode = args[0];
                            InputFileName = args[1];
                            Factor = float.Parse(args[2],
                                CultureInfo.InvariantCulture); //altrimenti pensa che debba usare la virgola!
                            Gamma = float.Parse(args[3], CultureInfo.InvariantCulture);
                            OutputFileName = args[4];
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
                        try
                        {
                            Mode = args[0];
                            Resolution = args[1];
                            Angle = float.Parse(args[2], CultureInfo.InvariantCulture);
                            OutputFileName = args[3];
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("usage: raytracer.exe demo \"widthxheight\" angle \"output_file_name\"");
                            throw;
                        }
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