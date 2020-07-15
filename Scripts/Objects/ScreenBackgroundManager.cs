using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBackgroundManager : MonoBehaviour
{
    public float m_changeTime = 5f;
    private float m_unitChangeTime = 0f;

    private PlayerControl m_player = null;
    private BackgroundSwitcher[] m_backgrounds = null;
    private ScreenCameraMovement m_camMovement = null; 
    private TiledImagesManager m_imgsBGManager = null;
    private float m_screenWorldWidth = 0f;
    private PlatformColor m_color = PlatformColor.NUM_COLORS;
    private int m_camPrevID = -1;

    void Start()
    { 
        m_imgsBGManager = GameObject.FindObjectOfType<TiledImagesManager>();
        m_player = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerControl>();
        m_backgrounds = GameObject.FindObjectsOfType<BackgroundSwitcher>();
        m_camMovement = GameObject.FindObjectOfType<ScreenCameraMovement>();
        m_screenWorldWidth = 5f * 1280f / 800f;
        MathUtilization.SortMonoBehaviorArrayWithPositionX( m_backgrounds );

        m_unitChangeTime = m_changeTime / m_backgrounds.Length / 3f;
    }


    // Update is called once per frame
    void Update()
    {
        if ( m_camPrevID != m_camMovement.m_currentPos )
            ResetBackgrounds();
        if ( m_player == null )
            m_player = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerControl>();
        else if ( m_player.m_onColorPlatform )
        {
            m_color = m_player.m_color;
            UpdateBackgrounds( m_color );
        }
    }
    
    public bool IsCurrentBackgroundFilled()
    {
        for ( int bID = 0; bID < m_backgrounds.Length; bID++ )
        {
            if ( !m_backgrounds[bID].m_colorActivated )
            {
                return false;
            }
        }
        return true;
    }

    bool IsBgIDAlreadyInView( int bgID )
    {
        if ( m_camMovement.m_currentPos == m_camMovement.m_screens.Length - 1
            || m_backgrounds[bgID].transform.position.x <
               m_camMovement.m_screens[m_camMovement.m_currentPos + 1].camMinX - m_screenWorldWidth )
        {
            if ( m_camMovement.m_currentPos == 0 ||
              m_backgrounds[bgID].transform.position.x >
               m_camMovement.m_screens[m_camMovement.m_currentPos - 1].camMaxX + m_screenWorldWidth )
                return true;
        }
        return false;
    }

    IEnumerator BackgroundsUpdate(PlatformColor color)
    {
        TimeCustom counter = new TimeCustom();
        counter.Start( m_changeTime );
        float percentElapsed = 0f;
        for ( int backgroundID = 0; backgroundID < m_backgrounds.Length; backgroundID++ )
        {
            if ( percentElapsed < (float)backgroundID / (float)m_backgrounds.Length )
            {
                percentElapsed = counter.GetElapsedNormalized();
                yield return new WaitForEndOfFrame();
            }

            if ( !m_backgrounds[backgroundID].m_colorActivated )
            {
                if ( m_backgrounds[backgroundID].m_color == color )
                {
                    m_backgrounds[backgroundID].m_colorActivated = true;
                    m_backgrounds[backgroundID].m_wasColorChanged = false;
                }
            }    
            
        }
        if ( IsCurrentBackgroundFilled() )
            m_imgsBGManager.FillAllImgBGs();
        yield return null;
    }

    public void UpdateBackgrounds( PlatformColor color )
    {
        StartCoroutine( BackgroundsUpdate( color ) );
    }

    void ResetBackgrounds()
    {
        m_imgsBGManager.ResetAllImgBGs();
        for ( int backgroundID = 0; backgroundID < m_backgrounds.Length; backgroundID++ )
        {
                m_backgrounds[backgroundID].m_colorActivated = false;
                m_backgrounds[backgroundID].m_wasColorChanged = false;
        }
        m_camPrevID = m_camMovement.m_currentPos;
    }
}
