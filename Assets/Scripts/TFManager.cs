using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Tf2;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class TFManager : MonoBehaviour
{
    public List<string> frame_ids = new List<string>();
    public List<string> parent_frame_ids = new List<string>();
    public List<Vector3> translations = new List<Vector3>();
    public List<Quaternion> rotations = new List<Quaternion>();


    // Start is called before the first frame update
    void Start()
    {
        var con = ROSConnection.GetOrCreateInstance();
        con.Subscribe<TFMessageMsg>("tf_static", CallbackTF);
        con.Subscribe<TFMessageMsg>("tf", CallbackTF);
    }


    void CallbackTF(TFMessageMsg msg)
    {
        foreach(var tf in msg.transforms){
            Vector3 t = tf.transform.translation.From<FLU>();
            Quaternion q = tf.transform.rotation.From<FLU>();
            if(frame_ids.Contains(tf.child_frame_id)){

            }
            else{
                frame_ids.Add(tf.child_frame_id);
                parent_frame_ids.Add(tf.header.frame_id);
                translations.Add(t);
                rotations.Add(q);
            }
        }
    }
}
