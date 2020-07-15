using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageBackgroundSwitcher : MonoBehaviour
{
    public Sprite[] m_backgroundSprites = null;

    private ScreenCameraMovement m_camMove = null;
    private int m_prevCamPos = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        m_camMove = GameObject.FindObjectOfType<ScreenCameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_prevCamPos!=m_camMove.m_currentPos)
        {
            m_prevCamPos = m_camMove.m_currentPos;
            CheckForBackgroundChange();
        }
    }

    void CheckForBackgroundChange()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = m_backgroundSprites[0];
        for(int id=0;id<m_backgroundSprites.Length;id++ )
        {
            if((float)m_prevCamPos/(float)m_camMove.m_screens.Length>
                (float)(id)/m_backgroundSprites.Length)
            {
                renderer.sprite = m_backgroundSprites[id];
            }
            else
            {
                break;
            }
        }
    }
}
