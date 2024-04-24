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
        gameManager.randomints.Remove(randomint); // 스택의 첫 번째 항목을 처리했으므로 제거
        Destroy(this.gameObject);
    }

    // 크기를 비선형으로 변경하는 코루틴 함수
    private IEnumerator ChangeSizeCoroutine(float startSize, float targetSize, float duration, Transform targetObject)
    {
        float timer = 0f;

        while (timer < duration)
        {
            // 시간에 따른 비율 계산
            float t = timer / duration;

            // 삼각 함수를 사용하여 크기 변경
            float newSize = Mathf.Lerp(startSize, targetSize, Mathf.Sin(t * Mathf.PI * 0.5f));
            targetObject.localScale = new Vector3(newSize, newSize, newSize);

            // 다음 프레임까지 대기
            yield return null;

            // 타이머 업데이트
            timer += Time.deltaTime;
        }

        // 크기 변경이 완료된 후에 최종 크기로 설정
        targetObject.localScale = new Vector3(targetSize, targetSize, targetSize);
    }
}