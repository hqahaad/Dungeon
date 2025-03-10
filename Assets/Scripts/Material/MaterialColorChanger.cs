using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColorChanger : MonoBehaviour
{
    [SerializeField] private Color materialColor;

    void Awake()
    {
        var renderer = GetComponent<MeshRenderer>();
        var mpb = new MaterialPropertyBlock();

        mpb.SetColor("_BaseColor", materialColor);

        renderer.SetPropertyBlock(mpb);
    }
}
