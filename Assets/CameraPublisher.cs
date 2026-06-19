using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;

public class CameraPublisher : MonoBehaviour
{
    public Camera cameraSensor;
    public RenderTexture renderTexture;

    public string topicName = "/camera/image_raw";

    ROSConnection ros;

    Texture2D texture2D;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<ImageMsg>(topicName);

        texture2D = new Texture2D(
            renderTexture.width,
            renderTexture.height,
            TextureFormat.RGB24,
            false);

        cameraSensor.targetTexture = renderTexture;

        InvokeRepeating(nameof(PublishImage), 1.0f, 0.1f);   //10 Hz
    }

    void PublishImage()
    {
        RenderTexture.active = renderTexture;

        texture2D.ReadPixels(
            new Rect(0, 0, renderTexture.width, renderTexture.height),
            0,
            0);

        texture2D.Apply();

        byte[] imageData = texture2D.GetRawTextureData();

        double time = Time.time;

        int sec = (int)time;
        uint nanosec = (uint)((time - sec) * 1000000000);

        HeaderMsg header = new HeaderMsg(
            new TimeMsg(sec, nanosec),
            "camera"
        );

        ImageMsg image = new ImageMsg(
            header,
            (uint)renderTexture.height,
            (uint)renderTexture.width,
            "rgb8",
            0,
            (uint)(renderTexture.width * 3),
            imageData
        );

        ros.Publish(topicName, image);

        RenderTexture.active = null;
    }
}