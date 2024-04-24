using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public SoundManager soundManager;
    public GameObject[] prefabToSpawn;
    public List<GameObject> spawnPosL;
    public List<GameObject> spawnPosR;
    public List<GameObject> spawnPosB;
    public int birdCount = 0;
    public int flowerCount = 0;
    public List<string> flowerStacks = new List<string>();
    public bool isFlowerSpace = true;
    public bool isFlowerStack = false;
    public List<int> randomints = new List<int>();
    public int flowerCountLimit = 10;
    public int stackFlowerCount = 0;
    private void Start()
    {
        CreateSetUpFolder();
    }
    public void CreateBird(int n, string imageDirectory) // ���� �̹����� ������ ����
    {
        // �������� spawnPosition ��ġ�� ����
        int randomint = Random.Range(0, 3);
        GameObject spawnedObject1 = Instantiate(prefabToSpawn[0], n == 1 ? spawnPosL[randomint].transform.position : spawnPosR[randomint].transform.position, Quaternion.Euler(new Vector3(0f, n == 1 ? 90f : -90f, 0f)));

        // ���ο� ���׸��� ����
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.name = birdCount.ToString();

        // Ư�� �̹����� ���׸��� �Ҵ�
        byte[] fileData;
        // ������ �о���� �� FileShare.ReadWrite �ɼ��� ����Ͽ� ���� ������ �����ϰ� ��
        using (FileStream fs = new FileStream(imageDirectory, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            fileData = new byte[fs.Length];
            fs.Read(fileData, 0, (int)fs.Length);
        }
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
            File.Move(Directory.GetFiles(Application.streamingAssetsPath + $"/scan1")[0], Application.streamingAssetsPath + $"/bin2/B{birdCount}.png");

            // ���� ����
            DeleteFolder(Application.streamingAssetsPath + $"/scan1");
        }

        // SFX ���
        soundManager.PlaySound(0);

        birdCount++;
    }

    public void CreateFlower(string imageDirectory)
    {
        if (!File.Exists(imageDirectory))
        {
            Debug.LogError("�̹��� ������ �������� �ʽ��ϴ�: " + imageDirectory);
            return;
        }

        if (isFlowerStack)
        {
            if (flowerStacks.Count >= flowerCountLimit)
            {
                File.Delete(imageDirectory);
                Debug.Log("�Ѱ谪���� �ʰ��� ������ �ڵ� �����Ǿ����ϴ�.");
                return;
            }
            ++stackFlowerCount;
            File.Move(Directory.GetFiles(Application.streamingAssetsPath + $"/scan2")[0],
                Application.streamingAssetsPath + $"/bin/F{stackFlowerCount}.png");
            imageDirectory = Application.streamingAssetsPath + $"/bin/F{stackFlowerCount}.png";
            flowerStacks.Add(imageDirectory);
            return;
        }
        else
        {
            int randomint = GenerateRandomNumber(5, randomints);
            if(randomint == -1){
                Debug.Log("����Ʈ�� ��� �������� ���� �ش� ������ ���� ����(bin)���� �̵��մϴ�.");
                isFlowerStack = true;
                isFlowerSpace = false;
                ++stackFlowerCount;
                File.Move(Directory.GetFiles(Application.streamingAssetsPath + $"/scan2")[0],
                Application.streamingAssetsPath + $"/bin/F{stackFlowerCount}.png");
                flowerStacks.Add(Application.streamingAssetsPath + $"/bin/F{stackFlowerCount}.png");
                StartCoroutine(flowerStackProcessCoroutine());
            }
            else {
                File.Move(Directory.GetFiles(Application.streamingAssetsPath + $"/scan2")[0], Application.streamingAssetsPath + $"/bin2/F{flowerCount}.png");
                GenerateFlower(randomint, Application.streamingAssetsPath + $"/bin2/F{flowerCount}.png", false);
            }
        }

    }
    public void GenerateFlower(int randomint, string imageDirectory, bool isStacks)
    {
        //if (!File.Exists(imageDirectory))
        //{
        //    Debug.LogError("�̹��� ������ �������� �ʽ��ϴ�: " + imageDirectory);
        //    return;
        //}
        GameObject spawnedObject2 = Instantiate(prefabToSpawn[1], spawnPosB[randomint].transform.position, Quaternion.Euler(new Vector3(0, -90, 0)));

        // ���ο� ���׸��� ����
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.name = flowerCount.ToString();

        // Ư�� �̹����� ���׸��� �Ҵ�
        byte[] fileData;
        // ������ �о���� �� FileShare.ReadWrite �ɼ��� ����Ͽ� ���� ������ �����ϰ� ��
        Debug.LogWarning($"FileStream imageDirectory : {imageDirectory}");
        using (FileStream fs = new FileStream(imageDirectory, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            fileData = new byte[fs.Length];
            fs.Read(fileData, 0, (int)fs.Length);
        }
        Debug.Log("Ư�� �� �̹��� ���׸��� �Ҵ�");
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        newMaterial.mainTexture = texture;

        // �������� ���׸����� �ؽ��ĸ� ���Ƴ����ִ� �޼���
        ChangeTexture(spawnedObject2, newMaterial, "flower");

        // ������Ʈ ��ȯ
        spawnedObject2.GetComponent<AppearFlower>().AppearStart(imageDirectory, randomint, isStacks);
        // SFX ���
        soundManager.PlaySound(1);

        flowerCount++;
    }

    public IEnumerator flowerStackProcessCoroutine()
    {
        while (flowerStacks.Count > 0)
        {
            if (isFlowerSpace)
            {
                if (randomints.Count >= 5) isFlowerSpace = false;
                else
                {
                    // GenerateRandomNumber�� ȣ���Ͽ� ���� ���� ������
                    int randomint = GenerateRandomNumber(5, randomints);
                    if (randomint != -1)
                    {
                        Debug.LogWarning($"fowerStackFirst : {flowerStacks.First()}");
                        // ���� ������ GenerateFlower ȣ��
                        GenerateFlower(randomint, flowerStacks.First(), true);
                        Debug.Log($"�����Ǿ� �ִ� �� ���� : {randomints.Count()}");
                        Debug.Log($"���� �� ���� ���� : {flowerStacks.Count()}");
                    }
                }
            }
            yield return null; // �߰��� �κ�: ���� ���������� �Ѿ�� ���� yield return null �߰�
        }
        isFlowerStack = false;
        Debug.Log("�� �̻� ������ �����ϴ�.");
    }
    public int GenerateRandomNumber(int to, List<int> A)
    {
        int roofTime = 0;
        int randomnumber = 0;

        do
        {
            if (roofTime >= to)
            {
                Debug.LogWarning("�ش� ����Ʈ�� ��á���ϴ�.");
                return -1;
            }
            randomnumber = Random.Range(0, to); // 0���� 4 ������ ���� ����

            roofTime++;
        }
        while (A.Contains(randomnumber)); // A ����Ʈ�� ��ġ�� ���� ���� ������ �ݺ�

        randomints.Add(randomnumber);

        return randomnumber;
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
        DeleteFolder(Application.streamingAssetsPath + $"/bin2");
    }

    void CreateSetUpFolder()
    {
        string[] folderPaths = {
        Application.streamingAssetsPath + $"/scan1",
        Application.streamingAssetsPath + $"/scan2",
        Application.streamingAssetsPath + $"/bin",
        Application.streamingAssetsPath + $"/bin2",
        Application.streamingAssetsPath + $"/Video",
        Application.streamingAssetsPath + $"/MaskImage"
        };
        foreach (string folderPath in folderPaths)
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
    private string GetUniqueFileName(string filePath)
    {
        string directory = Path.GetDirectoryName(filePath);
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string fileExtension = Path.GetExtension(filePath);
        string uniqueFileName = filePath;
        int count = 1;

        while (File.Exists(uniqueFileName))
        {
            uniqueFileName = Path.Combine(directory, fileName + "_" + count + fileExtension);
            count++;
        }

        return uniqueFileName;
    }
}