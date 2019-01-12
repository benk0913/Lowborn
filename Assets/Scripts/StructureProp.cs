using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureProp : MonoBehaviour
{
    [SerializeField]
    public StructureData Data;

    [System.NonSerialized]
    public List<Vector2> OccupiedSMAPs = new List<Vector2>();

    public int Floor;
    
    public int Angle
    {
        get
        {
            return Mathf.FloorToInt(Mathf.Abs(transform.rotation.eulerAngles.y));
        }
    }

    public int SizeX
    {
        get
        {
            if (Angle == 90 || Angle == 270)
            {
                return Data.SizeZ;
            }

            return Data.SizeX;
        }
    }

    public int SizeZ
    {
        get
        {
            if (Angle == 90 || Angle == 270)
            {
                return Data.SizeX;
            }

            return Data.SizeZ;
        }
    }

    public void OccupySMAP()
    {
        OccupySpot(transform.position.x, transform.position.z);

        for (int x = 1; x < this.SizeX + 1; x++)
        {
            OccupySpot(transform.position.x + x, transform.position.z);
            OccupySpot(transform.position.x - x, transform.position.z);

            for (int z = 1; z < this.SizeZ + 1; z++)
            {
                OccupySpot(transform.position.x + x, transform.position.z + z);
                OccupySpot(transform.position.x - x, transform.position.z - z);

            }
        }

        for (int z = 1; z < this.SizeZ + 1; z++)
        {
            OccupySpot(transform.position.x, transform.position.z + z);
            OccupySpot(transform.position.x, transform.position.z - z);

            for (int x = 1; x < this.SizeX + 1; x++)
            {
                OccupySpot(transform.position.x + x, transform.position.z + z);
                OccupySpot(transform.position.x - x, transform.position.z - z);

            }
        }
    }

    public void RemoveOccupation()
    {
        for(int i=0;i<OccupiedSMAPs.Count;i++)
        {
            BuildingTool.Instance.RemoveOccupationFromSMAP(OccupiedSMAPs[i], this);
        }
    }

    private void OccupySpot(float x, float z)
    {
        if (!BuildingTool.Instance.AddOccupationToSMAP(new Vector2(x, z), this))
        {
            return;
        }

        OccupiedSMAPs.Add(new Vector2(x, z));
    }
}
