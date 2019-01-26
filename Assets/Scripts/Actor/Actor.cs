using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent NavAgent;

    [SerializeField]
    Animator Anim;

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            NavigateTo(CORE.Instance.GroundMouseHit.point);
        }

        Anim.SetFloat("Velocity", NavAgent.velocity.sqrMagnitude);
    }

    #region Basic

    void NavigateTo(Vector3 targetPosition)
    {
        NavAgent.SetDestination(targetPosition);
    }

    #endregion
}
