using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI FloorText;

    [SerializeField]
    Image ViewModeOutsideImg;
    [SerializeField]
    Image ViewModeWallsImg;
    [SerializeField]
    Image ViewModeHideWallImg;

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

    public void SetViewMode(CORE.ViewMode Mode)
    {
        CORE.Instance.CurrentViewMode = Mode;

        switch(CORE.Instance.CurrentViewMode)
        {
            case CORE.ViewMode.Outside:
                {
                    ViewModeOutsideImg.color = Color.green;
                    ViewModeWallsImg.color = Color.green;
                    ViewModeHideWallImg.color = Color.green;
                    break;
                }
            case CORE.ViewMode.Walls:
                {
                    ViewModeOutsideImg.color = Color.white;
                    ViewModeWallsImg.color = Color.green;
                    ViewModeHideWallImg.color = Color.green;
                    break;
                }
            case CORE.ViewMode.HideWalls:
                {
                    ViewModeOutsideImg.color = Color.white;
                    ViewModeWallsImg.color = Color.white;
                    ViewModeHideWallImg.color = Color.green;
                    break;
                }
        }
    }
    
    public void SetViewMode(int Mode)
    {
        SetViewMode((CORE.ViewMode)Mode);
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
