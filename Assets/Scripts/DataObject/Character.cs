using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "DataObjects/Character", order = 2)]
public class Character : ScriptableObject
{
    public string ID;

    public GenderType Gender = GenderType.Male;
    public AgeTypeEnum AgeType { private set; get; }

    public int Age
    {
        set
        {
            age = value;

            if(age < 4)
            {
                this.AgeType = AgeTypeEnum.Baby;
            }
            else if (age < 15)
            {
                this.AgeType = AgeTypeEnum.Child;
            }
            else if (age < 55)
            {
                this.AgeType = AgeTypeEnum.Adult;
            }
            else
            {
                this.AgeType = AgeTypeEnum.Old;
            }
        }
        get
        {
            return age;
        }
    }
    int age = 16;


    public VCSet VisualSet;
    public int HairColor;
    public int Hair;
    public int SkinColor;
    public int Face;
    public int Clothing;

    public Sprite HairSprite
    {
        get
        {
            return VisualSet.HairColor[HairColor].Pool[Hair].SetSprite;
        }
    }

    public Sprite FaceSprite
    {
        get
        {
            return VisualSet.SkinColor[SkinColor].Pool[Face].SetSprite;
        }
    }

    public Sprite ClothingSprite
    {
        get
        {
            return VisualSet.SkinColor[0].Pool[Clothing].SetSprite;
        }
    }



    public Character Heir = null;
    
    


    public enum GenderType
    {
        Male,
        Female
    }

    public enum AgeTypeEnum
    {
        Baby,
        Child,
        Adult,
        Old
    }

    public Character()
    {
        this.ID = Util.GenerateUniqueID();
    }

    public void Randomize()
    {
        this.name = "Random Name";
        this.Gender = (GenderType) Random.Range(0, 2);
        this.Age = Random.Range(5, 50);
    }
}
