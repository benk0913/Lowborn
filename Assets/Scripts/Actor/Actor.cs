using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent NavAgent;

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            NavigateTo(CORE.Instance.GroundMouseHit.point);
        }
    }

    #region Basic

    void NavigateTo(Vector3 targetPosition)
    {
        NavAgent.SetDestination(targetPosition);
    }

    #endregion
}
