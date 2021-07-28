using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    // public GameObject foodPrefab;
    // public GameObject burnerPefab;
    public BoxCollider2D gridArea;
    public List<GameObject> foods = new List<GameObject>();
    [HideInInspector]
    public int gainerCount;

    void Start()
    {
        gainerCount = 0;
        Invoke("Spawn", 4f);
    }

    public void Spawn()
    {
        Bounds bounds = this.gridArea.bounds;
        int x = (int)Random.Range(bounds.min.x, bounds.max.x);
        int y = (int)Random.Range(bounds.min.y, bounds.max.y);

        if (gainerCount < 4)
        {
            Instantiate(foods[0], new Vector2(x, y), Quaternion.identity);
            gainerCount++;
        }
        else
        {
            int number = (int)Random.Range(0, foods.Count);
            Instantiate(foods[number], new Vector2(x, y), Quaternion.identity);
        }
        // this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Spawn();
        }
    }
}
