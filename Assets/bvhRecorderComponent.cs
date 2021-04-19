using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bvhRecorderComponent : MonoBehaviour
{
    public Animator animator;
    public UnityEngine.Video.VideoPlayer videoPlayer;
    public BVHRecorder recorder;
    bool has_saved = false;
    public bool has_begun_playing = false;
    //public string animation_name;
    // Start is called before the first frame update
    void Start()
    {
        recorder.capturing = true;
    }
    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
    }
    bool VideoHasStopped()
    {
        //Debug.Log(videoPlayer.frame);
        //Debug.Log(videoPlayer.frameCount);
        return videoPlayer.frame >= 0 ? ((ulong)videoPlayer.frame) == (videoPlayer.frameCount-1) : false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(animator != null);
        //Debug.Log(videoPlayer != null);
        //Debug.Log(videoPlayer.frame);
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
        if (has_begun_playing)
        {
            //if (animator != null)
            //    Debug.Log("HII");
            //if (!AnimatorIsPlaying() && !has_saved)
            //    {
            //        Debug.Log("Stopped!");
            //        recorder.capturing = false;
            //        recorder.saveBVH();
            //        has_saved = true;
            //    }
            //else if (videoPlayer != null)
            if (videoPlayer != null) {
                //Debug.Log(videoPlayer.frame);
                //Debug.Log(videoPlayer.frameCount);
                //Debug.Log(VideoHasStopped());
                //Debug.Log(has_saved);
                //Debug.Log(VideoHasStopped() && !has_saved);
                if (VideoHasStopped() && !has_saved)
                {
                    Debug.Log("Stopped!");
                    recorder.capturing = false;
                    recorder.saveBVH();
                    has_saved = true;
                }
            }

            if (has_saved)
                Application.Quit();
        }
    }
    void OnApplicationQuit()
    {
        if (!has_saved)
        {
            Debug.Log("Stopped!");
            recorder.capturing = false;
            recorder.saveBVH();
            has_saved = true;
        }
    }
}
