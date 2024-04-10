using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearFlower : MonoBehaviour
{
    public void AppearStart()
    {
        StartCoroutine(AppearFlowerCoroutine(8));
    }

    private IEnumerator AppearFlowerCoroutine(float duration)
    {
        StartCoroutine(ChangeSizeCoroutine(0, 0.8f, 1, this.transform));
        yield return new WaitForSeconds(duration);
        StartCoroutine(ChangeSizeCoroutine(0.8f, 0, 1, this.transform));
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
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
