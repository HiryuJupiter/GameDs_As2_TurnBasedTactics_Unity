using System.Collections;
using UnityEngine;


public class BasicEnemy : MonoBehaviour
{
    const float moveDist = 3f;

    public float damagePlayerZPos = -8f;

    GameState gameState;
    GameSettings settings;
    EnemySpawner spawner;

    public void Initialize(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    private void Start()
    {
        settings = GameSettings.Instance;
        gameState = GameState.Instance;
        Debug.DrawRay(transform.position, -Vector3.forward * 100f);
    }

    void Update()
    {
        //transform.Translate(-Vector3.forward * Time.deltaTime * 10f);
        
    }

    private void DestroySelf()
    {
        alive = false;
        spawner.RemoveEnemy(this);
        Destroy(gameObject);
    }


    //public void Move()
    //{
    //    StartCoroutine(DoMove());
    //}
    bool alive = true;
    public IEnumerator DoMove()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + new Vector3(0f, 0f, -moveDist);
        float t = 0;

        while (t < 1f)
        {
            if (!alive)
                yield break;

            t += Time.deltaTime * 2f;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        transform.position = endPos;

        if (transform.position.z <= damagePlayerZPos)
        {
            GameState.Instance.ModifyHealth(-10);
            DestroySelf();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (HitsPlayerBullet(collision))
        {
            gameState.EarnGold(20);
            if (collision != null)
            {
                if (collision.gameObject != null)
                {
                    Destroy(collision.gameObject);
                }
            }
            DestroySelf();
        }
        else if (HitsPlayerUnit(collision))
        {
            collision.gameObject.GetComponent<PlayerUnitOldSchool>().KilledByEnemy();
            if (collision != null)
            {
                if (collision.gameObject != null)
                {
                    Destroy(collision.gameObject);
                }
            }
            DestroySelf();
        }
    }

    bool HitsPlayerBullet(Collider collision) => settings.IsOnPlayerButtleLayer(collision);
    bool HitsPlayerUnit(Collider collision) => settings.IsOnPlayerUnitLayer(collision);
}
