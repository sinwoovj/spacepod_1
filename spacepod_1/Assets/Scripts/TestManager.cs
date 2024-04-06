using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public ParticleManager particleManager;
    public GameManager gameManager;
    private string pressedKey;
    private int screenSettingNum = 2;
    void Update()
    {
        if (Input.anyKeyDown)
        {
            pressedKey = Input.inputString;
            switch (pressedKey)
            {
                case "1":
                    // L
                    gameManager.CreateBird(1);
                    break;
                case "2":
                    // R
                    gameManager.CreateBird(2);
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
