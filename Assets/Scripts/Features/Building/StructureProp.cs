using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class StructureProp : MonoBehaviour
{
    //TODO Replace Data with a reference to a prop scriptable obj & move data params to there.
    [SerializeField]
    public Prop Data;

    [System.NonSerialized]
    public List<Vector2> OccupiedSMAPs = new List<Vector2>();

    List<SubMat> OriginalMaterials;

    public Dynasty Owner;

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

    private void OnEnable()
    {
        SaveOriginalMaterials();
    }
    
    public void OccupySMAP(bool CurveNavMesh = true)
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

        if (CurveNavMesh)
        {
            LocationMap.Instance.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }

    public void RemoveStructureProp()
    {
        RemoveOccupation();

        for (int i = 0; i < Data.OnRemoveEvents.Count; i++)
        {
            Data.OnRemoveEvents[i].ExecuteEvent(this);
        }
    }

    public void PlaceStructureProp(int currentFloor, Dynasty owner)
    {
        this.Owner = owner;
        this.Floor = currentFloor;
        this.OccupySMAP();
        
        for(int i=0;i<Data.OnPlaceEvents.Count;i++)
        {
            Data.OnPlaceEvents[i].ExecuteEvent(this);
        }
    }

    public void RemoveOccupation()
    {
        for(int i=0;i<OccupiedSMAPs.Count;i++)
        {
            LocationMap.Instance.RemoveOccupationFromSMAP(OccupiedSMAPs[i], this);
        }

        LocationMap.Instance.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    private void OccupySpot(float x, float z, bool Recurve = true)
    {
        if (!LocationMap.Instance.AddOccupationToSMAP(new Vector2(x, z), this))
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

    public void HideMesh()
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
        for(int i=0;i<MeshRenderers.Length;i++)
        {
            MeshRenderer renderer = MeshRenderers[i];
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
                List<Material> materials = new List<Material>();
                materials.InsertRange(0, OriginalMaterials[i].Materials);
                materials.Add(material);

                renderer.materials = materials.ToArray();
            }
        }
    }

    public void SetOriginalMaterials()
    {
        SaveOriginalMaterials();

        for (int i = 0; i < MeshRenderers.Length; i++)
        {
            MeshRenderers[i].materials = OriginalMaterials[i].Materials;
        }
    }

    void SaveOriginalMaterials()
    {
        if(OriginalMaterials != null)
        {
            return;
        }

        OriginalMaterials = new List<SubMat>();

        for (int i=0;i<MeshRenderers.Length;i++)
        {
            OriginalMaterials.Add(new SubMat(i, MeshRenderers[i].materials));
        }
    }

    public class SubMat
    {
        public int Index;
        public Material[] Materials;

        public SubMat(int index, Material[] mat)
        {
            this.Index = index;
            this.Materials = mat;
        }
    }
}
