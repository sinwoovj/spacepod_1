using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnData : MonoBehaviour {
    [SerializeField]
    bool isUsing=false;

    public void SetUsing(bool isusing)
    {
        isUsing = isusing;
    }

    public bool GetUsing()
    {
        return isUsing;
    }
}

