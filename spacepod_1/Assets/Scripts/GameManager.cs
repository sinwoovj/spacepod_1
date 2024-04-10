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
    public void CreateBird(int n, string imageDirectory) // ���� �̹����� ������ ����
    {
        // �������� spawnPosition ��ġ�� ����
        int randomint = Random.Range(0, 5);
        GameObject spawnedObject1 = Instantiate(prefabToSpawn[0], n==1 ? spawnPosL[randomint].transform.position : spawnPosR[randomint].transform.position, Quaternion.Euler(new Vector3(0f,n==1?90f:-90f,0f)));
        
        // ���ο� ���׸��� ����
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.name = birdCount.ToString();
        
        // Ư�� �̹����� ���׸��� �Ҵ�
        byte[] fileData = File.ReadAllBytes(imageDirectory);
        Debug.Log("Ư�� ���� �̹��� ���׸��� �Ҵ�");
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        newMaterial.mainTexture = texture;
        
        // �������� ���׸����� �ؽ��ĸ� ���Ƴ����ִ� �޼���
        ChangeTexture(spawnedObject1, newMaterial, "magpie");
        
        // ������Ʈ �̵�
        spawnedObject1.GetComponent<MoveBird>().MoveStart(n == 1 ? BirdDir.Left : BirdDir.Right, randomint);
        if (imageDirectory != Directory.EnumerateFiles(Application.streamingAssetsPath + $"/MaskImage", "*.png").ToArray()[0])
        {
            // �̹��� bin���� �̵�
            File.Move(Directory.GetFiles(Application.streamingAssetsPath + $"/scan1")[0], Application.streamingAssetsPath + $"/bin/{birdCount}.png");
            
            // ���� ����
            DeleteFolder(Application.streamingAssetsPath + $"/scan1");
        }
        
        // SFX ���
        soundManager.PlaySound(0);

        birdCount++;
    }

    public void CreateFlower(string imageDirectory)
    {
        // �������� spawnPosition ��ġ�� ����
        int randomint = Random.Range(0, 5);
        GameObject spawnedObject2 = Instantiate(prefabToSpawn[1], spawnPosB[randomint].transform.position , Quaternion.Euler(new Vector3(0,-90,0)));

        // ���ο� ���׸��� ����
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.name = flowerCount.ToString();

        // Ư�� �̹����� ���׸��� �Ҵ�
        byte[] fileData = File.ReadAllBytes(imageDirectory);
        Debug.Log("Ư�� �� �̹��� ���׸��� �Ҵ�");
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        newMaterial.mainTexture = texture;

        // �������� ���׸����� �ؽ��ĸ� ���Ƴ����ִ� �޼���
        ChangeTexture(spawnedObject2, newMaterial, "flower");

        // ������Ʈ �̵�
        spawnedObject2.GetComponent<AppearFlower>().AppearStart();

        if (imageDirectory != Directory.EnumerateFiles(Application.streamingAssetsPath + $"/MaskImage", "*.png").ToArray()[1])
        {
            // �̹��� bin���� �̵�
            File.Move(Directory.GetFiles(Application.streamingAssetsPath + $"/scan2")[0], Application.streamingAssetsPath + $"/bin/{birdCount}.png");
           
            // ���� ����
            DeleteFolder(Application.streamingAssetsPath + $"/scan2");
        }

        // SFX ���
        soundManager.PlaySound(1);

        flowerCount++;
    }

    void ChangeTexture(GameObject prefabInstance, Material newTexture, string tag)
    {
        if (prefabInstance != null)
        {
            // ������ �������� �ڽĵ��� ��ȸ�ϸ鼭 Ư�� ������ �����ϴ� �ڽ��� ã��
            foreach (Transform child in prefabInstance.transform)
            {
                // Ư�� ������ Ȯ���Ͽ� ������ �����ϴ� �ڽ��� ã��
                // ���� ���, �ڽ��� �±װ� "YourTag"�� ��츦 �������� ����
                if (child.CompareTag(tag))
                {
                    // ������ �����ϴ� �ڽ��� Renderer ������Ʈ���� ���׸����� ������
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
        // ���� ���� ��� ���� ����
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
                Debug.Log("������ �����Ǿ����ϴ�: " + folderPath);
            }
            else
            {
                Debug.Log("�̹� ������ �����մϴ�: " + folderPath);
            }
        }
    }
}
