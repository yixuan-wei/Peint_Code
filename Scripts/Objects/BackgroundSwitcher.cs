using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
    public PlatformColor m_color = PlatformColor.PINK;
    public bool m_colorActivated = false;
    public Sprite m_emptySprite = null;
    public Sprite m_filledSprite = null;
    public float m_changeTime = 1f;

    public bool m_wasColorChanged = true;
    private bool m_wasFlipped = false;
    private float m_changeSteps = 100f;
    private SpriteRenderer m_renderer = null;
    private SetImageAlpha m_alpha = null;
    private Coroutine m_alphaCounter = null;
    // Start is called before the first frame update
    void Start()
    {
        m_renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        m_alpha = gameObject.GetComponentInChildren<SetImageAlpha>();
        m_alpha.rightX = 1;
        m_alpha.leftX = 1;
        m_alpha.topY = 1;
        if ( transform.localScale.x < 0f )
            m_wasFlipped = true;
    }

    IEnumerator AlphaChange()
    {
        if ( !m_wasFlipped )
            m_alpha.rightX = 0;
        else m_alpha.leftX = 0;

        SpriteRenderer oldSprite = gameObject.AddComponent<SpriteRenderer>();
        SetImageAlpha oldAlpha = gameObject.AddComponent<SetImageAlpha>();
        oldAlpha.rightX = 1;
        oldSprite.sprite = m_renderer.sprite;
        oldSprite.sortingLayerName = "Background";
        oldSprite.sortingOrder = m_renderer.sortingOrder -1;

        float timeEachStep = m_changeTime/m_changeSteps;
        float pastSteps = 0f;

        m_renderer.sprite = m_filledSprite;
        
        while (true)
        {
            if ( pastSteps > m_changeSteps )
                break;
            float alphaExtent = (pastSteps)/m_changeSteps;
            if ( !m_wasFlipped )
                m_alpha.rightX = alphaExtent;
            else m_alpha.leftX = alphaExtent;
            pastSteps += 1f;
            yield return new WaitForSeconds( timeEachStep );            
        }
        m_alpha.leftX = 1;
        m_alpha.rightX = 1;

        Destroy( oldAlpha );
        Destroy( oldSprite );
        m_wasColorChanged = true;
        m_alphaCounter = null;
    }
    // Update is called once per frame
    void Update()
    {
        if ( m_colorActivated && !m_wasColorChanged )
        {
            if(m_alphaCounter==null)
                m_alphaCounter = StartCoroutine( AlphaChange() );
            
        }
        else if ( !m_colorActivated && !m_wasColorChanged )
        {
            m_renderer.sprite = m_emptySprite;
            m_wasColorChanged = true;
        }
    }
}
