using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayTool : MonoBehaviour
{
    public static PlayTool Instance;

    public DateTime CurrentTime;
    public float TimeSpeed = 1f;
    Coroutine TimeRoutineInstance;
    public UnityEvent OnSecondPassedEvent = new UnityEvent();

    public bool isToolActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(!isToolActive)
        {
            return;
        }

        if(Input.GetMouseButtonDown(1))
        {
            if (LocationMap.Instance.StructureMouseHit.collider != null)
            {
                LocationMap.Instance.Data.PlayerActor.NavigateTo(LocationMap.Instance.GroundMouseHit.point);
            }
            else if (LocationMap.Instance.GroundMouseHit.collider != null)
            {
                LocationMap.Instance.Data.PlayerActor.NavigateTo(LocationMap.Instance.GroundMouseHit.point);
            }
        }
    }


    public void ActivateTool()
    {
        if (isToolActive)
        {
            return;
        }

        isToolActive = true;

        ResumeTime();
    }

    public void DeactivateTool()
    {
        if (!isToolActive)
        {
            return;
        }

        isToolActive = false;

        PauseTime();
    }

    #region Time;

    public void SetDate(DateTime date)
    {
        this.CurrentTime = date;
    }

    public void PauseTime()
    {
        Time.timeScale = 0f;

        if (TimeRoutineInstance != null)
        {
            StopCoroutine(TimeRoutineInstance);
        }
    }

    public void ResumeTime()
    {
        Time.timeScale = TimeSpeed;

        if(TimeRoutineInstance != null)
        {
            StopCoroutine(TimeRoutineInstance);
        }

        TimeRoutineInstance = StartCoroutine(TimeRoutine());
    }

    IEnumerator TimeRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            CurrentTime = CurrentTime.AddSeconds(1);
            OnSecondPassedEvent.Invoke();
        }
    }

    #endregion
}
