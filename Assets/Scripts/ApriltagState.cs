using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using TFMessageMsg = RosMessageTypes.Tf2.TFMessageMsg;

public class ApriltagState : MonoBehaviour
{
    ROSConnection con;
    [SerializeField] GameObject ghostTilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        con = ROSConnection.GetOrCreateInstance();
        con.Subscribe<TFMessageMsg>("tf", CallbackTF);
    }

    void CallbackTF(TFMessageMsg msg){
        foreach(var tr in msg.transforms){
            string[] ss = tr.child_frame_id.Split(':');
            if(ss.Length == 2 && ss[0] == "tag36h11" && int.TryParse(ss[1], out int id)){
                var sprite = Resources.Load<Sprite>("AprilTag/tag36_11_" + id.ToString("D5") + ".png");
                var obj = Instantiate<GameObject>(ghostTilePrefab);
                var mat = obj.GetComponent<Material>();
            }
        }
    }
}
