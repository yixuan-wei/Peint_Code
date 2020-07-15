using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMetricsUI : MonoBehaviour
{
    public float m_jumpStartSpeed = 6.0f;
    public float m_jumpSpeedRange = 8.0f;
    private Text m_jumpValue = null;
    public float m_moveStartSpeed = 2.0f;
    public float m_moveSpeedRange = 7.0f;
    private Text m_moveValue = null;

    public float m_jumpMoveStartSpeed = 2.0f;
    public float m_jumpMoveSpeedRange = 7.0f;
    private Text m_jumpMoveValue = null;

    private Slider jumpMoveSlider = null;
    private Slider jumpSlider = null;
    private Slider moveSlider=null;
    private PlayerControl m_player = null;
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerControl>();
        GameObject[] sliders = null;
        sliders = GameObject.FindGameObjectsWithTag("GameController");
        foreach(GameObject slider in sliders)
        {
            if(slider.name=="JumpSlider")
            {
                jumpSlider = slider.GetComponentInChildren<Slider>();
                jumpSlider.value = ( m_player.m_jumpStartSpeed - m_jumpStartSpeed ) / m_jumpSpeedRange;
                m_jumpValue = slider.GetComponentInChildren<Text>();
                m_jumpValue.text = m_player.m_jumpStartSpeed.ToString();
            }
            else if(slider.name=="MoveSlider")
            {
                moveSlider = slider.GetComponentInChildren<Slider>();
                moveSlider.value = ( m_player.m_moveSpeed - m_moveStartSpeed ) / m_moveSpeedRange;
                m_moveValue = slider.GetComponentInChildren<Text>();
                m_moveValue.text = m_player.m_moveSpeed.ToString();
            }
            else if(slider.name=="JumpMoveSlider")
            {
                jumpMoveSlider = slider.GetComponentInChildren<Slider>();
                jumpMoveSlider.value = ( m_player.m_jumpMoveSpeed - m_jumpMoveStartSpeed ) / m_jumpMoveSpeedRange;
                m_jumpMoveValue = slider.GetComponentInChildren<Text>();
                m_jumpMoveValue.text = m_player.m_jumpMoveSpeed.ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( m_player == null )
        {
            m_player = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerControl>();
            Debug.Log( "Metric renew player" );
        }
            
        m_player.m_jumpStartSpeed = jumpSlider.value*m_jumpSpeedRange + m_jumpStartSpeed;
        m_jumpValue.text = m_player.m_jumpStartSpeed.ToString();
        m_player.m_moveSpeed = moveSlider.value * m_moveSpeedRange + m_moveStartSpeed;
        m_moveValue.text = m_player.m_moveSpeed.ToString();
        m_player.m_jumpMoveSpeed = jumpMoveSlider.value * m_jumpMoveSpeedRange + m_jumpMoveStartSpeed;
        m_jumpMoveValue.text = m_player.m_jumpMoveSpeed.ToString();
    }
}
