using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarWorldUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI DurationText;

    [SerializeField]
    Image FillImage;

    public void SetInfo(Transform stickTo = null ,float Duration = 1f)
    {
        transform.position = Camera.main.WorldToScreenPoint(stickTo.position);

        if (ProgressRoutineInstance != null)
        {
            StopCoroutine(ProgressRoutineInstance);
        }

        ProgressRoutineInstance = StartCoroutine(ProgressRoutine(stickTo, Duration));
    }

    public void Halt()
    {
        this.gameObject.SetActive(false);
    }

    Coroutine ProgressRoutineInstance;
    IEnumerator ProgressRoutine(Transform stickTo, float Duration)
    {
        float t = 0f;
        while(t<1f)
        {
            t += (1f / Duration) * Time.deltaTime;

            FillImage.fillAmount = t;
            DurationText.text = Mathf.FloorToInt(Mathf.Lerp(Duration, 0f, t)).ToString();

            if(stickTo != null)
            {
                transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(stickTo.position), Time.deltaTime * 3f);
            }

            yield return 0;
        }

        ProgressRoutineInstance = null;

        this.gameObject.SetActive(false);
    }
}
