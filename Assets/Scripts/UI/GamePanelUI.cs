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

    [SerializeField]
    Button PlayToolButton;
    [SerializeField]
    Button BuildToolButton;

    //TODO Might be a good idea to move these panels to CORE object.
    [SerializeField]
    GameObject BuildToolPanel;
    [SerializeField]
    GameObject PlayToolPanel;

    public static GamePanelUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetGameTool((int)GameTool.Play);
    }

    public void SetFloorText(string sText)
    {
        FloorText.text = sText;
    }

    public void ViewUpperFloor()
    {
        LocationMap.Instance.ViewFloor++;
    }

    public void ViewLowerFloor()
    {
        LocationMap.Instance.ViewFloor--;
    }

    public void SetViewMode(ViewMode Mode)
    {
        LocationMap.Instance.CurrentViewMode = Mode;

        switch(LocationMap.Instance.CurrentViewMode)
        {
            case ViewMode.Outside:
                {
                    ViewModeOutsideImg.color = Color.green;
                    ViewModeWallsImg.color = Color.green;
                    ViewModeHideWallImg.color = Color.green;
                    break;
                }
            case ViewMode.Walls:
                {
                    ViewModeOutsideImg.color = Color.white;
                    ViewModeWallsImg.color = Color.green;
                    ViewModeHideWallImg.color = Color.green;
                    break;
                }
            case ViewMode.HideWalls:
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
        SetViewMode((ViewMode)Mode);
    }

    public void SetGameTool(int tool)
    {
        LocationMap.Instance.CurrentGameTool = (GameTool)tool;

        switch (LocationMap.Instance.CurrentGameTool)
        {
            case GameTool.Build:
                {
                    BuildToolPanel.gameObject.SetActive(true);
                    PlayToolPanel.gameObject.SetActive(false);

                    PlayToolButton.gameObject.SetActive(true);
                    BuildToolButton.gameObject.SetActive(false);

                    BuildingTool.Instance.ActivateTool();
                    PlayTool.Instance.DeactivateTool();
                    break;
                }
            case GameTool.Play:
                {
                    BuildToolPanel.gameObject.SetActive(false);
                    PlayToolPanel.gameObject.SetActive(true);

                    PlayToolButton.gameObject.SetActive(false);
                    BuildToolButton.gameObject.SetActive(true);

                    BuildingTool.Instance.DeactivateTool();
                    PlayTool.Instance.ActivateTool();
                    break;
                }
        }
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
