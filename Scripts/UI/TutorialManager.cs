using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public struct TutorialState
{
    public Vector2 position;
    public GameObject menu;
    public int tutorialID;
};

public enum Tutorials
{
    T_INVALID = -1,
    T_JUMP,
    T_COLOR,
    T_THREE_COLORS,
    T_COLOR_PINK,
    T_JUMP2,
    T_COLOR2,
    T_JUMP3,
    NUM_TUTORIALS
};

public class TutorialManager : MonoBehaviour
{
    public float m_textRevealTime = 1f;
    public bool m_isMenuOn = false;
    public TutorialState[] m_states;
    public Tutorials m_currentState = Tutorials.T_INVALID;
    private MusicManager m_sfx = null;
    private PlayerControlTutorial m_player = null;
    private PlayerInputScreen m_input = null;
    private GameObject[] m_tutorialTriggers;

    private void Awake()
    {
        m_input = GameObject.FindObjectOfType<PlayerInputScreen>();
        m_player = GameObject.FindObjectOfType<PlayerControlTutorial>();
        m_sfx = GameObject.FindObjectOfType<MusicManager>();
        m_tutorialTriggers = new GameObject[m_states.Length];
        for(int id=0;id<m_states.Length;id++)
        {
            TutorialState state = m_states[id];
            GameObject tutorialTrigger = new GameObject("Tutorial"+(Tutorials)id);
            tutorialTrigger.tag = "Tutorial";
            tutorialTrigger.transform.position = state.position;
            BoxCollider2D collider = tutorialTrigger.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            state.menu.SetActive( false );
            m_tutorialTriggers[id] = tutorialTrigger;
        }
    }

    public void ActivateTutorial(string name)
    {
        Tutorials index = Tutorials.T_INVALID;
        Debug.Log( name );
        if(name=="Tutorial"+Tutorials.T_JUMP.ToString())
        {
            index = Tutorials.T_JUMP;
            StartTextRevealing( m_states[(int)index].menu );
            //m_states[(int)index].menu.GetComponent<Button>().onClick.AddListener( OnJumpChangeRequested );
        }
        else if(name=="Tutorial"+Tutorials.T_COLOR.ToString())
        {
            index = Tutorials.T_COLOR;
            StartTextRevealing( m_states[(int)index].menu );
            //m_states[(int)index].menu.GetComponent<Button>().onClick.AddListener( OnColorChangeRequested );
        }
        else if(name=="Tutorial"+Tutorials.T_THREE_COLORS.ToString())
        {
            index = Tutorials.T_THREE_COLORS;
            StartTextRevealing( m_states[(int)index].menu );
            Button[] texts = m_states[(int)index].menu.GetComponentsInChildren<Button>();
            for ( int tID = 0; tID < texts.Length; tID++ )
            {
                if ( texts[tID].name == "Text1" )
                {
                    texts[tID].onClick.AddListener( () => OnText1Clicked( texts ) );
                }
                else if ( texts[tID].name == "Text2" )
                {
                    texts[tID].gameObject.SetActive( false );
                }
            }
        }
        else if ( name == "Tutorial" + Tutorials.T_COLOR_PINK.ToString() )
        {
            index = Tutorials.T_COLOR_PINK;
            StartTextRevealing( m_states[(int)index].menu );
            m_states[(int)index].menu.GetComponentInChildren<Button>().onClick.AddListener( ()=>OnCertainColorChangeRequested(PlatformColor.PINK) );
        }
        else if ( name == "Tutorial" + Tutorials.T_JUMP2.ToString() && m_currentState==Tutorials.T_COLOR_PINK)
        {
            index = Tutorials.T_JUMP2;
            StartTextRevealing( m_states[(int)index].menu );
            Button[] buttons = m_states[(int)index].menu.GetComponentsInChildren<Button>();
            for(int bID=0;bID<buttons.Length;bID++ )
            {
                if ( buttons[bID].name == "JumpMenu" )
                {
                    EventTrigger trigger = buttons[bID].gameObject.AddComponent<EventTrigger>();
                    var pointerDown = new EventTrigger.Entry();
                    pointerDown.eventID = EventTriggerType.PointerDown;
                    pointerDown.callback.AddListener( (b) => OnJumpFirstRequested( buttons ) );
                    trigger.triggers.Add( pointerDown );
                }
                else if ( buttons[bID].name == "ColorMenu" )
                {
                    buttons[bID].gameObject.SetActive( false );
                }
            }
        }
        else if ( name == "Tutorial" + Tutorials.T_COLOR2.ToString() && m_currentState == Tutorials.T_JUMP2)
        {
            index = Tutorials.T_COLOR2 ;
            StartTextRevealing( m_states[(int)index].menu );
        }
        else if ( name == "Tutorial" + Tutorials.T_JUMP3.ToString() )
        {
            index = Tutorials.T_JUMP3;
            StartTextRevealing( m_states[(int)index].menu );
            //m_states[(int)index].menu.GetComponent<Button>().onClick.AddListener( OnJumpChangeRequested );
        }

        if ( index != Tutorials.T_INVALID )
        {
            m_tutorialTriggers[(int)index].GetComponent<BoxCollider2D>().enabled = false;
            m_states[(int)index].menu.SetActive( true );
            //StartTextRevealing( m_states[(int)index].menu );
            //if(index!=Tutorials.T_COLOR && index!=Tutorials.T_JUMP)
            //    Time.timeScale = 0f;

            m_sfx.StopAllSFX();
            m_isMenuOn = true;
        }
    }

