using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using CompressedImageMsg = RosMessageTypes.Sensor.CompressedImageMsg;
using HeaderMsg = RosMessageTypes.Std.HeaderMsg;
using CameraInfoMsg = RosMessageTypes.Sensor.CameraInfoMsg;
using RegionOfInterestMsg = RosMessageTypes.Sensor.RegionOfInterestMsg;

public class RobotEye : MonoBehaviour
{
    [SerializeField] private RenderTexture vision;
    private ROSConnection con;
    private string imgTopicName = "camera/image/compressed";
    private string infoTopicName = "camera/camera_info";

    void Start()
    {
        con = ROSConnection.GetOrCreateInstance();
        con.RegisterPublisher<CompressedImageMsg>(imgTopicName);
        con.RegisterPublisher<CameraInfoMsg>(infoTopicName);

        var cam = GetComponent<Camera>();
        float focal_length = 1f / Mathf.Tan(Mathf.Deg2Rad * cam.fieldOfView / 2f);
        focal_length *= (float)vision.height / 2f;
        var msg = new CameraInfoMsg(
            new HeaderMsg(), (uint)vision.height, (uint)vision.width,
            "plumb_bob", new double[]{0d,0d,0d,0d,0d},
            new double[]{
                focal_length, 0d, (float)vision.width / 2f,
                0d, focal_length, (float)vision.height / 2f,
                0d, 0d, 1d
            },
            new double[]{
                1d, 0d, 0d,
                0d, 1d, 0d,
                0d, 0d, 1d
            },
            new double[]{
                focal_length, 0d, (float)vision.width / 2f, 0d,
                0d, focal_length, (float)vision.height / 2f, 0d,
                0d, 0d, 1d, 0d
            },
            0, 0,
            new RegionOfInterestMsg(0, 0, (uint)vision.width, (uint)vision.height, false)
        );
        con.Publish(infoTopicName, msg);
    }

    public void GetImageAndPublish(){
        var tex = new Texture2D(vision.width, vision.height, TextureFormat.RGB24, false);
        RenderTexture.active = vision;

        tex.ReadPixels(new Rect(0, 0, vision.width, vision.height), 0, 0);
        tex.Apply();
        var msg = new CompressedImageMsg(new HeaderMsg(), "png", tex.EncodeToPNG());

        RenderTexture.active = null;
        Destroy(tex);

        con.Publish(imgTopicName, msg);
    }
}
