using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorWheel : MonoBehaviour
{
    public Sprite[] m_sprites = null;

    private PlayerControl m_player = null;
    private PlatformColor m_color;
    private Image m_image = null;

    private void Start()
    {
        m_player = GameObject.FindObjectOfType<PlayerControl>();
        m_color = PlatformColor.PINK;
        m_image = gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        if(m_player==null)
        {
            m_player = GameObject.FindObjectOfType<PlayerControl>();
        }
        else if(m_player.m_color!=m_color)
        {
            m_color = m_player.m_color;
            m_image.sprite = m_sprites[(int)m_color];
        }
    }
}
