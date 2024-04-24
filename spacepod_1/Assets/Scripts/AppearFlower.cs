using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppearFlower : MonoBehaviour
{
    public float flowerDuration = 15f;
    public void AppearStart(string imageDirectory, int randomint, bool isStacks)
    {
        if (isStacks) GameObject.Find("GameManager").GetComponent<GameManager>().flowerStacks.RemoveAt(0);
        StartCoroutine(AppearFlowerCoroutine(randomint, flowerDuration, imageDirectory, isStacks));
    }

    private IEnumerator AppearFlowerCoroutine(int randomint, float duration, string imageDirectory, bool isStacks)
    {
        StartCoroutine(ChangeSizeCoroutine(0, 0.8f, 1, this.transform));
        yield return new WaitForSeconds(duration);
        StartCoroutine(ChangeSizeCoroutine(0.8f, 0, 1, this.transform));
        yield return new WaitForSeconds(2f);
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.isFlowerSpace = true;
        gameManager.randomints.Remove(randomint); // ������ ù ��° �׸��� ó�������Ƿ� ����
        Destroy(this.gameObject);
    }

    // ũ�⸦ �������� �����ϴ� �ڷ�ƾ �Լ�
    private IEnumerator ChangeSizeCoroutine(float startSize, float targetSize, float duration, Transform targetObject)
    {
        float timer = 0f;

        while (timer < duration)
        {
            // �ð��� ���� ���� ���
            float t = timer / duration;

            // �ﰢ �Լ��� ����Ͽ� ũ�� ����
            float newSize = Mathf.Lerp(startSize, targetSize, Mathf.Sin(t * Mathf.PI * 0.5f));
            targetObject.localScale = new Vector3(newSize, newSize, newSize);

            // ���� �����ӱ��� ���
            yield return null;

            // Ÿ�̸� ������Ʈ
            timer += Time.deltaTime;
        }

        // ũ�� ������ �Ϸ�� �Ŀ� ���� ũ��� ����
        targetObject.localScale = new Vector3(targetSize, targetSize, targetSize);
    }
}