using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMeshEmitter : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.BakeMesh(mesh);
    }
}
