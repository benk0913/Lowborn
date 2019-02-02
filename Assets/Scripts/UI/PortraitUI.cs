using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitUI : MonoBehaviour
{
    [SerializeField]
    public Character cCharacter;

    [SerializeField]
    Image Face;

    [SerializeField]
    Image Hair;

    [SerializeField]
    Image Clothing;

    public void Setup(Character character)
    {
        this.cCharacter = character;
        RefreshVisuals();
    }

    public void RefreshVisuals()
    {
        Face.sprite = cCharacter.Face.Sprite;
        Hair.sprite = cCharacter.Hair.Sprite;
        Clothing.sprite = cCharacter.Clothing.Sprite;
    }
}
