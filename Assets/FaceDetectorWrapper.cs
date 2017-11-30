using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class FaceDetectorWrapper : MonoBehaviour
{
    [DllImport("FaceDetectionUWP.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Test(int number);

    //[DllImport("FaceDetection.dll", CallingConvention = CallingConvention.Cdecl)]
    //public static extern int Detect(string filePath, int[] top, int[] left, int[] right, int[] bottom);

    // Use this for initialization
    void Start()
    {
        Debug.Log(Test(1));
    }

    // Update is called once per frame
    void Update()
    {
        //var babu = new FaceDetection.Library.FaceDetectorInterop();
        //Debug.Log(string.Format("Test: {0}", babu.TestInterop(1)));

        //Debug.Log( Directory.GetFiles(Directory.GetCurrentDirectory(), "FaceDetection.dll") );

        //File.CreateText(Directory.GetCurrentDirectory() + @"\babu.txt");
    }

    /*private void lalala()
    {
        Console.WriteLine("Test: {0}", Test(1));

        Stopwatch swDetect = Stopwatch.StartNew();
        int[] top = new int[32];
        int[] left = new int[32];
        int[] right = new int[32];
        int[] bottom = new int[32];

        var size = Detect(@"C:\Projects\sandbox\dlib-19.7\examples\faces\2008_002506.jpg", top, left, right, bottom);

        Console.WriteLine("Detect: {0} faces.", size);
        swDetect.Stop();
        Console.WriteLine("ElapsedTime: {0} ms.", swDetect.ElapsedMilliseconds);

        IList<RectangleStruct> list = new List<RectangleStruct>();
        for (int i = 0; i < size; i++)
        {
            var rectangle = new RectangleStruct(top[i], left[i], right[i], bottom[i]);
            list.Add(rectangle);
            Console.WriteLine("Face[{0}]: \n{1}", i, rectangle);
        }

        Console.ReadKey();
    }*/
}
