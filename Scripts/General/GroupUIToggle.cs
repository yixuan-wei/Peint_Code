using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupUIToggle : MonoBehaviour
{
    private Toggle[] m_toggles;
    private GameSettings m_settings;

    private void Start()
    {
        m_settings = GameObject.FindObjectOfType<GameSettings>();

        Toggle[] toggles=GetComponentsInChildren<Toggle>();
        if ( toggles.Length != 3 )
            Debug.LogError( "Difficulty menu should have 3 toggles: easy, normal, hard" );
        m_toggles = new Toggle[3];
        for(int toggleIdx=0;toggleIdx<m_toggles.Length;toggleIdx++ )
        {
            int index = -1;

            if ( toggles[toggleIdx].name == "EasyToggle" )
                index = 0;
            else if ( toggles[toggleIdx].name == "NormalToggle" )
                index = 1;
            else if ( toggles[toggleIdx].name == "HardToggle" )
                index = 2;

            m_toggles[index] = toggles[toggleIdx];
            m_toggles[index].onValueChanged.AddListener( delegate
            {
                ToggleValueChanged( index );
            } );
        }

        int difficulty = PlayerPrefs.GetInt(GameSettings.s_difficulty,0);
        if ( difficulty < 0 || difficulty > 3 )
        {
            Debug.LogError( "difficulty in game setting is wrong" );
            return;
        }

        m_toggles[difficulty].isOn = true;
    }

    public void ToggleValueChanged(int difficulty)
    {
        m_settings.SetDifficulty( difficulty );
    }
}
