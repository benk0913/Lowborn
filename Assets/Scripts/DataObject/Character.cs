using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "DataObjects/Character", order = 2)]
public class Character : ScriptableObject
{
    public string ID;
    public GenderSet VisualSet;


    public GenderType Gender
    {
        get
        {
            return gender;
        }
        set
        {
            if(gender == value)
            {
                return;
            }

            gender = value;

            RefreshVisualTree();
        }
    }
    GenderType gender = GenderType.Female;

    public int Age
    {
        set
        {
            if(age == value)
            {
                return;
            }

            age = value;

            if(age < 4)
            {
                this.AgeType = AgeTypeEnum.Baby;
                RefreshVisualTree();
            }
            else if (age < 15)
            {
                this.AgeType = AgeTypeEnum.Child;
                RefreshVisualTree();
            }
            else if (age < 55)
            {
                this.AgeType = AgeTypeEnum.Adult;
                RefreshVisualTree();
            }
            else
            {
                this.AgeType = AgeTypeEnum.Old;
                RefreshVisualTree();
            }
        }
        get
        {
            return age;
        }
    }
    int age = 16;

    public AgeTypeEnum AgeType { private set; get; }

    public VisualCharacteristic SkinColor
    {
        get
        {
            return skinColor;
        }
        set
        {
            if (skinColor == value)
                return;

            skinColor = value;

            Face = skinColor.GetVCByName(Face.name);
        }
    }
    VisualCharacteristic skinColor;

    public VisualCharacteristic HairColor
    {
        get
        {
            return hairColor;
        }
        set
        {
            if(hairColor == value)
                return;

            hairColor = value;

            Hair = hairColor.GetVCByName(Hair.name);
        }
    }
    VisualCharacteristic hairColor;

    public VisualCharacteristic Face;
    public VisualCharacteristic Hair;
    public VisualCharacteristic Clothing;
   

    public Character()
    {
        this.ID = Util.GenerateUniqueID();
    }

    private void OnEnable()
    {
        InitializeLooks();
    }

    public void Randomize()
    {
        this.name = "Random Name";
        this.Gender = (GenderType) Random.Range(0, 2);
        this.Age = Random.Range(5, 50);
    }

    public void RefreshVisualTree()
    {
        //TODO Get rid of this when we have enough data.
        RaceSet raceSet = CORE.Instance.Database.GetRace("Human");

        AgeSet ageSet = CORE.Instance.Database.GetRace("Human").GetAgeSet(AgeTypeEnum.Adult);

        if (ageSet == null)
        {
            Debug.LogError("NO AGE SET! " + AgeTypeEnum.Adult.ToString());
            return;
        }


        VisualSet = (gender == GenderType.Male) ?
            CORE.Instance.Database.GetRace("Human").GetAgeSet(AgeType).Male : CORE.Instance.Database.GetRace("Human").GetAgeSet(AgeType).Female;
        

        if (VisualSet == null)
        {
            Debug.LogError("NO VISUAL SET! " + age + " | "  + gender.ToString() + " | " + AgeTypeEnum.Adult.ToString());
            return;
        }


        SkinColor = VisualSet.SkinColors.GetVCByName(SkinColor.name);

        HairColor = VisualSet.HairColor.GetVCByName(HairColor.name);

        Clothing = VisualSet.Clothing.GetVCByName(Clothing.name);
    }

    void InitializeLooks()
    {
        //TODO Get rid of this when we have enough data.
        RaceSet raceSet = CORE.Instance.Database.GetRace("Human");

        AgeSet ageSet = CORE.Instance.Database.GetRace("Human").GetAgeSet(AgeTypeEnum.Adult);

        if(ageSet == null)
        {
            Debug.LogError("NO AGE SET! "+AgeTypeEnum.Adult.ToString());
            return;
        }

        VisualSet = Gender == GenderType.Male? 
            CORE.Instance.Database.GetRace("Human").GetAgeSet(AgeTypeEnum.Adult).Male : 
            CORE.Instance.Database.GetRace("Human").GetAgeSet(AgeTypeEnum.Adult).Female;

        if (VisualSet == null)
        {
            Debug.LogError("NO VISUAL SET! " + AgeTypeEnum.Adult.ToString());
            return;
        }

        skinColor = VisualSet.SkinColors.Pool[0];
        hairColor = VisualSet.HairColor.Pool[0];

        Face = skinColor.Pool[0];
        Hair = hairColor.Pool[0];
        
        //TODO Add item support
        Clothing = VisualSet.Clothing.Pool[0];

        RefreshVisualTree();
    }
}
