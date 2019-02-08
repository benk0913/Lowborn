using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitUI : MonoBehaviour
{
    [SerializeField]
    public Character CurrentCharacter;

    [SerializeField]
    Image Face;

    [SerializeField]
    Image Hair;

    [SerializeField]
    Image Clothing;

    private void Awake()
    {
        if(CurrentCharacter != null)
        {
            SetCharacter(CurrentCharacter);
        }
    }

    public void SetCharacter(Character character)
    {
        if(CurrentCharacter != null)
        {
            character.VisualChanged.RemoveListener(RefreshVisuals);
        }

        CurrentCharacter = character;
        RefreshVisuals();

        character.VisualChanged.AddListener(RefreshVisuals);
    }

    public void RefreshVisuals()
    {
        Face.sprite = CurrentCharacter.Face.Sprite;
        Hair.sprite = CurrentCharacter.Hair.Sprite;
        Clothing.sprite = CurrentCharacter.Clothing.Sprite;
    }
}
