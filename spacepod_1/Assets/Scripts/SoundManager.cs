using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] audioSource; // ����� �ҽ� ������Ʈ

    void Start()
    {
        // AudioSource ������Ʈ ��������
        AudioSource[] audioSources = this.gameObject.GetComponents<AudioSource>();

        // �迭 �ʱ�ȭ
        audioSource = new AudioSource[audioSources.Length];

        // AudioSource ������Ʈ ����
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSource[i] = audioSources[i];
        }
    }

    public void PlaySound(int n)
    {
        // ����� �ҽ� ���
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
