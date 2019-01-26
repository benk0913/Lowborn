using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParticles : MonoBehaviour
{
    [SerializeField]
    ParticleSystem Particles;

    public void SetTarget(MeshFilter targetMesh)
    {
        transform.position = targetMesh.transform.position;
        transform.rotation = targetMesh.transform.rotation;

        var sh = Particles.shape;
        sh.enabled = true;
        sh.shapeType = ParticleSystemShapeType.Mesh;
        sh.mesh = targetMesh.mesh;
    }
}
