using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableEntity : MonoBehaviour
{
    [SerializeField]
    List<Transform> InteractionPoints = new List<Transform>();


    public List<InteractionContainer> PossibleInteractions = new List<InteractionContainer>();

    public Transform GetNearestInteractionPoint(Vector3 from)
    {
        if(InteractionPoints.Count == 0)
        {
            return transform;
        }

        float shortestDistance = Mathf.Infinity;
        Transform bestPoint = null;
        float tempDistance = 0f;
        for(int i=0;i<InteractionPoints.Count;i++)
        {
            tempDistance = Vector3.Distance(InteractionPoints[i].position, from);
            if (Vector3.Distance(InteractionPoints[i].position, from) < shortestDistance)
            {
                shortestDistance = tempDistance;
                bestPoint = InteractionPoints[i];
            }
        }

        return bestPoint;
    }

    public Vector3 GetNearestInteractionPosition(Vector3 from)
    {
        return GetNearestInteractionPoint(from).position;
    }

    private void Start()
    {
        for(int i=0;i<PossibleInteractions.Count;i++)
        {
            PossibleInteractions[i]._Interaction.OnComplete.AddListener(PossibleInteractions[i].OnInteractionComplete.Invoke);
        }
    }

    [System.Serializable]
    public class InteractionContainer
    {
        [SerializeField]
        public Interaction _Interaction;

        [SerializeField]
        public UnityEvent OnInteractionComplete;
    }
}
