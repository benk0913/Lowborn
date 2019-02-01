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

    public void RefreshVisuals()
    {
        Face.sprite = cCharacter.FaceSprite;
        Hair.sprite = cCharacter.HairSprite;
        Clothing.sprite = cCharacter.ClothingSprite;
    }
}
