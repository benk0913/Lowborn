using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoveringGainUI : MonoBehaviour
{
    public TextMeshProUGUI GainText;
    public Image IconImage;
    public Image BGImage;
    public CanvasGroup CG;

    public void SetInfo(Sprite icon, int Value)
    {
        GainText.text = Value > 0 ? ( "+" + Value ) : ( Value.ToString() );
        BGImage.color = Value > 0 ? (Color.green)   : (Color.red);

        IconImage.sprite = icon;

    }

    public void SetInfo(Sprite icon, float Value)
    {
        GainText.text = Value > 0 ? ("+" + Mathf.FloorToInt(Value*100f) +"%") : (Mathf.FloorToInt(Value * 100f) + "%");
        BGImage.color = Value > 0 ? (Color.green) : (Color.red);

        IconImage.sprite = icon;
    }

    private void OnEnable()
    {
        StartCoroutine(DisplayRoutine());
    }


    IEnumerator DisplayRoutine()
    {
        float t = 0f;
        while(t<1f)
        {
            t += 6f * Time.unscaledDeltaTime;

            CG.alpha = t;

            transform.position += transform.up * 100 * Time.unscaledDeltaTime;

            yield return 0;
        }

        yield return new WaitForSecondsRealtime(0.1f);

        t = 1f;
        while (t > 0f)
        {
            t -= 3f * Time.unscaledDeltaTime;

            transform.position += transform.up * 50f  * Time.unscaledDeltaTime;

            CG.alpha = t;

            yield return 0;
        }

        this.gameObject.SetActive(false);
    }

}
