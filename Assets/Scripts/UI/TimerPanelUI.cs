using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerPanelUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI DateText;

    [SerializeField]
    TextMeshProUGUI TimeText;

    [SerializeField]
    Button PauseButton;

    [SerializeField]
    Button PlayButton;

    [SerializeField]
    Button SpeedButton;

    private void Start()
    {
        PlayTool.Instance.OnSecondPassedEvent.AddListener(UpdateUI);   
    }

    public void SetPlayTime(float time)
    {

        PlayTool.Instance.PauseTime();

        if (time == 0)
        {
            PauseButton.interactable = false;
            PlayButton.interactable = true;
            SpeedButton.interactable = true;

            return;
        }

        if(time == 1f)
        {
            PauseButton.interactable = true;
            PlayButton.interactable = false;
            SpeedButton.interactable = true;
        }
        else
        {
            PauseButton.interactable = true;
            PlayButton.interactable = true;
            SpeedButton.interactable = false;
        }

        PlayTool.Instance.TimeSpeed = time;
        PlayTool.Instance.ResumeTime();
    }

    void UpdateUI()
    {
        DateText.text = PlayTool.Instance.CurrentTime.ToString("dd/MM/yyyy");
        TimeText.text = PlayTool.Instance.CurrentTime.ToString("hh:mm");
    }
    

}
