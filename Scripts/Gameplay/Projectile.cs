using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_speed = 3f;
    public float m_livingTime = 1f;
    public float m_explosionTime = .8f;

    private BoxCollider2D m_boxCollider = null;
    private ParticleSystem m_particles = null;
    private float m_lifeCounter = 0f;

    private void Awake()
    {
        m_boxCollider = GetComponent<BoxCollider2D>();
        m_particles = GetComponentInChildren<ParticleSystem>();
        m_particles.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_lifeCounter>m_livingTime)
        {
            Destroy( gameObject );
        }
        m_lifeCounter += Time.deltaTime;
        Vector2 position = transform.position;
        position.x += -m_speed*Time.deltaTime;
        transform.position = position;
    }

    IEnumerator ReadyToDie()
    {
        Destroy( GetComponent<SpriteRenderer>() );
        m_speed = 0f;
        m_boxCollider.enabled = false;
        m_particles.Play();
        yield return new WaitForSeconds( m_explosionTime );
        Destroy( gameObject );
        yield return null;
    }

    private void FixedUpdate()
    {
        LayerMask mask = LayerMask.GetMask("Player","Platform");
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask( mask );
        Collider2D[] m_results = new Collider2D[10];
        int num = Physics2D.OverlapCollider(m_boxCollider,filter,m_results);
        if(num!=0)
        {
            for(int id=0;id<num;id++ )
            {
                if(m_results[id].gameObject.layer==LayerMask.NameToLayer("Platform"))
                {
                    Debug.Log( "Hit Platform" );
                    StartCoroutine( ReadyToDie() );
                    break;
                }
                else if(m_results[id].gameObject.layer==LayerMask.NameToLayer("Player"))
                {
                    Debug.Log( "Hit Player" );
                    Reset r = GameObject.FindObjectOfType<Reset>();
                    r.RespawnPlayer();
                    StartCoroutine( ReadyToDie() );
                    break;
                }
            }
        }
    }
}