    void StartTextRevealing(GameObject menu)
    {
        Text[] texts = menu.GetComponentsInChildren<Text>();
        SetUIAlpha[] alphas = new SetUIAlpha[texts.Length];
        for(int tIdx=0;tIdx<texts.Length;tIdx++ )
        {
            alphas[tIdx] = texts[tIdx].gameObject.AddComponent<SetUIAlpha>();
            alphas[tIdx].rightX = 0f;
            alphas[tIdx].m_transitionTime = m_textRevealTime;
        }
    }

    public void OnColorChangeStart()
    {
        m_isMenuOn = false;
        m_currentState += 1;
        m_states[(int)m_currentState].menu.SetActive( false );
        Time.timeScale = 1f;
        m_input.ClearTouchStates();
    }

    public void OnColorChangeRequested()
    {
        m_isMenuOn = false;
        m_player.SetNextColor();
        m_currentState += 1;
        m_states[(int)m_currentState].menu.SetActive( false );
        Time.timeScale = 1f;
        m_input.ClearTouchStates();
    }

    public void OnText1Clicked(Button[] texts)
    {
        for(int tID=0;tID<texts.Length;tID++ )
        {
            if(texts[tID].name=="Text1")
            {
                texts[tID].gameObject.SetActive( false );
            }
            else if(texts[tID].name=="Text2")
            {
                texts[tID].gameObject.SetActive( true );
            }
        }
        m_input.ClearTouchStates();
    }

    public void OnText2Clicked()
    {
        m_isMenuOn = false;
        m_input.m_changeColor = false;
        m_currentState += 1;
        m_states[(int)m_currentState].menu.SetActive( false );
        Time.timeScale = 1f;
        m_input.ClearTouchStates();
    }

    public void OnCertainColorChangeRequested(PlatformColor color)
    { 
        if(m_player.m_color==color)
        {
            m_isMenuOn = false;
            m_input.m_changeColor = false;
            m_currentState += 1;
            m_states[(int)m_currentState].menu.SetActive( false );
            Time.timeScale = 1f;
            m_input.ClearTouchStates();
            return;
        }
        m_player.SetNextColor();
        if ( m_player.m_color == color )
        {
            m_isMenuOn = false;
            m_input.m_changeColor = false;
            m_currentState += 1;
            m_states[(int)m_currentState].menu.SetActive( false );
            Time.timeScale = 1f;
            m_input.ClearTouchStates();
        }
    }

    public void OnNextStageRequested()
    {
        m_isMenuOn = false;
        m_currentState += 1;
        m_states[(int)m_currentState].menu.SetActive( false );
        Time.timeScale = 1f;
        m_input.ClearTouchStates();
    }

    public void OnJumpChangeRequested()
    {
        m_isMenuOn = false;
        m_input.m_jump = true;
        m_input.m_readyToJump = true;
        m_input.m_wasJumping = false;
        m_currentState += 1;
        m_states[(int)m_currentState].menu.SetActive( false );
        Time.timeScale = 1f;
    }

    public void OnJumpFirstRequested(Button[] buttons)
    {
        for ( int bID = 0; bID < buttons.Length; bID++ )
        {
            if ( buttons[bID].name == "JumpMenu" )
            {
                buttons[bID].gameObject.SetActive( false );
            }
            else if ( buttons[bID].name == "ColorMenu" )
            {
                buttons[bID].gameObject.SetActive( true );
            }
        }
        OnJumpChangeRequested();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        foreach ( TutorialState state in m_states )
        {
            Gizmos.DrawWireSphere( state.position, .5f );
        }
    }
}
