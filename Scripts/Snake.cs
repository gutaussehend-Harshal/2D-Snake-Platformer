using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    private Vector2 dir = Vector2.right;
    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;
    private bool upInput, DownInput, leftInput, rightInput;
    private int intialSize = 5;
    public BoxCollider2D wallArea;
    private float maxX, maxY, minX, minY;
    private int scoreCount;
    public Text scoreText;
    private float snakeFaceAngle;
    public GameObject pauseUI, gameOverUI;
    public string currentScene, menuScene;
    private bool isPaused;
    public Camera cam;

    public GameObject segmentGameObject;
    public GameObject shield;
    void Start()
    {
        snakeFaceAngle = -90f;

        Vector3 newPosition = transform.position;
        Bounds bounds = this.wallArea.bounds;
        maxX = bounds.max.x;
        minX = bounds.min.x;
        maxY = bounds.max.y;
        minY = bounds.min.y;

        ResetState();

        // InvokeRepeating("Movement", 2f, 0.1f);
    }

    private void Update()
    {

        upInput = Input.GetKeyDown(KeyCode.UpArrow);
        DownInput = Input.GetKeyDown(KeyCode.DownArrow);
        leftInput = Input.GetKeyDown(KeyCode.LeftArrow);
        rightInput = Input.GetKeyDown(KeyCode.RightArrow);

        changePos();

        ScreenWrap();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            OnPauseButtonClick();
        }
    }

    void ScreenWrap()
    {
        Vector3 newPosition = transform.position;
        if (newPosition.x >= maxX)
        {
            newPosition.x = -newPosition.x + 1f;
        }
        else if (newPosition.x <= minX)
        {
            newPosition.x = -newPosition.x - 1f;
        }
        if (newPosition.y >= maxY)
        {
            newPosition.y = -newPosition.y + 1f;
        }
        else if (newPosition.y <= minY)
        {
            newPosition.y = -newPosition.y - 1f;
        }
        transform.position = newPosition;
    }

    private void changePos()
    {

        if (upInput && (dir != Vector2.down))
        {
            dir = Vector2.up;
            snakeFaceAngle = 0;
        }
        else if (DownInput && (dir != Vector2.up))
        {
            dir = Vector2.down;
            snakeFaceAngle = 180;
        }
        else if (leftInput && (dir != Vector2.right))
        {
            dir = Vector2.left;
            snakeFaceAngle = 90;
        }
        else if (rightInput && (dir != Vector2.left))
        {
            dir = Vector2.right;
            snakeFaceAngle = -90;
        }

        // if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     dir = Vector2.right;
        //     snakeFaceAngle = -90;
        // }
        // else if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     dir = -Vector2.up;    // '-up' means 'down'
        //     snakeFaceAngle = 180;
        // }
        // else if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     dir = -Vector2.right; // '-right' means 'left'
        //     snakeFaceAngle = 90;
        // }
        // else if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     dir = Vector2.up;
        //     snakeFaceAngle = 0;
        // }

        this.transform.eulerAngles = new Vector3(0, 0, snakeFaceAngle);
    }

    // void Movement()
    // {
    //     ScreenWrap();

    //     for (int i = _segments.Count - 1; i > 0; i--)
    //     {
    //         _segments[i].position = _segments[i - 1].position;
    //     }

    //     this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) + dir.x,
    //                                           Mathf.Round(this.transform.position.y) + dir.y,
    //                                           0f);
    // }
    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        this.transform.position = new Vector3(Mathf.Round(this.transform.position.x) + dir.x,
                                               Mathf.Round(this.transform.position.y) + dir.y,
                                               0f);
        ScreenWrap();
    }

    public void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }
    public void ReduceSize()
    {
        Destroy(_segments[_segments.Count - 1].gameObject);
        _segments.RemoveAt(_segments.Count - 1);

        cam.GetComponent<FoodSpawner>().gainerCount--;

        if (_segments.Count <= intialSize)
        {
            GameOver();
        }
    }

    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        _segments.Add(this.transform);

        for (int i = 1; i < this.intialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector3.zero;

        scoreCount = 0;
        scoreText.text = "Score: " + scoreCount.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            scoreCount++;
            scoreText.text = "Score: " + scoreCount.ToString();
            Grow();
            Destroy(other.gameObject);
            cam.GetComponent<FoodSpawner>().Spawn();
        }
        else if (other.tag == "Burner")
        {
            scoreCount--;
            ReduceSize();
            if (scoreCount <= 0)
            {
                GameOver();
            }
            scoreText.text = "Score: " + scoreCount.ToString();
            Destroy(other.gameObject);
            cam.GetComponent<FoodSpawner>().Spawn();
        }
        else if (other.tag == "Obstacle")
        {
            GameOver();
        }
        else if (other.tag == "ScoreBooster")
        {
            scoreCount += 10;
            scoreText.text = "Score: " + scoreCount.ToString();
            Destroy(other.gameObject);
            cam.GetComponent<PowerUpSpawner>().Start();
        }
        else if (other.tag == "Shield")
        {
            // shield = true;
            // bc2d.enabled = false;
            // for (int i = _segments.Count - 1; i > 0; i--)
            // {
            //     _segments[i].GetComponent<BoxCollider2D>().enabled = false;
            // }
            Physics2D.IgnoreLayerCollision(8, 8, true);
            shield.gameObject.SetActive(true);
            Destroy(other.gameObject);
            cam.GetComponent<PowerUpSpawner>().Start();
            StartCoroutine("StartLayerCollision");
        }
    }

    IEnumerator StartLayerCollision()
    {
        yield return new WaitForSeconds(20f);
        // for (int i = _segments.Count - 1; i > 0; i--)
        // {
        //     _segments[i].GetComponent<BoxCollider2D>().enabled = true;
        // }
        shield.gameObject.SetActive(false);
        Physics2D.IgnoreLayerCollision(8, 8, false);
    }

    public void OnPauseButtonClick()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseUI.gameObject.SetActive(true);
        }
    }
    public void OnResumeButtonClick()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseUI.gameObject.SetActive(false);
    }

    public void OnMenuButtonClick()
    {
        SceneManager.LoadScene(menuScene);
    }
    public void OnRestartButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentScene);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
    }
}
