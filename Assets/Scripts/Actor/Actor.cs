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

            SetCharacter(CurrentCharacter);
        }
    }

    private void Update()
    {
        Body.Anim.SetFloat("Velocity", NavAgent.velocity.sqrMagnitude);
    }

    #region Basic

    public void SetCharacter(Character character)
    {
        if(CurrentCharacter!=null)
        {
            character.VisualChanged.RemoveListener(RefreshVisuals);
        }

        CurrentCharacter = character;

        character.VisualChanged.AddListener(RefreshVisuals);

        RefreshVisuals();
    }

    public void NavigateTo(Vector3 targetPosition)
    {
        NavAgent.SetDestination(targetPosition);
    }

    #endregion

    #region Visuals


    public void RefreshVisuals()
    {
        if(CurrentCharacter.VisualSet.BodyModel == null)
        {
            Debug.LogError(this.name + " : NO BODY MODEL! ");
            return;
        }

        if (    Body == null 
            || (Body != null && CurrentCharacter.VisualSet.BodyModel.name != Body.gameObject.name))
        {
            if (Body != null)
            {
                Destroy(Body.gameObject);
            }

            Body = Instantiate(CurrentCharacter.VisualSet.BodyModel).GetComponent<ActorBody>();
            Body.transform.SetParent(BodyContainer);
            Body.transform.position = BodyContainer.position;
            Body.transform.rotation = BodyContainer.rotation;
        }

        Material[] newMaterials = new Material[2];
        newMaterials[0] = CurrentCharacter.Face.SetMaterial;
        newMaterials[1] = CurrentCharacter.Clothing.SetMaterial;
        Body.BodyRenderer.materials = newMaterials;


        newMaterials = new Material[2];
        newMaterials[0] = CurrentCharacter.Face.SetMaterial;
        newMaterials[1] = CurrentCharacter.Hair.SetMaterial;
        Body.HeadRenderer.materials = newMaterials;

    }

    #endregion
}
