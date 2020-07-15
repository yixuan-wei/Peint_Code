using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public string m_mainMenuName = "MainMenu";
    private PlayerInputScreen m_input = null;
    private GameObject m_pauseMenu  =null;
    private MusicManager m_sfx = null;
    private TutorialManager m_tutorial = null;
    private bool m_tutorialOn = false;

    private void Start()
    {
        m_tutorial = GameObject.FindObjectOfType<TutorialManager>();
        m_sfx = GameObject.FindObjectOfType<MusicManager>();
        m_pauseMenu = GameObject.Find( "PauseMenu" );
        Button[] pauseButtons = m_pauseMenu.GetComponentsInChildren<Button>();
        for(int bID=0;bID<pauseButtons.Length;bID++ )
        {
            pauseButtons[bID].onClick.AddListener(m_sfx.PlayUIButtonSFX);
        }
        m_pauseMenu.SetActive( false );
        m_input = GameObject.FindObjectOfType<PlayerInputScreen>();
    }

    // Update is called once per frame
    void Update()
    {        
        if ( m_input == null )
            m_input = GameObject.FindObjectOfType<PlayerInputScreen>();
        if(m_sfx==null)
            m_sfx = GameObject.FindObjectOfType<MusicManager>();

    }

    private void LateUpdate()
    {
        if ( m_tutorial != null )
            m_tutorialOn = m_tutorial.m_isMenuOn;
    }

    public void GoToMainMenu()
    {
       LoadingScreen.m_loadInstance.Show(SceneManager.LoadSceneAsync( m_mainMenuName ));
    }

    public void ReloadCurrentScene() {
        Time.timeScale = 1f;
        LoadingScreen.m_loadInstance.Show(SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));

    }

    public void TogglePause()
    {
        if ( m_input != null && !m_tutorialOn)
        {
            if ( !m_pauseMenu.activeInHierarchy && Time.timeScale==1f  )
            {
                Time.timeScale = 0f;
                m_pauseMenu.SetActive( true );
                m_sfx.StopAllSFX();
                Debug.Log( "sfx stop" );
            }
            else if( m_pauseMenu.activeInHierarchy && Time.timeScale==0f)
            {
                m_input.ClearTouchStates();
                m_pauseMenu.SetActive( false );
                Time.timeScale = 1f;
            }
        }
    }
}
