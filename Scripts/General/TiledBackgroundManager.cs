using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledBackgroundManager : MonoBehaviour
{
    public float m_transitionTime = 2f;

    private ImageBackgroundManager[] m_imgBGManager;
    private float m_unitChangeTime = 1f;
    private Coroutine m_changeCountdown = null;

    IEnumerator ImagesBGChange()
    {
        for ( int img = 0; img < m_imgBGManager.Length; img++ )
        {
            m_imgBGManager[img].ChangeImageBackground( m_unitChangeTime );
            yield return new WaitForSeconds( m_unitChangeTime );
        }
        yield return null;
    }

    private void Start()
    {
        m_imgBGManager = GameObject.FindObjectsOfType<ImageBackgroundManager>();
        if ( m_imgBGManager.Length < 1 )
            Debug.LogError( "no image background manager in scene" );
        m_unitChangeTime = m_transitionTime / (float)m_imgBGManager.Length;
        MathUtilization.SortMonoBehaviorArrayWithPositionX( m_imgBGManager );
    }

    public void ResetAllImgBGs()
    {
        m_changeCountdown = null;
        for ( int img = 0; img < m_imgBGManager.Length; img++ )
        {
            m_imgBGManager[img].ResetImageBackground();
        }
    }

    public void FillAllImgBGs()
    {
        if ( m_changeCountdown == null )
            m_changeCountdown = StartCoroutine( ImagesBGChange() );
    }
}
