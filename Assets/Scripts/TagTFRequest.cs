using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using RosMessageTypes.Std;

public class TagTFRequest : MonoBehaviour
{
    private ROSConnection con;
    private string topicName = "req_apriltag_tf";

    // Start is called before the first frame update
    void Start()
    {
        con = ROSConnection.GetOrCreateInstance();
        con.RegisterPublisher<BoolMsg>(topicName);
    }

    public void PublishReq()
    {
        con.Publish(topicName, new BoolMsg(true));
    }
}
