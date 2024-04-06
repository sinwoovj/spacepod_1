using UnityEngine.Tilemaps;
using UnityEngine;
using System;

public interface IFillFromStr
{
    void FillFromString(string str);
}

public class Pair : IFillFromStr
{
    public int v1, v2;

    public void FillFromString(string str)
    {
        var arr = str.Split(',');


        v1 = int.Parse(arr[0]);
        v2 = int.Parse(arr[1]);
    }
}

public class Range : IFillFromStr
{
    public int min, max;
    public void FillFromString(string str)
    {
        var arr = str.Split(',');

        min = int.Parse(arr[0]);
        max = int.Parse(arr[1]);
    }
}


[System.Serializable]
public class SetValue
{
    public float birdMoveSpeed;
    public float animDuration;
}