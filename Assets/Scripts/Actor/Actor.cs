using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent NavAgent;

    [SerializeField]
    ActorBody Body;

    [SerializeField]
    Transform BodyContainer;

    public Character CurrentCharacter;


    private void Start()
    {
        if (CurrentCharacter == null) //TODO Remove later, if generates characters twice.
        {
            CurrentCharacter = (Character) ScriptableObject.CreateInstance(typeof(Character));
            CurrentCharacter.Randomize();
            
        }
    }

    private void Update()
    {
        Body.Anim.SetFloat("Velocity", NavAgent.velocity.sqrMagnitude);
    }

    #region Basic

    public void SetCharacter(Character character)
    {
        CurrentCharacter = character;
    }

    void NavigateTo(Vector3 targetPosition)
    {
        NavAgent.SetDestination(targetPosition);
    }

    #endregion

    #region Visuals


    public void RefreshVisuals()
    {
        Destroy(Body);

        SetBody(Instantiate(CurrentCharacter.VisualSet.BodyModel));   
    }

    void SetBody(GameObject newBody)
    {
        Body = newBody.GetComponent<ActorBody>();
        Body.transform.SetParent(BodyContainer);
        Body.transform.position = BodyContainer.position;
        Body.transform.rotation = BodyContainer.rotation;
    }

    #endregion
}
