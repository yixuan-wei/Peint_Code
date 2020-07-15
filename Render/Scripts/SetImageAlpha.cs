using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetImageAlpha : PostEffectsBase
{
    [Range(0, 1)]
    public float rightX = 1;
    [Range(0,1)]
    public float leftX = 1;
    [Range(0, 1)]
    public float topY = 1;
    // Use this for initialization
    public Shader alphaShader;
    private Material _materal;
    public Material _Material
    {
        get
        {
            _materal = CheckShaderAndCreateMaterial( alphaShader, _materal );
            return _materal;
        }
    }
    private void Awake()
    {
        alphaShader = Shader.Find( "Unlit/GradualReveal" );
        GetComponent<SpriteRenderer>().material = _Material;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        _Material.SetFloat( "_AlphaRX", rightX*2f-1f);
        _Material.SetFloat( "_AlphaLX", 2f-leftX*2f);
        _Material.SetFloat( "_AlphaTY", topY*2f-1f );
    }
}
//https://blog.csdn.net/qq_34537249/article/details/78264955
