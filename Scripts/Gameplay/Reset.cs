using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    public GameObject m_player = null;
    public GameSettings m_settings = null;
    private PlatformColor m_resetColor;
    public StartPoint[] m_startPlatforms = null;
    public Vector2 m_spawnOffset = new Vector2(0.0f,3.0f);

    private float m_deathTime = 0.5f;
    private Vector2 m_startPosition;
    private Coroutine m_respawnCo=null;
    private MusicManager m_sfxUtil = null;
    private Shooter[] m_shooters = null;

    private void Start()
    {
        if ( m_player.scene.name != null )
            Debug.LogError( "Player Prefab for Reset is not a prefab" );

        m_settings = GameSettings.FindObjectOfType<GameSettings>();
        m_startPosition = GameObject.FindGameObjectWithTag( "Player" ).transform.position;
        m_shooters = GameObject.FindObjectsOfType<Shooter>();
        m_sfxUtil = GameObject.FindObjectOfType<MusicManager>();

        int difficulty = PlayerPrefs.GetInt(GameSettings.s_difficulty);
        if(!m_settings.m_playerMetrics[difficulty].m_checkpointOn)
        {
            return;
        }
        
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
    private void UpdateShooters()
    {
        foreach(Shooter s in m_shooters)
        {
            s.RestartShooting();
        }
        Projectile[] projectiles = GameObject.FindObjectsOfType<Projectile>();
        foreach(Projectile p in projectiles)
        {
            Destroy( p.gameObject );
        }
    }

    IEnumerator RespawnCountdown()
    {
        Debug.Log( "Start respawn player" );
        UpdateStartPosition();
        GameObject thisPlayer = GameObject.FindGameObjectWithTag("Player");
        PlayerControl control = thisPlayer.GetComponent<PlayerControl>();
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
        Destroy(thisPlayer.GetComponent<Rigidbody2D>());
        Destroy( thisPlayer.GetComponent<BoxCollider2D>() );
        yield return new WaitForSeconds( m_deathTime );

        Destroy( thisPlayer );
        Debug.Log( "destroy the old player" );
        GameObject newPlayer = Instantiate(m_player, m_startPosition+m_spawnOffset,Quaternion.identity);
        newPlayer.GetComponent<PlayerControl>().m_color = m_resetColor;
        UpdateShooters(); 
        m_respawnCo = null;
        yield return null;
    }

    public void RespawnPlayer()
    {
        if ( m_respawnCo == null )
            m_respawnCo = StartCoroutine( RespawnCountdown() );
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        if(collision.gameObject.tag=="Player")
        {
            RespawnPlayer();
        }
    }
}
