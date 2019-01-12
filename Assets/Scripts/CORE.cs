using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CORE : MonoBehaviour
{
    [SerializeField]
    GameObject GamePanel;

    public bool IsMouseOnUI { private set; get; }

    public static CORE Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetIsMouseOnUI(bool state)
    {
        IsMouseOnUI = state;
    }
}
