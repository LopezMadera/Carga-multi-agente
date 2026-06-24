using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class CameraPoseSubscriber : MonoBehaviour
{
    ROSConnection ros;

    public string topicName = "/camera_pose";

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<PoseMsg>(topicName, PoseCallback);
    }

    void PoseCallback(PoseMsg msg)
    {
        transform.position = new Vector3(
            (float)msg.position.x,
            (float)msg.position.y,
            (float)msg.position.z
        );

        transform.rotation = new Quaternion(
            (float)msg.orientation.x,
            (float)msg.orientation.y,
            (float)msg.orientation.z,
            (float)msg.orientation.w
        );
    }
}
