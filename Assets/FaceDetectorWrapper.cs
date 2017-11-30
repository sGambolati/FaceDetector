using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;

using UnityEngine;
using UnityEngine.XR.WSA.WebCam;
using System;

public class FaceDetectorWrapper : MonoBehaviour
{
    [DllImport("FaceDetectionUWP.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Test(int number);

    [DllImport("FaceDetectionUWP.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Detect(string filePath, int[] top, int[] left, int[] right, int[] bottom);

    private PhotoCapture photoCapture = null;

    private const string FILE_NAME = @"cognitive_analysis.jpg";

    private bool isDetectingFaces = false;
    private bool PhotoModeStarted = false;

    // Use this for initialization
    void Start()
    {
        //DLL method test.
        //Debug.Log(Test(1));
        PhotoCapture.CreateAsync(false, this.OnPhotoCreated);
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotoModeStarted  && !isDetectingFaces)
        {
            isDetectingFaces = true;

            string filename = string.Format(FILE_NAME);
            string filePath = Path.Combine(Application.persistentDataPath, filename);

            this.photoCapture.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, this.OnCapturedPhotoToDisk);
        }
    }

    private void CallDLL(string fileName)
    {
        int[] top = new int[32];
        int[] left = new int[32];
        int[] right = new int[32];
        int[] bottom = new int[32];

        var size = Detect(fileName, top, left, right, bottom);

        Debug.LogFormat("Detect: {0} faces.", size);

        IList<FaceDetectionResult> list = new List<FaceDetectionResult>();
        for (int i = 0; i < size; i++)
        {
            var rectangle = new FaceDetectionResult(top[i], left[i], right[i], bottom[i]);
            list.Add(rectangle);
            Debug.LogFormat("Face[{0}]: \n{1}", i, rectangle);
        }

        isDetectingFaces = false;
    }

    // This method store the PhotoCapture object just created and retrieve the high quality
    // available for the camera and then request to start capturing the photo with the
    // given camera parameters.
    private void OnPhotoCreated(PhotoCapture captureObject)
    {
        this.photoCapture = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        CameraParameters c = new CameraParameters()
        {
            hologramOpacity = 0.0f,
            cameraResolutionWidth = cameraResolution.width,
            cameraResolutionHeight = cameraResolution.height,
            pixelFormat = CapturePixelFormat.BGRA32
        };
        captureObject.StartPhotoModeAsync(c, this.OnPhotoModeStarted);
    }

    // This method is called when we have access to the camera and can take photo with it.
    // We request to take the photo and store it in the storage.
    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        PhotoModeStarted = result.success;
        if (result.success)
        {
            //string filename = string.Format(FILE_NAME);
            //string filePath = Path.Combine(Application.persistentDataPath, filename);
            //this.photoCapture.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, this.OnCapturedPhotoToDisk);
        }
        else
        {
            Debug.LogError("Unable to start photo mode.");
        }
    }

    // This method is called when the photo is finish taked (or not, so check the succes property)
    // We can read the file from disk and do anything we need with it.
    // Finally, we request to stop the photo mode to free the resource.
    private void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            string fileName = string.Format(FILE_NAME);
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            //byte[] image = File.ReadAllBytes(filePath);

            // We have the photo taken.
            CallDLL(filePath);
            //Debug.Log(filePath);

            isDetectingFaces = false;
        }
        else
        {
            Debug.LogError("Failed to save Photo to disk.");
        }
    }

    private void OnDestroy()
    {
        this.photoCapture.StopPhotoModeAsync(this.OnStoppedPhotoMode);
    }

    // This method is called when the photo mode is stopped and we can dispose the resources allocated.
    private void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        this.photoCapture.Dispose();
        this.photoCapture = null;
    }
}
