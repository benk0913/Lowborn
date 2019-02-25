using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventZoneEntity : MonoBehaviour
{
    [SerializeField]
    UnityEvent OnEnter;

    [SerializeField]
    UnityEvent OnExit;

    [SerializeField]
    string Tag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Tag)
        {
            OnEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == Tag)
        {
            OnExit.Invoke();
        }
    }
}
