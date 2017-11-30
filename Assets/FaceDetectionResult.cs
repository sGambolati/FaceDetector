using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public class FaceDetectionResult
{
    public FaceDetectionResult(int top, int left, int right, int bottom)
    {
        this.top = top;
        this.left = left;
        this.right = right;
        this.bottom = bottom;
    }

    public int top { get; set; }
    public int left { get; set; }
    public int right { get; set; }
    public int bottom { get; set; }

    public override string ToString()
    {
        return string.Format("\tTop: {0}\n" +
                "\tLeft: {1}\n" +
                "\tRight: {2}\n" +
                "\tBottom: {3}", 
                this.top, this.left, this.right, this.bottom);
    }
};
