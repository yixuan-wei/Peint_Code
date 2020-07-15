using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public GameObject m_menu=null;
    public GameObject m_popOut = null;
    public float m_popOutTime = 1f;

    private void Awake()
    {
        m_menu.SetActive( false );
        if(m_popOut!=null)
            m_popOut.SetActive( false );
    }

    public void OnMenuRequested()
    {
        m_menu.SetActive( true );
    }

    public void OnMemuClosed()
    {
        m_menu.SetActive( false );
    }

    IEnumerator PopOutCountdown()
    {
        m_popOut.SetActive( true );
        yield return new WaitForSecondsRealtime( m_popOutTime );
        Debug.Log( "pop out diminish" );
        m_popOut.SetActive( false );
        yield return null;
    }

    public void OnPopOutRequested()
    {
        StartCoroutine( PopOutCountdown() );
    }
}
