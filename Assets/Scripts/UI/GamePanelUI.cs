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

    [SerializeField]
    GameObject BuildTool;
    [SerializeField]
    GameObject PlayTool;

    public static GamePanelUI Instance;

    public GameTool CurrentGameTool
    {
        get
        {
            return currentGameTool;
        }
        set
        {
            currentGameTool = value;

            switch(currentGameTool)
            {
                case GameTool.Build:
                    {
                        BuildTool.gameObject.SetActive(true);
                        PlayTool.gameObject.SetActive(false);

                        PlayToolButton.gameObject.SetActive(true);
                        BuildToolButton.gameObject.SetActive(false);
                        break;
                    }
                case GameTool.Play:
                    {
                        BuildTool.gameObject.SetActive(false);
                        PlayTool.gameObject.SetActive(true);

                        PlayToolButton.gameObject.SetActive(false);
                        BuildToolButton.gameObject.SetActive(true);
                        break;
                    }
            }
        }
    }
    GameTool currentGameTool;

    private void Awake()
    {
        Instance = this;

        CurrentGameTool = GameTool.Play;
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

    public void SetGameTool(int tool)
    {
        CurrentGameTool = (GameTool)tool;
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


    public enum GameTool
    {
        Build,
        Play
    }
}
