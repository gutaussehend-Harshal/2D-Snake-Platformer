using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public List<GameObject> powerUps = new List<GameObject>();
    public void Start()
    {
        Invoke("PowerUpSpawn", 10f);
    }

    public void PowerUpSpawn()
    {
        Bounds bounds = this.gridArea.bounds;
        int x = (int)Random.Range(bounds.min.x, bounds.max.x);
        int y = (int)Random.Range(bounds.min.y, bounds.max.y);

        int number = (int)Random.Range(0, powerUps.Count);
        Instantiate(powerUps[number], new Vector2(x, y), Quaternion.identity);
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.tag == "Player")
    //     {
    //         PowerUpSpawn();
    //     }
    // }

    IEnumerator TimeForSpawning()
    {
        float nextSpawnTime = Random.Range(5f, 20f);
        yield return new WaitForSeconds(nextSpawnTime);
        PowerUpSpawn();
    }
}
