using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] soundEffect; // ����� ���� Ŭ��
    private AudioSource audioSource; // ����� �ҽ� ������Ʈ

    void Start()
    {
        // AudioSource ������Ʈ ��������
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int n)
    {
        // AudioSource�� ����� AudioClip ����
        audioSource.clip = soundEffect[n];
        audioSource.Play();
    }
}
