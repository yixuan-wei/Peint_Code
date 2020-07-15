using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public bool m_activated = false;
    public PlatformColor m_startColor = PlatformColor.PINK;
    private BoxCollider2D m_collider = null;

    private void Start()
    {
        m_collider = gameObject.GetComponent<BoxCollider2D>();
        //if ( m_activated )
        //    m_collider.isTrigger = false;
        //else m_collider.isTrigger = true;
    }
    
    private void OnCollisionEnter2D( Collision2D collision )
    {
        //may be combine the start point to a platform?
        //to avoid "floating in air" with inactivated platforms
        if(collision.gameObject.tag=="Player" && collision.gameObject.transform.position.y>m_collider.size.y*.5f+transform.position.y)
        {
            Debug.Log( "start point accepted" );
            m_activated = true;
            //m_collider.isTrigger = false;
        }
    }
}
