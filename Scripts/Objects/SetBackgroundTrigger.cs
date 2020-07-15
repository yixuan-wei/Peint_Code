using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBackgroundTrigger : MonoBehaviour
{
    private ScreenBackgroundManager m_bgManager = null;

    private void Awake()
    {
        m_bgManager = GameObject.FindObjectOfType<ScreenBackgroundManager>();
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if(collision.gameObject.tag=="Player")
        {
            for(int cID=0;cID<(int)PlatformColor.NUM_COLORS;cID++ )
            {
                m_bgManager.UpdateBackgrounds( (PlatformColor)cID );
            }
        }
    }
}
