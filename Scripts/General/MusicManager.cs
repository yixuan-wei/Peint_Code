using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioClip m_jumpSFX = null;
    public AudioClip m_runSFX = null;
    public AudioClip m_deadSFX = null;
    public AudioClip m_colorSFX = null;
    public AudioClip m_menuBgMusic = null;
    public AudioClip m_playBgMusic = null;

    private AudioSource m_playerAudio = null;
    private AudioSource m_colorAudio = null;
    private AudioSource m_uiAudio = null;
    private AudioSource m_bgAudio =null;

    private int m_curSceneID = -1;
    private bool m_isPlaying = false;

    private void Awake()
    {
        AudioSource[] audioSources = gameObject.GetComponentsInChildren<AudioSource>();
        for(int asID=0;asID<audioSources.Length;asID++ )
        {
            if ( audioSources[asID].name == "PlayerMove" )
            {
                m_playerAudio = audioSources[asID];
                m_playerAudio.volume = .7f;
            }
            else if ( audioSources[asID].name == "PlayerColor" )
            {
                m_colorAudio = audioSources[asID];
            }
            else if ( audioSources[asID].name == "UI" )
            {
                m_uiAudio = audioSources[asID];
                m_uiAudio.volume = .4f;
            }
            else if ( audioSources[asID].name == "Background" )
            {
                m_bgAudio = audioSources[asID];
            }
        }
        UpdateCurrentSceneButtons();
    }

    void UpdateCurrentSceneButtons()
    {
        Button[] allCurButtons = GameObject.FindObjectsOfType<Button>();
        for ( int bID = 0; bID < allCurButtons.Length; bID++ )
        {
            allCurButtons[bID].onClick.AddListener( PlayUIButtonSFX );
        }
        m_curSceneID = SceneManager.GetActiveScene().buildIndex;
    }

    private void LateUpdate()
    {
        int thisSceneID=SceneManager.GetActiveScene().buildIndex;
        if ( m_curSceneID != thisSceneID )
        {
            StopAllSFX();
            UpdateCurrentSceneButtons();
            if ( m_curSceneID > 6 )
                m_isPlaying = true;
            else m_isPlaying = false;
            if ( (m_isPlaying && m_bgAudio.clip == m_menuBgMusic)
                || (!m_isPlaying && m_bgAudio.clip == m_playBgMusic) )
                ToggleBgMusic();
        }
    }

    void ToggleBgMusic()
    {
        m_bgAudio.Stop();
        if ( m_isPlaying )
        {
            m_bgAudio.clip = m_playBgMusic;
            m_bgAudio.volume = .9f;
        }
        else if ( !m_isPlaying )
        {
            m_bgAudio.clip = m_menuBgMusic;
            m_bgAudio.volume = 1.0f;
        }
        m_bgAudio.Play();
    }

    public void PlayJumpSFX()
    {       
        m_playerAudio.Stop();
        m_playerAudio.clip = m_jumpSFX;
        m_playerAudio.Play();
    }

    public void PlayRunSFX()
    {
        if ( m_playerAudio.clip==m_jumpSFX|| !m_playerAudio.isPlaying )
        {
            m_playerAudio.Stop();
            m_playerAudio.clip = m_runSFX;
            m_playerAudio.Play();
        }
    }

    public void PlayDeadSFX()
    {
        m_playerAudio.Stop();
        m_playerAudio.clip = m_deadSFX;
        m_playerAudio.Play();
    }

    public void PlayColorSFX()
    {
        m_colorAudio.Stop();
        m_colorAudio.Play();
    }

    public void StopAllSFX()
    {
        m_colorAudio.Stop();
        m_playerAudio.Stop();
    }

    public void PlayUIButtonSFX()
    {
        m_uiAudio.Stop();
        m_uiAudio.Play();
    }
}
