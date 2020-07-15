using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public AudioMixer m_mixer = null;
    public float m_audioVolRange = 20f;
    public float m_bgVolumeStart = -20f;
    public float m_sfxVolumeStart = -20f;

    private float m_bgVol = 0f;
    private float m_sfxVol = 0f;

    private AudioSource m_sfxAudio = null;

    static public string s_bgVolume = "BackgroundVolume";
    static public string s_sfxVolume = "SFXVolume";

    private void Start()
    {
        m_mixer.GetFloat( s_bgVolume, out m_bgVol );
        m_mixer.GetFloat( s_sfxVolume, out m_sfxVol );
        Slider[] sliders = GetComponentsInChildren<Slider>();
        foreach(Slider s in sliders)
        {
            if ( s.name == "MusicSlider" )
                s.value = ( m_bgVol - m_bgVolumeStart ) / m_audioVolRange;
            else if ( s.name == "SFXSlider" )
                s.value = ( m_sfxVol - m_sfxVolumeStart ) / m_audioVolRange;
        }

    }

    private void Update()
    {
        if(m_sfxAudio==null)
        {
            AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource>();
            foreach ( AudioSource a in audios )
            {
                if ( a.name == "PlayerColor" )
                {
                    m_sfxAudio = a;
                    break;
                }
            }
        }
    }

    public void SetSfxVolume(float sfxLevel)
    {
        float trueSFX = m_sfxVolumeStart+sfxLevel*m_audioVolRange;
        m_mixer.SetFloat( s_sfxVolume, trueSFX );
        if(m_sfxAudio!=null)
            m_sfxAudio.Play();
    }

    public void SetBgVolume( float bgLevel )
    {
        float trueBg = m_bgVolumeStart+bgLevel*m_audioVolRange;
        m_mixer.SetFloat( s_bgVolume, trueBg );
    }
}
