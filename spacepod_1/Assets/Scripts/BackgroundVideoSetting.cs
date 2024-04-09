using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;
using UnityEditor;
public class BackgroundVideoSetting : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        SetVideo();
    }

    public void SetVideo()
    {
        videoPlayer.url = Directory.GetFiles(Application.streamingAssetsPath + $"/Video")[0];
        Debug.Log(videoPlayer.url);

        // 준비가 되면 동영상 재생 시작
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare();
        
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        // 동영상 재생 시작
        vp.Play();
    }
}
