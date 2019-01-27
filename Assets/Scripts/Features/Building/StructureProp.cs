using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StructureProp : MonoBehaviour
{
    [SerializeField]
    public StructureData Data;

    [System.NonSerialized]
    public List<Vector2> OccupiedSMAPs = new List<Vector2>();

    List<Material> OriginalMaterials;

    public int Floor;

    [SerializeField]
    public MeshRenderer[] MeshRenderers;

    public void Reset()
    {
        LoadMeshes();
    }

    public void LoadMeshes()
    {
        MeshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

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

    private void Start()
    {
        SaveOriginalMaterials();
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

    public void ShowMesh()
    {
        foreach (MeshRenderer MeshObject in MeshRenderers)
        {
            MeshObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }

        BoxCollider[] colliders = GetComponents<BoxCollider>();
        foreach (BoxCollider coll in colliders)
        {
            coll.enabled = true;
        }
    }

    public void HideMesh(Material hiddenMaterial)
    {
        foreach (MeshRenderer MeshObject in MeshRenderers)
        {
            MeshObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }

        BoxCollider[] colliders = GetComponents<BoxCollider>();
        foreach (BoxCollider coll in colliders)
        {
            coll.enabled = false;
        }
    }

    public void SetMaterial(Material material, bool ReplaceOriginalMaterial = false)
    {
        foreach (MeshRenderer MeshObject in MeshRenderers)
        {
            MeshRenderer renderer = MeshObject.GetComponent<MeshRenderer>();
            if (renderer.material == material)
            {
                return;
            }


            SaveOriginalMaterials();

            if (ReplaceOriginalMaterial)
            {
                renderer.materials = new Material[1];
                renderer.material = material;
            }
            else
            {
                List<Material> newMaterials = new List<Material>();
                newMaterials.InsertRange(0, OriginalMaterials);
                newMaterials.Insert(0, material);

                renderer.materials = newMaterials.ToArray();
            }
        }
    }

    public void SetOriginalMaterials()
    {
        SaveOriginalMaterials();

        MeshRenderer MeshObject = MeshRenderers[0];

        if (MeshObject.GetComponent<MeshRenderer>().material == OriginalMaterials[0])
        {
            return;
        }

        MeshObject.GetComponent<MeshRenderer>().materials = OriginalMaterials.ToArray();
    }

    void SaveOriginalMaterials()
    {
        if(OriginalMaterials != null)
        {
            return;
        }

        OriginalMaterials = new List<Material>();

        MeshRenderer MeshObject = MeshRenderers[0];
        OriginalMaterials.InsertRange(0, MeshObject.GetComponent<MeshRenderer>().materials);
    }
}
