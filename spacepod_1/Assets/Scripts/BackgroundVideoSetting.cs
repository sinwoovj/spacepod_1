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
        videoPlayer.url = Application.streamingAssetsPath + $"/Video.mp4";
        Debug.Log(videoPlayer.url);

        // �غ� �Ǹ� ������ ��� ����
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare();
        
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        // ������ ��� ����
        vp.Play();
    }
    /*
    private void OnApplicationQuit()
    {
        string videoFilePath = Application.streamingAssetsPath + $"/Video.mp4";

        if (File.Exists(videoFilePath))
        {
            File.Delete(videoFilePath);
            Debug.Log("videoFile deleted successfully.");
        }
        else
        {
            Debug.Log("videoFile does not exist.");
        }
    }*/
}
