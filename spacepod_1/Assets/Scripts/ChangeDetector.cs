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
            if (assetFolder.Contains("scan1")) // ���� 
            {
                Debug.Log("Imported Magpie Image: " + str);
                GameObject.Find("GameManager").GetComponent<GameManager>().CreateBird(Random.Range(1, 3));
            }
            if (assetFolder.Contains("scan2")) // ��
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

        // �ʱ� ���� ��� ����
        foreach (string folderPath in foldersToMonitor)
        {
            previousFilesDict[folderPath] = GetFileList(folderPath);
        }

        // ���� ����͸� �ڷ�ƾ ����
        StartCoroutine(MonitorFolderChanges());
    }

    IEnumerator MonitorFolderChanges()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // ������ ����͸��ϴ� ���� ���� (��: 1��)

            // ���� ���� ��� ��������
            foreach (string folderPath in foldersToMonitor)
            {
                // ���� ���� ��� ��������
                List<string> currentFiles = GetFileList(folderPath);

                // ���� ���� ��ϰ� ���Ͽ� ����� ���� Ȯ��
                List<string> changedFiles = GetChangedFiles(previousFilesDict[folderPath], currentFiles);


                // ����� ���� ����� ���� ���
                if (changedFiles.Count > 0)
                {
                    // ����� ���� ��� ��� �Ǵ� ���ϴ� �۾� ����
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
                        else if (assetFolder.Contains("scan1")) // ���� 
                        {
                            Debug.Log("Imported Magpie Image: " + str);
                            GameObject.Find("GameManager").GetComponent<GameManager>().CreateBird(Random.Range(1, 3), Directory.GetFiles(Application.streamingAssetsPath + $"/scan1")[0]);
                        }
                        else if (assetFolder.Contains("scan2")) // ��
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
                // ���� ���� ����� ���� ���� ������� ������Ʈ
                previousFilesDict[folderPath] = currentFiles;
            }
        }
    }

    List<string> GetFileList(string folderPath)
    {
        // ���� �� ��� ���� ��������
        string[] files = Directory.GetFiles(folderPath);
        return new List<string>(files);
    }

    List<string> GetChangedFiles(List<string> previousFiles, List<string> currentFiles)
    {
        List<string> changedFiles = new List<string>();

        // ���� ���� ��ϰ� ���� ���� ����� ���Ͽ� ����� ���� Ȯ��
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
        string[] imageFiles = Directory.GetFiles(Application.streamingAssetsPath + $"/scan1", "*.png"); // ���⼭�� .png Ȯ���� ���ϸ� ���������� �����߽��ϴ�.
        // �� �̹��� ���� ���� ó��
        foreach (string filePath in imageFiles)
        {
            // ���� �̸��� ���� �ٸ� ó�� ����
            if (filePath.Contains("bird")) // ����: ���� �̸��� "bird"�� ���ԵǾ� ������ �� �̹���
            {
                ProcessBirdImage(filePath);
            }
            else if (filePath.Contains("flower")) // ����: ���� �̸��� "flower"�� ���ԵǾ� ������ �� �̹���
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
        // �� �̹��� ó�� �ڵ�
        Debug.Log("Processing bird image: " + filePath);
    }

    void ProcessFlowerImage(string filePath)
    {
        // �� �̹��� ó�� �ڵ�
        Debug.Log("Processing flower image: " + filePath);
    }
    */
}