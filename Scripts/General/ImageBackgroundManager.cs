using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageBackgroundManager : MonoBehaviour
{
    private SetImageAlpha m_backgroundAlpha = null;
    private Coroutine m_changeCountdown = null;

    // Start is called before the first frame update
    private void Start()
    {
        m_backgroundAlpha = GetComponent<SetImageAlpha>();
        m_backgroundAlpha.rightX = 0;
        m_backgroundAlpha.leftX = 1;
        m_backgroundAlpha.topY = 1;
    }

    public void ChangeImageBackground( float changeTime )
    {
        m_changeCountdown = StartCoroutine( AlphaChange( changeTime ) );
    }

    public void ResetImageBackground()
    {
        if ( m_changeCountdown != null )
            StopCoroutine( m_changeCountdown );
        m_changeCountdown = null;
        m_backgroundAlpha.rightX = 0;
    }

    IEnumerator AlphaChange( float changeTime )
    {
        TimeCustom counter = new TimeCustom();
        counter.Start( changeTime );
        m_backgroundAlpha.rightX = 0;
        while ( !counter.HasElapsed() )
        {
            float alphaExtent = counter.GetElapsedNormalized();
            m_backgroundAlpha.rightX = alphaExtent;
            yield return new WaitForEndOfFrame();
        }
        m_backgroundAlpha.rightX = 1;
        yield return null;
    }
}
