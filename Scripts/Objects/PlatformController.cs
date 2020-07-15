using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformColor
{
    PINK,
    GREEN,
    YELLOW,
    NUM_COLORS
};

public enum PlatformType
{
    COLOR,
    BLOCKER,
    HAZARD,
    NUM_PLATFORM_TYPE
};

public class PlatformController : MonoBehaviour
{
    private PlatformColor m_color = PlatformColor.PINK;
    private PlatformSwitcher[] m_colorPlatforms = null;
    private PlayerControl m_player = null;
    private ScreenCameraMovement m_camMove = null;
    private ScreenBackgroundManager m_bgManager = null;
    private float m_screenWorldWidth=0f;
    private int m_camPrevID=-1;

    private void Start()
    {
        m_screenWorldWidth = 5f * 1280f / 800f;
        m_bgManager = GameObject.FindObjectOfType<ScreenBackgroundManager>();
        m_camMove = GameObject.FindObjectOfType<ScreenCameraMovement>();
        m_player = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerControl>();
        m_color = m_player.m_color;
        m_colorPlatforms = GameObject.FindObjectsOfType<PlatformSwitcher>();
        for ( int i = 0; i < m_colorPlatforms.Length; i++ )
        {
            if ( m_colorPlatforms[i].m_color == m_color )
                m_colorPlatforms[i].m_isColorSame = false;
        }
        ActivateAndDeactivatePlatforms( m_color );
    }

    // Update is called once per frame
    void Update()
    {
        if ( m_camPrevID != m_camMove.m_currentPos )
        {
            CheckHorizontalPlatformsInView();
        }
        if (m_player==null)
        {
            m_player = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerControl>();
        }
        else if(m_color!= m_player.m_color)
        {
            m_color = m_player.m_color;
            ActivateAndDeactivatePlatforms( m_color );
        }
    }

    private void ActivateAndDeactivatePlatforms(PlatformColor color)
    {
        for(int i=0; i<m_colorPlatforms.Length;i++ )
        {
            if ( color == m_colorPlatforms[i].m_color )
            {
                m_colorPlatforms[i].m_isColorSame = true;
            }
            else
            {
                m_colorPlatforms[i].m_isColorSame = false;
            }
        }
    }

    bool IsPlatformIDInView( int platformID )
    {
        if ( ( m_camMove.m_currentPos == m_camMove.m_screens.Length - 1
            || m_colorPlatforms[platformID].transform.position.x <
               m_camMove.m_screens[m_camMove.m_currentPos + 1].camMinX - m_screenWorldWidth ) &&
              ( m_camMove.m_currentPos == 0 ||
              m_colorPlatforms[platformID].transform.position.x >
               m_camMove.m_screens[m_camMove.m_currentPos - 1].camMaxX + m_screenWorldWidth ) )
            return true;
        else return false;
    }

    void CheckHorizontalPlatformsInView()
    {
        Debug.Log( "Start checking horizontal Platform view" );
        m_camPrevID = m_camMove.m_currentPos;
        bool[] colorExists = new bool[(int)PlatformColor.NUM_COLORS];
        for ( int id = 0; id < colorExists.Length; id++ )
            colorExists[id] = false;
        for ( int i = 0; i < m_colorPlatforms.Length; i++ )
        {
            if ( IsPlatformIDInView( i ) && m_colorPlatforms[i].transform.rotation == new Quaternion( 0, 0, 0, 1f ) )
            {
                colorExists[(int)m_colorPlatforms[i].m_color] = true;
            }
        }
        for ( int id = 0; id < colorExists.Length; id++ )
        {
            if ( colorExists[id] == false )
                m_bgManager.UpdateBackgrounds( (PlatformColor)id );
        }
    }
}
