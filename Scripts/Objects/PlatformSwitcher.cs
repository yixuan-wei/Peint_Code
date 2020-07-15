using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitcher : MonoBehaviour
{
    public bool m_isColorSame = true;
    public PlatformColor m_color = PlatformColor.PINK;
    public Sprite[] m_sprites = null;
    public PlatformType m_type = PlatformType.COLOR;

    private BoxCollider2D m_collider = null;
    private SpriteRenderer m_renderer = null;
    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponentInChildren<SpriteRenderer>();
        m_collider = GetComponent<BoxCollider2D>();
        m_collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ( m_type == PlatformType.COLOR || m_type == PlatformType.HAZARD )
        {
            m_collider.enabled = m_isColorSame;
            if ( !m_isColorSame )
            {
                m_renderer.sprite = m_sprites[0];
            }
            else
            {
                m_renderer.sprite = m_sprites[1];
            }
        }
        else if(m_type==PlatformType.BLOCKER)
        {
            m_collider.enabled = !m_isColorSame;
        }
    }
}
