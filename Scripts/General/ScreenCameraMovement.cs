using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraScreenDefinition
{
    public float camMinX;
    public float camMaxX;
};


public class ScreenCameraMovement : MonoBehaviour
{

    public CameraScreenDefinition[] m_screens = null;
    private GameObject m_player = null;
    private ContinuousCameraMovement m_continueCam = null;

    private float transitionTime=1f;
    private float halfHeight = 5f;
    private float halfWidth = 5f*1280f/800f;
    public int m_currentPos;
    public bool m_isMoving = false;

    private float m_lastTransitionPos = 0f;
    private float m_newTransitionPos = 0f;
    private bool m_playerShift = false;

    private void Start()
    {
        m_newTransitionPos = m_lastTransitionPos = m_screens[0].camMinX;
        m_continueCam = GetComponent<ContinuousCameraMovement>();
        m_continueCam.m_minCameraPosition = m_screens[0].camMinX;
        m_continueCam.m_maxCamearaPosition = m_screens[0].camMaxX;
        m_player = GameObject.FindGameObjectWithTag( "Player" );
        //transform.position = new Vector3( m_cameraXPositions[0], 0, transform.position.z );
        m_currentPos = 0;
    }

    void UpdateCurPosForNewPlayer()
    {
        Debug.Log( "Start camera pos update" );
        for(int sID=0;sID<m_screens.Length;sID++ )
        {
            float playerX = m_player.transform.position.x;
            if ( playerX < m_screens[sID].camMaxX )
            {
                m_currentPos = sID;
                Debug.Log( "current cam pos: " + m_currentPos );
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( m_player == null )
        {
            m_player = GameObject.FindGameObjectWithTag( "Player" );
            UpdateCurPosForNewPlayer();
            m_isMoving = true;
            m_playerShift = true;
            transitionTime = 0f;
            m_continueCam.m_minCameraPosition = m_screens[m_currentPos].camMinX;
            m_continueCam.m_maxCamearaPosition = m_screens[m_currentPos].camMaxX;
            m_lastTransitionPos = transform.position.x;
            m_newTransitionPos = m_player.transform.position.x+m_continueCam.m_screenSixthWidth;
        }
        else if( m_currentPos < m_screens.Length - 1 )
        {
            float newChangeLimit = (m_screens[m_currentPos].camMaxX+halfWidth);
            if ( m_player.transform.position.x > newChangeLimit )
            {
                m_playerShift = false;
                m_currentPos += 1;
                transitionTime = 0f;
                m_isMoving = true;
                m_continueCam.m_minCameraPosition = m_screens[m_currentPos].camMinX;
                m_continueCam.m_maxCamearaPosition = m_screens[m_currentPos].camMaxX;
                m_lastTransitionPos = m_screens[m_currentPos - 1].camMaxX;
                m_newTransitionPos = m_screens[m_currentPos].camMinX;
            }
        }
    }

    private void LateUpdate()
    {
        if ( m_isMoving && transitionTime <= 1f && m_newTransitionPos != m_lastTransitionPos )
        {
            transitionTime += Time.deltaTime;
            Vector3 newPos = new Vector3( m_newTransitionPos,0,-10f);
            Vector3 oldPos = new Vector3(m_lastTransitionPos,0,-10f);
            transform.position = Vector3.Lerp( oldPos, newPos, transitionTime );
            
            if ( m_playerShift )
            {
                if ( transform.position.x < m_screens[m_currentPos].camMinX )
                {
                    transitionTime = 1.1f;
                    transform.position = new Vector3( m_screens[m_currentPos].camMinX, 0, -10f );
                }
                else   m_newTransitionPos = m_player.transform.position.x + m_continueCam.m_screenSixthWidth;
            } 
        }
        else
        {
            m_isMoving = false;
            m_playerShift = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for ( int id = 0; id < m_screens.Length; id++ )
        {
            Gizmos.DrawWireSphere(new Vector2(m_screens[id].camMinX,0),0.5f);
            Gizmos.DrawWireSphere(new Vector2(m_screens[id].camMaxX,0),0.5f);
            Gizmos.DrawLine( new Vector2( m_screens[id].camMinX - halfWidth, - halfHeight),
                new Vector2( m_screens[id].camMinX-halfWidth, halfHeight ) );
            Gizmos.DrawLine( new Vector2( m_screens[id].camMaxX + halfWidth, -halfHeight ),
                new Vector2( m_screens[id].camMaxX + halfWidth, halfHeight ) );
            Gizmos.DrawLine( new Vector2( m_screens[id].camMinX - halfWidth, -halfHeight ),
                 new Vector2( m_screens[id].camMaxX + halfWidth, -halfHeight ) );
            Gizmos.DrawLine( new Vector2( m_screens[id].camMinX - halfWidth, halfHeight ),
                 new Vector2( m_screens[id].camMaxX + halfWidth, halfHeight ) );
        }
    }
}
