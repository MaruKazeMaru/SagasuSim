using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using CompressedImageMsg = RosMessageTypes.Sensor.CompressedImageMsg;
using HeaderMsg = RosMessageTypes.Std.HeaderMsg;

public class ButtonVision : MonoBehaviour
{
    [SerializeField] private RenderTexture vision;
    private ROSConnection con;
    private string topicName = "image/compressed";

    void Start()
    {
        con = ROSConnection.GetOrCreateInstance();
        con.RegisterPublisher<CompressedImageMsg>(topicName);
    }

    void GetImageAndPublish(){
        var tex = new Texture2D(vision.width, vision.height, TextureFormat.RGB24, false);
        RenderTexture.active = vision;

        tex.ReadPixels(new Rect(0, 0, vision.width, vision.height), 0, 0);
        tex.Apply();
        var msg = new CompressedImageMsg(new HeaderMsg(), "png", tex.EncodeToPNG());

        RenderTexture.active = null;
        Destroy(tex);

        con.Publish(topicName, msg);
    }
}
