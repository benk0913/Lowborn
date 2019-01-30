using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTool : MonoBehaviour
{
    public static PlayTool Instance;

    private void Awake()
    {
        Instance = this;
    }
}
