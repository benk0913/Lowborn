using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterInfoUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Name;
    [SerializeField]
    TextMeshProUGUI Gender;
    [SerializeField]
    TextMeshProUGUI Age;
    [SerializeField]
    PortraitUI Portrait;

    Character CurrentCharacter;

    public void OnMouseDrag()
    {
        transform.position += Input.mousePosition;
    }

    
    public void Show(Character character)
    {
        this.gameObject.SetActive(true);

        CurrentCharacter = character;

        Portrait.SetCharacter(character);

        Name.text = character.name;
        Gender.text = "Gender: "+character.Gender.ToString();
        Age.text = "Age: " + character.Age + "(" + character.AgeType.ToString() + ")";

    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
