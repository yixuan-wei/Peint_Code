using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTutorial : MonoBehaviour
{
    public GameObject m_player = null;
    private PlatformColor m_resetColor;
    public StartPoint[] m_startPlatforms = null;
    public Vector2 m_spawnOffset = new Vector2(0.0f,3.0f);

    private float m_deathTime = 0.5f;
    private Vector2 m_startPosition;
    private Coroutine m_respawnCo=null;
    private MusicManager m_sfxUtil = null;

    private void Start()
    {
        m_sfxUtil = GameObject.FindObjectOfType<MusicManager>();
        m_startPlatforms = GameObject.FindObjectsOfType<StartPoint>();
        if ( m_startPlatforms.Length < 1 )
            Debug.LogError( "No start Platforms found" );
    }

    private void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if ( players.Length > 1 )
        {
            for ( int pID = 1; pID < players.Length; pID++ )
                GameObject.Destroy( players[pID] );
        }
    }

    void UpdateStartPosition()
    {
        float latestX = -100.0f;
        foreach (StartPoint startPlatform in m_startPlatforms)
        {
            if(startPlatform.m_activated && startPlatform.transform.position.x>=latestX)
            {
                m_resetColor = startPlatform.m_startColor;
                m_startPosition = startPlatform.transform.position;
                latestX = m_startPosition.x;
            }
        }
    }

    IEnumerator RespawnCountdown()
    {
        UpdateStartPosition();
        GameObject thisPlayer = GameObject.FindGameObjectWithTag("Player");
        PlayerControlTutorial control = thisPlayer.GetComponent<PlayerControlTutorial>();
        control.m_jumpStartSpeed = 0.0f;
        control.m_moveSpeed = 0.0f;
        Destroy( control );
        m_sfxUtil.PlayDeadSFX();
        Animator[] animators = thisPlayer.GetComponentsInChildren<Animator>();
        for(int aID=0;aID<animators.Length;aID++ )
        {
            animators[aID].SetBool( "Death", true );
        }
        SpriteRenderer[] renderers = thisPlayer.GetComponentsInChildren<SpriteRenderer>();
        for ( int rID = 0; rID < renderers.Length; rID++ )
        {
            renderers[rID].sortingOrder = -3;
            if ( renderers[rID].name == "Visual_Color" )
                renderers[rID].sortingOrder = -4;
        }
        yield return new WaitForSeconds( m_deathTime );

        GameObject.Destroy( thisPlayer );
        GameObject newPlayer = Instantiate(m_player, m_startPosition+m_spawnOffset,Quaternion.identity);
        newPlayer.GetComponent<PlayerControlTutorial>().m_color = m_resetColor;
        m_respawnCo = null;
        yield return null;
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        if(collision.gameObject.tag=="Player")
        {
            if ( m_respawnCo == null )
                m_respawnCo = StartCoroutine( RespawnCountdown() );
        }
    }
}
