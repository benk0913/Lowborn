using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableEntity : MonoBehaviour
{
    [SerializeField]
    StructureProp Prop;

    [SerializeField]
    InteractableEntity InteractableEntity;

    [SerializeField]
    Animator Anim;

    [SerializeField]
    float TimeToDestruct = 1f;

    public void StartDestruction()
    {
        if(InteractableEntity != null)
        {
            InteractableEntity.enabled = false;
        }
       
        Anim.SetTrigger("Destruct");
    }

    void Destruct()
    {
        if (Prop != null)
        {
            Prop.RemoveStructureProp();
        }

        Destroy(this.gameObject);
    }


}
