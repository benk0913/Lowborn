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
    TextMeshProUGUI AgeName;

    [SerializeField]
    TextMeshProUGUI ClothingName;

    Character CurrentCharacter;

    private void Awake()
    {
        CurrentCharacter = (Character)ScriptableObject.CreateInstance(typeof(Character));

        Portrait.SetCharacter(CurrentCharacter);
        ActorObject.SetCharacter(CurrentCharacter);

        CurrentCharacter.Age = 20;
        SetGender(0);
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
        
    }

    public void SetName()
    {
        CurrentCharacter.name = NameField.text;
    }

    public void NextSkinColor()
    {
        CurrentCharacter.SkinColor = CurrentCharacter.VisualSet.SkinColors.GetNext(CurrentCharacter.SkinColor);
        
        SkinColorName.text = CurrentCharacter.SkinColor.name;
        FaceName.text = CurrentCharacter.Face.name;
    }

    public void PreviousSkinColor()
    {
        CurrentCharacter.SkinColor = CurrentCharacter.VisualSet.SkinColors.GetPrevious(CurrentCharacter.SkinColor);

        SkinColorName.text = CurrentCharacter.SkinColor.name;
        FaceName.text = CurrentCharacter.Face.name;
    }

    public void NextFace()
    {
        CurrentCharacter.Face = CurrentCharacter.SkinColor.GetNext(CurrentCharacter.Face);
        
        FaceName.text = CurrentCharacter.Face.name;
    }

    public void PreviousFace()
    {
        CurrentCharacter.Face = CurrentCharacter.SkinColor.GetPrevious(CurrentCharacter.Face);
        
        FaceName.text = CurrentCharacter.Face.name;
    }

    public void NextHairColor()
    {
        CurrentCharacter.HairColor = CurrentCharacter.VisualSet.HairColor.GetNext(CurrentCharacter.HairColor);
        
        HairColorName.text = CurrentCharacter.HairColor.name;
        HairName.text = CurrentCharacter.Hair.name;
    }

    public void PreviousHairColor()
    {
        CurrentCharacter.HairColor = CurrentCharacter.VisualSet.HairColor.GetPrevious(CurrentCharacter.HairColor);
        
        HairColorName.text = CurrentCharacter.HairColor.name;
        HairName.text = CurrentCharacter.Hair.name;
    }

    public void NextHairStyle()
    {
        CurrentCharacter.Hair = CurrentCharacter.HairColor.GetNext(CurrentCharacter.Hair);
        
        HairName.text = CurrentCharacter.Hair.name;
    }

    public void PreviousHairStyle()
    {
        CurrentCharacter.Hair = CurrentCharacter.HairColor.GetPrevious(CurrentCharacter.Hair);
        
        HairName.text = CurrentCharacter.Hair.name;
    }


    //TODO Those below are not configurable.
    public void NextAge()
    {
        CurrentCharacter.Age++;

        AgeName.text = CurrentCharacter.Age.ToString();
    }

    public void PreviousAge()
    {
        CurrentCharacter.Age--;
        
        AgeName.text = CurrentCharacter.Age.ToString();
    }
    
    public void NextClothing()
    {
        CurrentCharacter.Clothing = CurrentCharacter.VisualSet.Clothing.GetNext(CurrentCharacter.Clothing);
        
        ClothingName.text = CurrentCharacter.Clothing.name;
    }

    public void PreviousClothing()
    {
        CurrentCharacter.Clothing = CurrentCharacter.VisualSet.Clothing.GetPrevious(CurrentCharacter.Clothing);
        
        ClothingName.text = CurrentCharacter.Clothing.name;
    }


    public void SelectCharacter()
    {
        MainMenuUI.Instance.SelectCharacter(CurrentCharacter);
    }
}
