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
    public void CreateBird(int n, string imageDirectory) // 차후 이미지를 끼워진 상태
    {
        // 프리팹을 spawnPosition 위치에 생성
        int randomint = Random.Range(0, 3);
        GameObject spawnedObject1 = Instantiate(prefabToSpawn[0], n == 1 ? spawnPosL[randomint].transform.position : spawnPosR[randomint].transform.position, Quaternion.Euler(new Vector3(0f, n == 1 ? 90f : -90f, 0f)));

        // 새로운 메테리얼 생성
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.name = birdCount.ToString();

        // 특정 이미지를 메테리얼에 할당
        byte[] fileData;
        // 파일을 읽어들일 때 FileShare.ReadWrite 옵션을 사용하여 파일 공유를 가능하게 함
        using (FileStream fs = new FileStream(imageDirectory, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            fileData = new byte[fs.Length];
            fs.Read(fileData, 0, (int)fs.Length);
        }
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
            File.Move(Directory.GetFiles(Application.streamingAssetsPath + $"/scan1")[0], Application.streamingAssetsPath + $"/bin2/B{birdCount}.png");

            // 폴더 비우기
            DeleteFolder(Application.streamingAssetsPath + $"/scan1");
        }

        // SFX 재생
        soundManager.PlaySound(0);

        birdCount++;
    }

    public void CreateFlower(string imageDirectory)
    {
        if (!File.Exists(imageDirectory))
        {
            Debug.LogError("이미지 파일이 존재하지 않습니다: " + imageDirectory);
            return;
        }

        if (isFlowerStack)
        {
            if (flowerStacks.Count >= flowerCountLimit)
            {
                File.Delete(imageDirectory);
                Debug.Log("한계값보다 초과된 파일은 자동 삭제되었습니다.");
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
                Debug.Log("리스트가 모두 꽉참으로 인해 해당 파일은 스택 폴더(bin)으로 이동합니다.");
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
        //    Debug.LogError("이미지 파일이 존재하지 않습니다: " + imageDirectory);
        //    return;
        //}
        GameObject spawnedObject2 = Instantiate(prefabToSpawn[1], spawnPosB[randomint].transform.position, Quaternion.Euler(new Vector3(0, -90, 0)));

        // 새로운 메테리얼 생성
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.name = flowerCount.ToString();

        // 특정 이미지를 메테리얼에 할당
        byte[] fileData;
        // 파일을 읽어들일 때 FileShare.ReadWrite 옵션을 사용하여 파일 공유를 가능하게 함
        Debug.LogWarning($"FileStream imageDirectory : {imageDirectory}");
        using (FileStream fs = new FileStream(imageDirectory, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            fileData = new byte[fs.Length];
            fs.Read(fileData, 0, (int)fs.Length);
        }
        Debug.Log("특정 꽃 이미지 메테리얼 할당");
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        newMaterial.mainTexture = texture;

        // 프리팹의 메테리얼의 텍스쳐를 갈아끼워주는 메서드
        ChangeTexture(spawnedObject2, newMaterial, "flower");

        // 오브젝트 소환
        spawnedObject2.GetComponent<AppearFlower>().AppearStart(imageDirectory, randomint, isStacks);
        // SFX 재생
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
                    // GenerateRandomNumber를 호출하여 랜덤 값을 가져옴
                    int randomint = GenerateRandomNumber(5, randomints);
                    if (randomint != -1)
                    {
                        Debug.LogWarning($"fowerStackFirst : {flowerStacks.First()}");
                        // 랜덤 값으로 GenerateFlower 호출
                        GenerateFlower(randomint, flowerStacks.First(), true);
                        Debug.Log($"생성되어 있는 꽃 갯수 : {randomints.Count()}");
                        Debug.Log($"남은 꽃 스택 갯수 : {flowerStacks.Count()}");
                    }
                }
            }
            yield return null; // 추가된 부분: 다음 프레임으로 넘어가기 위해 yield return null 추가
        }
        isFlowerStack = false;
        Debug.Log("더 이상 스택이 없습니다.");
    }
    public int GenerateRandomNumber(int to, List<int> A)
    {
        int roofTime = 0;
        int randomnumber = 0;

        do
        {
            if (roofTime >= to)
            {
                Debug.LogWarning("해당 리스트는 꽉찼습니다.");
                return -1;
            }
            randomnumber = Random.Range(0, to); // 0부터 4 사이의 난수 생성

            roofTime++;
        }
        while (A.Contains(randomnumber)); // A 리스트와 겹치는 수가 나올 때까지 반복

        randomints.Add(randomnumber);

        return randomnumber;
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
                Debug.Log("폴더가 생성되었습니다: " + folderPath);
            }
            else
            {
                Debug.Log("이미 폴더가 존재합니다: " + folderPath);
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