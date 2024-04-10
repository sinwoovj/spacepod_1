using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public SoundManager soundManager;
    public GameObject[] prefabToSpawn;
    public List<GameObject> spawnPosL;
    public List<GameObject> spawnPosR;
    public List<GameObject> spawnPosB;
    public int birdCount = 0;
    public int flowerCount = 0;
    private void Start()
    {
        CreateSetUpFolder();
    }
    public void CreateBird(int n, string imageDirectory) // 차후 이미지를 끼워진 상태
    {
        // 프리팹을 spawnPosition 위치에 생성
        int randomint = Random.Range(0, 5);
        GameObject spawnedObject1 = Instantiate(prefabToSpawn[0], n==1 ? spawnPosL[randomint].transform.position : spawnPosR[randomint].transform.position, Quaternion.Euler(new Vector3(0f,n==1?90f:-90f,0f)));
        
        // 새로운 메테리얼 생성
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.name = birdCount.ToString();
        
        // 특정 이미지를 메테리얼에 할당
        byte[] fileData = File.ReadAllBytes(imageDirectory);
        Debug.Log("특정 제비 이미지 메테리얼 할당");
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        newMaterial.mainTexture = texture;
        
        // 프리팹의 메테리얼의 텍스쳐를 갈아끼워주는 메서드
        ChangeTexture(spawnedObject1, newMaterial, "magpie");
        
        // 오브젝트 이동
        spawnedObject1.GetComponent<MoveBird>().MoveStart(n == 1 ? BirdDir.Left : BirdDir.Right, randomint);
        if (imageDirectory != Directory.EnumerateFiles(Application.streamingAssetsPath + $"/MaskImage", "*.png").ToArray()[0])
        {
            // 이미지 bin으로 이동
            File.Move(Directory.GetFiles(Application.streamingAssetsPath + $"/scan1")[0], Application.streamingAssetsPath + $"/bin/{birdCount}.png");
            
            // 폴더 비우기
            DeleteFolder(Application.streamingAssetsPath + $"/scan1");
        }
        
        // SFX 재생
        soundManager.PlaySound(0);

        birdCount++;
    }

    public void CreateFlower(string imageDirectory)
    {
        // 프리팹을 spawnPosition 위치에 생성
        int randomint = Random.Range(0, 5);
        GameObject spawnedObject2 = Instantiate(prefabToSpawn[1], spawnPosB[randomint].transform.position , Quaternion.Euler(new Vector3(0,-90,0)));

        // 새로운 메테리얼 생성
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.name = flowerCount.ToString();

        // 특정 이미지를 메테리얼에 할당
        byte[] fileData = File.ReadAllBytes(imageDirectory);
        Debug.Log("특정 꽃 이미지 메테리얼 할당");
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        newMaterial.mainTexture = texture;

        // 프리팹의 메테리얼의 텍스쳐를 갈아끼워주는 메서드
        ChangeTexture(spawnedObject2, newMaterial, "flower");

        // 오브젝트 이동
        spawnedObject2.GetComponent<AppearFlower>().AppearStart();

        if (imageDirectory != Directory.EnumerateFiles(Application.streamingAssetsPath + $"/MaskImage", "*.png").ToArray()[1])
        {
            // 이미지 bin으로 이동
            File.Move(Directory.GetFiles(Application.streamingAssetsPath + $"/scan2")[0], Application.streamingAssetsPath + $"/bin/{birdCount}.png");
           
            // 폴더 비우기
            DeleteFolder(Application.streamingAssetsPath + $"/scan2");
        }

        // SFX 재생
        soundManager.PlaySound(1);

        flowerCount++;
    }

    void ChangeTexture(GameObject prefabInstance, Material newTexture, string tag)
    {
        if (prefabInstance != null)
        {
            // 생성된 프리팹의 자식들을 순회하면서 특정 조건을 만족하는 자식을 찾음
            foreach (Transform child in prefabInstance.transform)
            {
                // 특정 조건을 확인하여 조건을 만족하는 자식을 찾음
                // 예를 들어, 자식의 태그가 "YourTag"인 경우를 조건으로 설정
                if (child.CompareTag(tag))
                {
                    // 조건을 만족하는 자식의 Renderer 컴포넌트에서 메테리얼을 가져옴
                    Renderer renderer = child.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = newTexture;
                    }
                }
            }
        }
    }

    void DeleteFolder(string folder)
    {
        // 폴더 내의 모든 파일 삭제
        foreach (string file in Directory.GetFiles(folder))
        {
            File.Delete(file);
        }
    }

    private void OnApplicationQuit()
    {
        DeleteFolder(Application.streamingAssetsPath + $"/scan1");
        DeleteFolder(Application.streamingAssetsPath + $"/scan2");
        DeleteFolder(Application.streamingAssetsPath + $"/bin");
    }

    void CreateSetUpFolder()
    {
        string[] folderPaths = {
        Application.streamingAssetsPath + $"/scan1",
        Application.streamingAssetsPath + $"/scan2",
        Application.streamingAssetsPath + $"/bin",
        Application.streamingAssetsPath + $"/Video",
        Application.streamingAssetsPath + $"/MaskImage"
        };
        foreach(string folderPath in folderPaths)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Debug.Log("폴더가 생성되었습니다: " + folderPath);
            }
            else
            {
                Debug.Log("이미 폴더가 존재합니다: " + folderPath);
            }
        }
    }
}
