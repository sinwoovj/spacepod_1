using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class TestManager : MonoBehaviour
{
    public ParticleManager particleManager;
    public GameManager gameManager;
    private string pressedKey;
    private int screenSettingNum = 2;
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        if (Input.anyKeyDown)
        {
            pressedKey = Input.inputString;
            switch (pressedKey)
            {
                case "1":
                    // L
                    gameManager.CreateBird(Random.Range(1,3), Directory.EnumerateFiles(Application.streamingAssetsPath + $"/MaskImage", "*.png").ToArray()[0]);
                    break;
                case "2":
                    // R
                    gameManager.CreateFlower(Directory.EnumerateFiles(Application.streamingAssetsPath + $"/MaskImage", "*.png").ToArray()[1]);
                    break;
                case "o":
                    particleManager.PlayParticle(BirdDir.Left, Random.Range(0, 5));
                    break;
                case "p":
                    particleManager.PlayParticle(BirdDir.Right , Random.Range(0, 5));
                    break;
                case "[":
                    if(screenSettingNum < 3)
                        screenSettingNum++;
                        ScreenSetFun(screenSettingNum);
                    break;
                case "]":
                    if (screenSettingNum > 0)
                        screenSettingNum--;
                        ScreenSetFun(screenSettingNum);
                    break;
            }
        }
    }
    public void ScreenSetFun(int num)
    {
        switch (num)
        {
            case 0:
                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, false);
                break;
            case 2:
                Screen.SetResolution(1920, 706, true);
                break;
            case 3:
                Screen.SetResolution(1920, 706, false);
                break;
        }
    }
}
