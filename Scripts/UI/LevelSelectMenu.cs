using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelSelectMenu : MonoBehaviour
{
    public int m_levelNum = 5;
    public Sprite[] m_levelSprites;
    public Sprite m_emptyButtonSprite;
    public Sprite m_colorButtonSprite;
    public Sprite m_glowButtonSprite;

    private Button[] m_levelButtons = null;

    private void Awake()
    {
        m_levelButtons = GetComponentsInChildren<Button>();
        if ( m_levelButtons.Length != m_levelNum )
        {
            Debug.LogError( "Level Select Number not match actual button Number" );
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        int levelProgress = PlayerPrefs.GetInt( GameSettings.s_levelProgress );
        if ( levelProgress > -1  )
            GetComponent<Image>().sprite = m_levelSprites[levelProgress];
        else GetComponent<Image>().sprite = m_levelSprites[m_levelNum];
        for ( int bID = 0; bID < m_levelButtons.Length; bID++ )
        {
            string name = m_levelButtons[bID].name;
            Image thisImage = m_levelButtons[bID].gameObject.GetComponent<Image>();
            if ( ( (int)name[name.Length - 1] ) > levelProgress + 49 )
            {
                m_levelButtons[bID].enabled = false;
                thisImage.sprite = m_emptyButtonSprite;
                continue;
            }
            else if ( ( (int)name[name.Length - 1] ) > levelProgress + 48 )
            {
                //m_levelButtons[bID].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2( 400, 400 );
                thisImage.sprite = m_glowButtonSprite;
            }
            else thisImage.sprite = m_colorButtonSprite;

            if ( levelProgress == 0 && name == "Level1" )
            {
                m_levelButtons[bID].onClick.AddListener( () => LoadLevelByName( "Intro" ) );
            }
            else
            {
                m_levelButtons[bID].onClick.AddListener( () => LoadLevelByName( name ) );
                Debug.Log( m_levelButtons[bID].onClick );
            }
        }
    }

    // Update is called once per frame
    public void LoadLevelByName(string levelName )
    {
        Debug.Log( "ready to load " + levelName );
        LoadingScreen.m_loadInstance.Show( SceneManager.LoadSceneAsync( levelName ) );
    }
}
