using UnityEngine;

public class Spawner : MonoBehaviour
{
    public MonsterPool pool;
    public MonsterDatabase database;
    public Transform player;

    public float spawnInterval = 2f;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;

            //random monster 
            MonsterData data = database.monsters[Random.Range(0, database.monsters.Length)];

            // posistion of player 
            Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * 10f;

            pool.Get(data.type, spawnPos);
        }
    }
}
