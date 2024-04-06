using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip soundEffect; // 재생할 사운드 클립
    private AudioSource audioSource; // 오디오 소스 컴포넌트

    void Start()
    {
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();

        // AudioSource에 재생할 AudioClip 설정
        audioSource.clip = soundEffect;
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
