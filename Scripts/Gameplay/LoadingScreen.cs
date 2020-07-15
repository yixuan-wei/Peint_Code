using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen m_loadInstance;

    private AsyncOperation m_curLoadingOperation;
    private bool m_isLoading;

    private Slider m_slider;

    private void Awake()
    {
        if(m_loadInstance==null)
        {
            m_loadInstance = this;
            DontDestroyOnLoad( gameObject );
        }
        else
        {
            Destroy( gameObject );
            return;
        }

        m_slider = GetComponentInChildren<Slider>();
        Hide();
    }

    private void Update()
    {
        if(m_isLoading)
        {
            SetProgress( m_curLoadingOperation.progress );
            if(m_curLoadingOperation.isDone)
            {
                Hide();
            }
        }
    }

    private void SetProgress(float progress)
    {
        m_slider.value = progress;
    }

    public void Show(AsyncOperation loadingOperation)
    {
        gameObject.SetActive( true );
        m_curLoadingOperation = loadingOperation;
        SetProgress( 0f );
        m_isLoading = true;

    }

    public void Hide()
    {
        gameObject.SetActive( false );
        m_curLoadingOperation = null;
        m_isLoading = false;
    }
}
