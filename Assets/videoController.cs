using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class videoController : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer videoPlayer;
    public BVHRecorder bvhRecorder;
    public bvhRecorderComponent bvhRecorderComponent;
    // Start is called before the first frame update
    void Start()
    {
        //Screen.SetResolution(640, 480, true);
        //Debug.LogError("HIIIIII");
        string[] args = System.Environment.GetCommandLineArgs();
        string video_path = "";
        //string video_path = "D:\\code\\metagen\\shadermotion\\Assets\\deadass1.mp4";
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log("ARG " + i + ": " + args[i]);
            if (args[i] == "--video")
            {
                video_path = args[i + 1];
            }
        }
        //video_path = "D:\\code\\metagen\\shadermotion\\Assets\\deadass1.mp4";
        //video_path = "D:\\code\\metagen\\shadermotion_data_test\\test.mp4";
        bvhRecorder.filename = Path.GetFileName(video_path);
        Debug.Log(video_path);

        videoPlayer.url = video_path;
        videoPlayer.Play();
        bvhRecorder.capturing = true;
        bvhRecorder.directory = Path.GetDirectoryName(video_path);
        bvhRecorderComponent.has_begun_playing = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
