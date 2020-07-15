using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public string m_nextScene = "Level2";

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_nextScene);
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    private void OnCollisionEnter2D( Collision2D collision )
    {
        if(collision.gameObject.tag=="Player")
        {
            int levelNum = SceneManager.GetActiveScene().buildIndex - 6;
            if(levelNum>PlayerPrefs.GetInt(GameSettings.s_levelProgress))
                PlayerPrefs.SetInt( GameSettings.s_levelProgress, levelNum );
            Debug.Log( PlayerPrefs.GetInt( GameSettings.s_levelProgress ) );
            StartCoroutine( LoadAsyncScene() );
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
