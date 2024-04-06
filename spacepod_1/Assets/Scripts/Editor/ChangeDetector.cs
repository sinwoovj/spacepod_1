using UnityEngine;
using UnityEditor;
using System.IO;
//using Codice.Client.Commands.Matcher;

class ChangeDetector : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
    {
        string[] image1 = Directory.GetFiles("Assets/Prior_Setting/scan1");
        string[] image2 = Directory.GetFiles("Assets/Prior_Setting/scan2");
        string[] video = Directory.GetFiles("Assets/Prior_Setting/Video");
        
        foreach (string str in importedAssets)
        {

            if (str == "Assets/Prior_Setting/SetValue.xlsx")
            {
                Debug.Log("Reimported Excel Data");
                //DataConverter.ReadFromExcel();
            }
            else if (image1 != null || str == image1[0])
            {
                Debug.Log("Imported image1: " + str);
                ImageSetting.SetImage(1);
            }
            else if (image2 != null || str == image2[0])
            {
                Debug.Log("Imported image2: " + str);
                ImageSetting.SetImage(2);
            }
            else if (str == video[0]) 
            {
                Debug.Log("Imported video: " + str);
                GameObject background = GameObject.Find("Background");
                if (background != null)
                {
                    background.GetComponent<BackgroundVideoSetting>().SetVideo();
                }
            }
            else
            {
                Debug.Log("Reimported Asset: " + str);
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
}