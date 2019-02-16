﻿using System;
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

    ProgressBarWorldUI CurrentProgressBar;

    public Character Character;

    public Interaction CurrentInteraction;
    public Coroutine CurrentInteractionRoutineInstance;

    private void Start()
    {
        if (Character == null) //TODO Remove later, if generates characters twice.
        {
            Character = (Character) ScriptableObject.CreateInstance(typeof(Character));
            Character.Randomize();

            SetCharacter(Character);
        }
    }

    private void Update()
    {
        Body.Anim.SetFloat("Velocity", NavAgent.velocity.sqrMagnitude);
    }

    public void SetCharacter(Character character)
    {
        if(Character!=null)
        {
            character.VisualChanged.RemoveListener(RefreshVisuals);
        }

        Character = character;

        character.VisualChanged.AddListener(RefreshVisuals);

        RefreshVisuals();
    }

    public void SetPlayableCharacter()
    {
        if (CORE.Instance == null)
        {
            return;
        }


        Character.Needs.Clear();

        for (int i = 0; i < CORE.Instance.Database.BaseNeeds.Count; i++)
        {
            Character.Needs.Add(new Character.NeedBar(CORE.Instance.Database.BaseNeeds[i]));
        }

        PlayTool.Instance.OnSecondPassedEvent.AddListener(RefreshNeeds);
    }

    void RefreshNeeds()
    {
        for(int i=0;i<Character.Needs.Count;i++)
        {
            Character.Needs[i].Deteriorate();
        }
    }

    #region AI_1

    public void HaltCurrentInteraction()
    {
        if(CurrentInteractionRoutineInstance != null)
        {
            StopCoroutine(CurrentInteractionRoutineInstance);
        }

        Body.Anim.SetTrigger("Interrupt");
        CurrentInteraction = null;

        if (CurrentProgressBar != null)
        {
            CurrentProgressBar.Halt();
        }
    }

    public void NavigateTo(Vector3 targetPosition)
    {
        HaltFacingTarget();
        NavAgent.SetDestination(targetPosition);
    }

    public void HaltFacingTarget()
    {
        if (FaceTargetRoutineInstance != null)
        {
            StopCoroutine(FaceTargetRoutineInstance);
        }

        NavAgent.updateRotation = true;
    }

    public void FaceTarget(Vector3 target)
    {
        HaltFacingTarget();
        FaceTargetRoutineInstance = StartCoroutine(FaceTargetRoutine(target));
    }

    Coroutine FaceTargetRoutineInstance;
    IEnumerator FaceTargetRoutine(Vector3 target)
    {
        NavAgent.updateRotation = false;

        target = new Vector3(target.x, transform.position.y, target.z);

        float t = 0f;
        while(t<1f)
        {
            t += 1f * Time.deltaTime;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position), t);
            
            yield return 0;
        }

        NavAgent.updateRotation = true;
        FaceTargetRoutineInstance = null;
    }

    #endregion

    #region AI_2

    public void WalkTo(Vector3 target)
    {
        HaltCurrentInteraction();

        NavigateTo(target);
    }

    public void Interact(InteractableEntity entity, Interaction currentInteraction)
    {
        HaltCurrentInteraction();

        if (CurrentInteractionRoutineInstance != null)
        {
            StopCoroutine(CurrentInteractionRoutineInstance);
        }

        CurrentInteractionRoutineInstance = StartCoroutine(CurrentInteractionRoutine(entity, currentInteraction));
    }

    private IEnumerator CurrentInteractionRoutine(InteractableEntity entity, Interaction currentInteraction)
    {

        NavigateTo(entity.GetNearestInteractionPosition(transform.position));

        yield return 0;

        while (NavAgent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            yield return 0;
        }

        while (NavAgent.remainingDistance != 0)
        {
            yield return 0;
        }

        FaceTarget(entity.transform.position);

        Body.Anim.SetInteger("InteractionNumber", currentInteraction.InteractionAnimationNumber);
        Body.Anim.SetTrigger("Interact");

        if(currentInteraction.ShowProgressBar)
        {
            CurrentProgressBar = ResourcesLoader.Instance.GetRecycledObject("ProgressBarInstance").GetComponent<ProgressBarWorldUI>();
            CurrentProgressBar.transform.SetParent(PlayModeUI.Instance.transform, false);
            CurrentProgressBar.SetInfo(transform ,currentInteraction.Duration);
        }

        float t = 0f;
        while(t<1f)
        {
            t += (1f/currentInteraction.Duration) * Time.deltaTime;
            
            yield return 0;
        }

        currentInteraction.OnComplete.Invoke();
        
        for(int i=0;i<currentInteraction.OnCompleteEvenets.Count;i++)
        {
            currentInteraction.OnCompleteEvenets[i].ExecuteEvent(this, entity);
        }

        if(currentInteraction.Repeat)
        {
            Interact(entity, currentInteraction);
        }
    }


    #endregion

    #region Visuals


    public void RefreshVisuals()
    {
        if(Character.VisualSet.BodyModel == null)
        {
            Debug.LogError(this.name + " : NO BODY MODEL! ");
            return;
        }

        if (    Body == null 
            || (Body != null && Character.VisualSet.BodyModel.name != Body.gameObject.name))
        {
            if (Body != null)
            {
                Destroy(Body.gameObject);
            }

            Body = Instantiate(Character.VisualSet.BodyModel).GetComponent<ActorBody>();
            Body.transform.SetParent(BodyContainer);
            Body.transform.position = BodyContainer.position;
            Body.transform.rotation = BodyContainer.rotation;
        }

        Material[] newMaterials = new Material[2];
        newMaterials[0] = Character.Face.SetMaterial;
        newMaterials[1] = Character.Clothing.SetMaterial;
        Body.BodyRenderer.materials = newMaterials;


        newMaterials = new Material[2];
        newMaterials[0] = Character.Face.SetMaterial;
        newMaterials[1] = Character.Hair.SetMaterial;
        Body.HeadRenderer.materials = newMaterials;

    }


    #endregion
}
