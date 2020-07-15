using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousCameraMovement : MonoBehaviour
{
    public float m_minCameraPosition = 0.0f;
    public float m_maxCamearaPosition = 13.5f;

    private GameObject m_player = null;
    public float m_screenSixthWidth = 5f*1280f/800f/1.8f;
    private ScreenCameraMovement m_screenCam = null;

    private void Start()
    {
        m_screenCam = GetComponent<ScreenCameraMovement>();
        m_player = GameObject.FindGameObjectWithTag( "Player" );
        transform.position = new Vector3( m_minCameraPosition, 0.0f, -10.0f );
    }

    // Update is called once per frame
    void Update()
    {
        if ( m_player == null )
        {
            m_player = GameObject.FindGameObjectWithTag( "Player" );
            transform.position = new Vector3( m_player.transform.position.x - m_screenSixthWidth, 0.0f, -10.0f );
        }
        if( m_player.transform.position.x > transform.position.x - m_screenSixthWidth )
        {
            transform.position = new Vector2(m_player.transform.position.x + m_screenSixthWidth,0f);
        }
    }

    private void LateUpdate()
    {
        if ( !m_screenCam.m_isMoving )
        {
            m_screenCam.m_isMoving = false;
            Vector3 camPos= transform.position;
            if ( camPos.x > m_maxCamearaPosition )
                camPos.x = m_maxCamearaPosition;
            else if ( camPos.x < m_minCameraPosition )
                camPos.x = m_minCameraPosition;
            transform.position = new Vector3( camPos.x, 0f, -10f );
        }        
    }
        
}
