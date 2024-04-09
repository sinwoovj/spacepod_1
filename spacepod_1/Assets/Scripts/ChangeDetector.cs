using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System.IO;
//using Codice.Client.Commands.Matcher;
/*
class ChangeDetector : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
    {        
        foreach (string str in importedAssets)
        {
            Debug.Log("Imported File Name: " + str);
            string assetFolder = Path.GetDirectoryName(str);

            if (str.Contains("SetValue.xlsx"))
            {
                Debug.Log("Fixed Excel Data");
                DataConverter.ReadFromExcel();
            }
            if (assetFolder.Contains("scan1")) // 제비 
            {
                Debug.Log("Imported Magpie Image: " + str);
                GameObject.Find("GameManager").GetComponent<GameManager>().CreateBird(Random.Range(1, 3));
            }
            if (assetFolder.Contains("scan2")) // 꽃
            {
                Debug.Log("Imported Flower Image: " + str);
                GameObject.Find("GameManager").GetComponent<GameManager>().CreateFlower();
            }
            if (assetFolder.Contains("Video")) 
            {
                Debug.Log("Imported video: " + str);
                GameObject background = GameObject.Find("Background");
                if (background != null)
                {
                    background.GetComponent<BackgroundVideoSetting>().SetVideo();
                }
            }            
        }
        foreach (string str in deletedAssets)
        {
            Debug.Log("Deleted Asset: " + str);
        }

        for (int i = 0; i < movedAssets.Length; i++)
        {
            Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
        }

        if (didDomainReload)
        {
            Debug.Log("Domain has been reloaded");
        }
    }
}*/
public class ChangeDetector : MonoBehaviour
{
    public List<string> foldersToMonitor = new List<string>();

    private Dictionary<string, List<string>> previousFilesDict = new Dictionary<string, List<string>>();

    void Start()
    {
        foldersToMonitor.Add(Application.streamingAssetsPath + $"/scan1");
        foldersToMonitor.Add(Application.streamingAssetsPath + $"/scan2");
        foldersToMonitor.Add(Application.streamingAssetsPath + $"/bin");
        foldersToMonitor.Add(Application.streamingAssetsPath + $"/MaskImage");
        foldersToMonitor.Add(Application.streamingAssetsPath + $"/Video");
        foldersToMonitor.Add(Application.streamingAssetsPath);

        // 초기 파일 목록 설정
        foreach (string folderPath in foldersToMonitor)
        {
            previousFilesDict[folderPath] = GetFileList(folderPath);
        }

        // 파일 모니터링 코루틴 시작
        StartCoroutine(MonitorFolderChanges());
    }

    IEnumerator MonitorFolderChanges()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 폴더를 모니터링하는 간격 설정 (예: 1초)

            // 현재 파일 목록 가져오기
            foreach (string folderPath in foldersToMonitor)
            {
                // 현재 파일 목록 가져오기
                List<string> currentFiles = GetFileList(folderPath);

                // 이전 파일 목록과 비교하여 변경된 파일 확인
                List<string> changedFiles = GetChangedFiles(previousFilesDict[folderPath], currentFiles);


                // 변경된 파일 목록이 있을 경우
                if (changedFiles.Count > 0)
                {
                    // 변경된 파일 목록 출력 또는 원하는 작업 수행
                    Debug.Log("Changed files: " + string.Join(", ", changedFiles));
                    foreach (string str in changedFiles)
                    {
                        Debug.Log("Imported File Name: " + str);
                        string assetFolder = Path.GetDirectoryName(str);

                        if (str.Contains("SetValue.xlsx"))
                        {
                            Debug.Log("Fixed Excel Data");
                            DataConverter.ReadFromExcel();
                        }
                        else if (assetFolder.Contains("scan1")) // 제비 
                        {
                            Debug.Log("Imported Magpie Image: " + str);
                            GameObject.Find("GameManager").GetComponent<GameManager>().CreateBird(Random.Range(1, 3), Directory.GetFiles(Application.streamingAssetsPath + $"/scan1")[0]);
                        }
                        else if (assetFolder.Contains("scan2")) // 꽃
                        {
                            Debug.Log("Imported Flower Image: " + str);
                            GameObject.Find("GameManager").GetComponent<GameManager>().CreateFlower();
                        }
                        else if (assetFolder.Contains("Video"))
                        {
                            Debug.Log("Imported video: " + str);
                            GameObject background = GameObject.Find("Background");
                            if (background != null)
                            {
                                background.GetComponent<BackgroundVideoSetting>().SetVideo();
                            }
                        }
                    }
                }
                // 현재 파일 목록을 이전 파일 목록으로 업데이트
                previousFilesDict[folderPath] = currentFiles;
            }
        }
    }

    List<string> GetFileList(string folderPath)
    {
        // 폴더 내 모든 파일 가져오기
        string[] files = Directory.GetFiles(folderPath);
        return new List<string>(files);
    }

    List<string> GetChangedFiles(List<string> previousFiles, List<string> currentFiles)
    {
        List<string> changedFiles = new List<string>();

        // 현재 파일 목록과 이전 파일 목록을 비교하여 변경된 파일 확인
        foreach (string file in currentFiles)
        {
            if (!previousFiles.Contains(file))
            {
                changedFiles.Add(file);
            }
        }

        return changedFiles;
    }
    /*
    void SortingImage(){
        string[] imageFiles = Directory.GetFiles(Application.streamingAssetsPath + $"/scan1", "*.png"); // 여기서는 .png 확장자 파일만 가져오도록 설정했습니다.
        // 각 이미지 파일 별로 처리
        foreach (string filePath in imageFiles)
        {
            // 파일 이름에 따라 다른 처리 수행
            if (filePath.Contains("bird")) // 예시: 파일 이름에 "bird"가 포함되어 있으면 새 이미지
            {
                ProcessBirdImage(filePath);
            }
            else if (filePath.Contains("flower")) // 예시: 파일 이름에 "flower"가 포함되어 있으면 꽃 이미지
            {
                ProcessFlowerImage(filePath);
            }
            else
            {
                Debug.LogWarning("Unknown image type: " + filePath);
            }
        }
    }

    void ProcessBirdImage(string filePath)
    {
        // 새 이미지 처리 코드
        Debug.Log("Processing bird image: " + filePath);
    }

    void ProcessFlowerImage(string filePath)
    {
        // 꽃 이미지 처리 코드
        Debug.Log("Processing flower image: " + filePath);
    }
    */
}