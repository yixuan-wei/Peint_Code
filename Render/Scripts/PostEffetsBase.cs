﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class PostEffectsBase : MonoBehaviour
{
       
    // Called when need to create the material used by this effect
    protected Material CheckShaderAndCreateMaterial( Shader shader, Material material )
    {
        if ( shader == null )
        {
            return null;
        }

        if ( shader.isSupported && material && material.shader == shader )
            return material;

        if ( !shader.isSupported )
        {
            return null;
        }
        else
        {
            material = new Material( shader );
            material.hideFlags = HideFlags.DontSave;
            if ( material )
                return material;
            else
                return null;
        }
    }
//https://blog.csdn.net/qq_34537249/article/details/78264955
}