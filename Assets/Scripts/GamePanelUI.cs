using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI FloorText;

    public static GamePanelUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetFloorText(string sText)
    {
        FloorText.text = sText;
    }

    public void ViewUpperFloor()
    {
        CORE.Instance.ViewFloor++;
    }

    public void ViewLowerFloor()
    {
        CORE.Instance.ViewFloor--;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ViewUpperFloor();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            ViewLowerFloor();
        }
    }
}
