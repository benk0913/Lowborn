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

    public Character CurrentCharacter;

    private void Start()
    {
        if(CurrentCharacter == null) //TODO Remove later, if generates characters twice.
        {
            CurrentCharacter = (Character) ScriptableObject.CreateInstance(typeof(Character));
            CurrentCharacter.Randomize();
        }
    }

    private void Update()
    {

        Anim.SetFloat("Velocity", NavAgent.velocity.sqrMagnitude);
    }

    #region Basic

    void NavigateTo(Vector3 targetPosition)
    {
        NavAgent.SetDestination(targetPosition);
    }

    #endregion
}
