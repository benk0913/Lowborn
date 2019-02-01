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

    Character CurrentCharacter;

    private void Awake()
    {
        CurrentCharacter = (Character)ScriptableObject.CreateInstance(typeof(Character));

        SetGender(0);
    }

    public void SetGender(int Gender)
    {
        CurrentCharacter.Gender = (Character.GenderType)Gender;

        if(CurrentCharacter.Gender == Character.GenderType.Male)
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
}
