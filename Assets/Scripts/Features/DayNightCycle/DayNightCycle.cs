using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    private Transform _lightSourceTransform;
    private bool _rotating;

    private void Start()
    {
        PlayTool.Instance.OnSecondPassedEvent.AddListener(RotateLightSource);
       
    }

    private void RotateLightSource()
    {
        if (!_rotating)
            StartCoroutine(Rotate(new Vector3(1, 0, 0), 1));
    }

    private IEnumerator Rotate(Vector3 angles, float duration)
    {
        _rotating = true;
        Quaternion startRotation = _lightSourceTransform.rotation;
        Quaternion endRotation = Quaternion.Euler(angles) * startRotation;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            _lightSourceTransform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }
        _lightSourceTransform.rotation = endRotation;
        _rotating = false;
    }
}
