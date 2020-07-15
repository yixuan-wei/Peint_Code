using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneName;
    public string cornerSceneName = "Settings";

    public void onSceneChange()
    {
        LoadingScreen.m_loadInstance.Show( SceneManager.LoadSceneAsync( sceneName ) );
        Time.timeScale = 1.0f;
    }


    public void quit()
    {
        Application.Quit();
    }

    public void onCornerButtonRequested()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync( cornerSceneName );
        LoadingScreen.m_loadInstance.Show(op);
    }
}
