using System.Collections;
using UnityEngine;


public class BasicEnemy : MonoBehaviour
{
    public float damagePlayerZPos = -8f;

    GameState gameState;
    GameSettings settings;

    private void Start()
    {
        settings = GameSettings.Instance;
        gameState = GameState.Instance;
        Debug.DrawRay(transform.position, -Vector3.forward * 100f);

    }

    void Update()
    {
        transform.Translate(-Vector3.forward * Time.deltaTime * 10f);
        if (transform.position.z <= damagePlayerZPos)
        {
            GameState.Instance.ModifyHealth(-10);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (HitsPlayerBullet(collision))
        {
            gameState.EarnGold(20);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (HitsPlayerUnit(collision))
        {
            collision.gameObject.GetComponent<PlayerUnitOldSchool>().KilledByEnemy();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    bool HitsPlayerBullet(Collider collision) => settings.IsOnPlayerButtleLayer(collision);
    bool HitsPlayerUnit(Collider collision) => settings.IsOnPlayerUnitLayer(collision);
}
