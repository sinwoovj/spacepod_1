using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip soundEffect; // ����� ���� Ŭ��
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ

    void Start()
    {
        // AudioSource ������Ʈ ��������
        audioSource = GetComponent<AudioSource>();

        // AudioSource�� ����� AudioClip ����
        audioSource.clip = soundEffect;
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
