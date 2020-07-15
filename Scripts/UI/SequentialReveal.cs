using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequentialReveal : MonoBehaviour
{
    public GameObject[] m_objectsInOrder;
    private SetImageAlpha[] m_alphas;
    public float m_revealTime = 1.5f;
    public float m_revealInterval = 1f;

    // Start is called before the first frame update
    void Start()
    {
        m_alphas = new SetImageAlpha[m_objectsInOrder.Length];
        for ( int oIdx=0;  oIdx < m_objectsInOrder.Length;oIdx++)
        {
            if ( m_objectsInOrder[oIdx].GetComponent<SpriteRenderer>() != null )
            {
                m_alphas[oIdx] = m_objectsInOrder[oIdx].AddComponent<SetImageAlpha>();
                m_alphas[oIdx].rightX = 0;
            }
            m_objectsInOrder[oIdx].SetActive( false );
        }

        StartCoroutine( RevealObjectsInOrder() );
    }

    IEnumerator RevealObjectsInOrder()
    {
        for(int oIdx=0;oIdx<m_objectsInOrder.Length;oIdx++ )
        {
            yield return new WaitForSeconds( m_revealInterval );
            m_objectsInOrder[oIdx].SetActive( true );
            if(m_alphas[oIdx]!=null)
                StartCoroutine( RevealOneObject(m_alphas[oIdx]) );
            yield return new WaitForSeconds( m_revealTime );
        }
        yield return null;
    }

    IEnumerator RevealOneObject(SetImageAlpha alpha)
    {
        TimeCustom timer = new TimeCustom();
        timer.Start( m_revealTime );
        while(!timer.HasElapsed())
        {
            float alphaExtent = timer.GetElapsedNormalized();
            alpha.rightX = alphaExtent;
            yield return new WaitForEndOfFrame();
        }
        alpha.rightX = 1;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
