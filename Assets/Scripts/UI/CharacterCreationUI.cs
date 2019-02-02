using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI NameField;

    [SerializeField]
    Button MaleButton;

    [SerializeField]
    Button FemaleButton;

    [SerializeField]
    PortraitUI Portrait;

    [SerializeField]
    Actor ActorObject;

    [SerializeField]
    TextMeshProUGUI SkinColorName;

    [SerializeField]
    TextMeshProUGUI FaceName;

    [SerializeField]
    TextMeshProUGUI HairColorName;

    [SerializeField]
    TextMeshProUGUI HairName;

    [SerializeField]
    TextMeshProUGUI ClothingName;

    Character CurrentCharacter;

    private void Awake()
    {
        CurrentCharacter = (Character)ScriptableObject.CreateInstance(typeof(Character));

        Portrait.Setup(CurrentCharacter);
        ActorObject.SetCharacter(CurrentCharacter);

        SetGender(1);
    }

    public void SetGender(int Gender)
    {
        CurrentCharacter.Gender = (GenderType)Gender;

        if(CurrentCharacter.Gender == GenderType.Male)
        {
            MaleButton.interactable = false;
            FemaleButton.interactable = true;
        }
        else
        {
            MaleButton.interactable = true;
            FemaleButton.interactable = false;
        }

        RefreshVisual();
    }

    public void SetName()
    {
        CurrentCharacter.name = NameField.text;
    }

    public void NextSkinColor()
    {
        CurrentCharacter.SkinColor = CurrentCharacter.VisualSet.SkinColors.GetNext(CurrentCharacter.SkinColor);

        RefreshVisual();
        SkinColorName.text = CurrentCharacter.SkinColor.name;
        FaceName.text = CurrentCharacter.Face.name;
    }

    public void PreviousSkinColor()
    {
        CurrentCharacter.SkinColor = CurrentCharacter.VisualSet.SkinColors.GetPrevious(CurrentCharacter.SkinColor);

        RefreshVisual();
        SkinColorName.text = CurrentCharacter.SkinColor.name;
        FaceName.text = CurrentCharacter.Face.name;
    }

    public void NextFace()
    {
        CurrentCharacter.Face = CurrentCharacter.SkinColor.GetNext(CurrentCharacter.Face);

        RefreshVisual();
        FaceName.text = CurrentCharacter.Face.name;
    }

    public void PreviousFace()
    {
        CurrentCharacter.Face = CurrentCharacter.SkinColor.GetPrevious(CurrentCharacter.Face);

        RefreshVisual();
        FaceName.text = CurrentCharacter.Face.name;
    }

    public void NextHairColor()
    {
        CurrentCharacter.HairColor = CurrentCharacter.VisualSet.HairColor.GetNext(CurrentCharacter.HairColor);

        RefreshVisual();
        HairColorName.text = CurrentCharacter.HairColor.name;
        HairName.text = CurrentCharacter.Hair.name;
    }

    public void PreviousHairColor()
    {
        CurrentCharacter.HairColor = CurrentCharacter.VisualSet.HairColor.GetPrevious(CurrentCharacter.HairColor);

        RefreshVisual();
        HairColorName.text = CurrentCharacter.HairColor.name;
        HairName.text = CurrentCharacter.Hair.name;
    }

    public void NextHairStyle()
    {
        CurrentCharacter.Hair = CurrentCharacter.HairColor.GetNext(CurrentCharacter.Hair);

        RefreshVisual();
        HairName.text = CurrentCharacter.Hair.name;
    }

    public void PreviousHairStyle()
    {
        CurrentCharacter.Hair = CurrentCharacter.HairColor.GetPrevious(CurrentCharacter.Hair);

        RefreshVisual();
        HairName.text = CurrentCharacter.Hair.name;
    }


    //TODO Remove Later (Should not be selected on char creation)
    public void NextClothing()
    {
        CurrentCharacter.Clothing = CurrentCharacter.VisualSet.Clothing.GetNext(CurrentCharacter.Clothing);

        RefreshVisual();
        ClothingName.text = CurrentCharacter.Clothing.name;
    }

    public void PreviousClothing()
    {
        CurrentCharacter.Clothing = CurrentCharacter.VisualSet.Clothing.GetPrevious(CurrentCharacter.Clothing);

        RefreshVisual();
        ClothingName.text = CurrentCharacter.Clothing.name;
    }


    void RefreshVisual()
    {
        Portrait.RefreshVisuals();
        ActorObject.RefreshVisuals();
    }


}
