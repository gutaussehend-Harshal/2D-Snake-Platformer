using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start()
    {
        gameObject.SetActive(true);
        StartCoroutine("ChangePosition");
    }

    public void RandomizedPos()
    {
        Bounds bounds = this.gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RandomizedPos();
        }
    }

    IEnumerator ChangePosition()
    {
        RandomizedPos();
        yield return new WaitForSeconds(5f);
        StartCoroutine("ChangePosition");
    }

    // void PowerUpTimer()
    // {
    //     StartCoroutine("ChangePosition");
    // }


}
