using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SoundManager soundManager;
    public GameObject prefabToSpawn;
    public List<GameObject> spawnPosL;
    public List<GameObject> spawnPosR;

    public void CreateBird(int n) // ���� �̹����� ������ ����
    {
        // �������� spawnPosition ��ġ�� ����
        int randomint = Random.Range(0, 5);
        GameObject spawnedObject = Instantiate(prefabToSpawn, n==1 ? spawnPosL[randomint].transform.position : spawnPosR[randomint].transform.position, Quaternion.Euler(new Vector3(0f,n==1?90f:-90f,0f)));
        spawnedObject.GetComponent<MoveBird>().MoveStart(n == 1 ? BirdDir.Left : BirdDir.Right, randomint);
        soundManager.PlaySound();
    }


}
