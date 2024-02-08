using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using UnityEngine.UIElements;
using UnityEditor.UI;

public class TagSawner : MonoBehaviour
{
    [SerializeField]
    private TFManager tfStatic;
    private Dictionary<int, Transform> tagTransforms = new Dictionary<int, Transform>();

    // Update is called once per frame
    void Update()
    {
        foreach(var s in tfStatic.frame_ids){
            if(
                s.StartsWith("apriltag") &&
                int.TryParse(s.Replace("apriltag", ""), out int id)
            ){
                if(! tagTransforms.ContainsKey(id)){
                    string imgPath = "ApriltagImages/apriltag" + id.ToString();
                    var tex = Resources.Load<Texture2D>(imgPath);
                    var obj = Instantiate(Resources.Load<GameObject>("Prefabs/Apriltag"));
                    var tag_tf = obj.transform;
                    tag_tf.parent = transform;
                    // "apriltag" -> "Apriltag"
                    tag_tf.name = s.Replace("ap", "Ap");
                    tag_tf.GetChild(0).GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tex);
                    tagTransforms.Add(id, tag_tf);
                }

                int i = tfStatic.frame_ids.IndexOf(s);
                tagTransforms[id].position = tfStatic.translations[i];
                tagTransforms[id].rotation = tfStatic.rotations[i];
            }
        }
    }
}
