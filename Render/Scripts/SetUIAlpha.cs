using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUIAlpha : PostEffectsBase
{
    public float m_transitionTime = 1f;
    [Range(0, 1)]
    public float rightX = 1;

    // Use this for initialization
    public Shader alphaShader;
    private Material _materal;
    private Text[] m_texts;
    private Image[] m_images;

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
        alphaShader = Shader.Find( "Unlit/GradualRevealUI" );

        m_texts = GetComponentsInChildren<Text>();
        foreach (Text t in m_texts)
        {
            t.material = _Material;
        }

        m_images = GetComponentsInChildren<Image>();
        foreach ( Image i in m_images )
        {
            i.material = _Material;
        }

        rightX = 0;
        StartCoroutine( TextReveal() );
        
    }

    IEnumerator TextReveal()
    {
        Debug.Log( "Start UI Alpha Change" );
        TimeCustom counter = new TimeCustom();
        counter.Start( m_transitionTime );
        while ( !counter.HasElapsed() )
        {
            float alphaExtent = counter.GetElapsedNormalized();
            rightX = alphaExtent;
            yield return new WaitForEndOfFrame();
        }
        rightX = 1f;
        yield return null;
    }
    // Update is called once per frame
    void LateUpdate()
    {
    //    if ( m_transitCounter <= m_transitionTime )
    //    {
    //        m_transitCounter += Time.deltaTime;
    //        rightX = m_transitCounter / m_transitionTime;
    //    }
    //    else rightX = 1f;
        _Material.SetFloat( "_AlphaRX", rightX * 2f - 1f );
    }
}
//https://blog.csdn.net/qq_34537249/article/details/78264955

