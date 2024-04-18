using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] audioSource; // 오디오 소스 컴포넌트

    void Start()
    {
        // AudioSource 컴포넌트 가져오기
        AudioSource[] audioSources = this.gameObject.GetComponents<AudioSource>();

        // 배열 초기화
        audioSource = new AudioSource[audioSources.Length];

        // AudioSource 컴포넌트 복사
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSource[i] = audioSources[i];
        }
    }

    public void PlaySound(int n)
    {
        // 오디오 소스 재생
        if (n >= 0 && n < audioSource.Length && audioSource[n] != null)
        {
            audioSource[n].Play();
        }
        else
        {
            Debug.LogError("Invalid audio source index or audio source is null.");
        }
    }
}
