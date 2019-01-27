using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name;

    public GenderType Gender;
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
    int age;
    
    public Sprite Body;
    public Sprite Face;
    public Sprite Hair;



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
}
