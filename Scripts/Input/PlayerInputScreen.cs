using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputScreen : MonoBehaviour
{
    public bool m_changeColor = false;
    public bool m_wasChanging = false;
    public bool m_jump = false;
    public bool m_readyToJump = false;
    public bool m_wasJumping = false;

    private Vector2 m_screenSize;
    private float m_touchPad = 5.0f;
    private Button m_pauseButton = null;
    private bool m_canvasInWorld = false;

    private InputStates[] m_touchStates = null;

    enum InputStates
    {
        JUMP,
        COLOR,
        NUM_STATES
    };

    private void Start()
    {
        m_screenSize = new Vector2( Screen.width, Screen.height );
        m_pauseButton = GameObject.Find( "PauseButton" ).GetComponent<Button>();
        if ( m_pauseButton.transform.parent.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace )
            m_canvasInWorld = true;
        m_touchStates = new InputStates[(int)InputStates.NUM_STATES];
    }

    // Update is called once per frame
    void Update()
    {
        if ( !m_jump )
            m_wasJumping = false;
#if UNITY_EDITOR
        DetectKeyboardInput();
#endif

#if UNITY_STANDALONE
        DetectKeyboardInput();
#endif

#if UNITY_WEBGL
        DetectKeyboardInput();
#endif

        //handle touch on android pad
        for ( int i=0;i<Input.touchCount;i++ )
        {
            ProcessTouch( Input.touches[i] );
        }
    }

    public void ClearTouchStates()
    {
        m_changeColor = false;
        m_wasChanging = false;
        m_jump = false;
        m_readyToJump = false;
        m_wasJumping = false;
        for(int sID=0;sID<m_touchStates.Length;sID++ )
        {
            m_touchStates[sID] = InputStates.NUM_STATES;
        }
    }

    void DetectKeyboardInput()
    {
        //m_jump = false;
        if(Input.GetAxis("Fire1")>0.0f)
        {
            if(!m_wasChanging)
                m_changeColor = true; 
        }
        else if(m_wasChanging)
        {
            m_changeColor = false;
            m_wasChanging = false;
        }

        if(Input.GetAxis("Jump")>0)
        {
            m_jump = true;
        }
        else if(m_wasJumping && !m_jump)
        {
            m_wasJumping = false;
        }
        else
        {
            m_jump = false;
        }
    }

    bool IsInLeftHalfScreen(Vector2 point)
    {
        if ( point.x < m_screenSize.x * .5f )
            return true;
        else return false;
    }

    void ProcessTouch(Touch t)
    {
        if ( t.fingerId > (int)InputStates.NUM_STATES )//don't track the third finger id
            return;
        if(t.phase==TouchPhase.Began)
        {
            bool leftScreen,p;
            leftScreen = IsInLeftHalfScreen( t.position );
            Debug.Log( "screen hit: " + t.position );
            p = m_pauseButton.IsClickInside( t.position, m_touchPad, m_canvasInWorld );
            if(p )
            {
                Debug.Log( "pause hit " + t.position );
                return;
            }
            else if ( leftScreen && !m_wasChanging)
            {
                m_touchStates[t.fingerId] = InputStates.COLOR;
                m_changeColor = true;
                Debug.Log( "start color change from left" );
            }
            else 
            {
                m_touchStates[t.fingerId] = InputStates.JUMP;
                m_jump = true;
            }

        }
        else if(t.IsFinished())
        {
            if ( m_touchStates[t.fingerId] == InputStates.COLOR )
                m_wasChanging = false;
            else if ( m_touchStates[t.fingerId] == InputStates.JUMP )
                m_jump = false;
            m_touchStates[t.fingerId] = InputStates.NUM_STATES;
        }
        else // touch in process
        {
            if ( m_touchStates[t.fingerId] == InputStates.JUMP )
            {
                if ( !IsInLeftHalfScreen( t.position ) && !m_pauseButton.IsClickInside( t.position, m_touchPad ) )
                {
                    m_jump = true;
                }
                else m_jump = false;
            }
        }
    }
}
