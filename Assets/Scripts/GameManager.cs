using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public int livesTotal = 3;
    public int bricksTotal = 20;

    public float resetDelay = 1f;

    public Text livesUiText;
    public GameObject gameOverUi;
    public GameObject youWonUi;

    public List<Brick> brickTypePrefabs;
    public GameObject paddlePrefab;
    public GameObject bricksGroup;

    public GameObject brickParticles;
    public GameObject deathParticles;

    public static GameManager instance = null;

    public bool inputAllowed = false;

    const int _defaultMaxLives = 3;
    GameObject _clonePaddle;

    // Use this for pre-initialization
    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        Setup();
    }

    public void Setup()
    {
        Debug.Log("Setting up level.");

        livesTotal = _defaultMaxLives;

        HideUiObjects();

        SetupPaddle();
        SetupBricks();

        inputAllowed = true;
    }

    private void HideUiObjects()
    {
        gameOverUi.gameObject.SetActive(false);
        youWonUi.gameObject.SetActive(false);
    }

    private void SetupBricks()
    {
        Instantiate(bricksGroup, bricksGroup.transform.position, Quaternion.identity);
    }

    private void SetupPaddle()
    {
        Debug.Log("Setting up paddle.");
        _clonePaddle = Instantiate(paddlePrefab, paddlePrefab.transform.position, Quaternion.identity) as GameObject;
    }

    void CheckGameEnd()
    {
        if (bricksTotal < 1)
        {
            // Win
            StartCoroutine(YouWinCoroutine());
        }

        if (livesTotal < 1)
        {
            // Lose
            StartCoroutine(YouLoseCoroutine());
        }
    }

    IEnumerator YouLoseCoroutine()
    {
        yield return StartCoroutine(ShowUiObject(gameOverUi));
        yield return StartCoroutine(HideUiObkect(gameOverUi));
        yield return StartCoroutine(RestartGame());
    }

    IEnumerator YouWinCoroutine()
    {
        yield return StartCoroutine(ShowUiObject(youWonUi));
        yield return StartCoroutine(HideUiObkect(youWonUi));
        yield return StartCoroutine(RestartGame());
    }

    IEnumerator ShowUiObject(GameObject uiObject, float waitTime = 1f)
    {    
        uiObject.SetActive(true);
        Time.timeScale = 0.25f;
        yield return new WaitForSeconds(waitTime);
    }

    IEnumerator HideUiObkect(GameObject uiObject, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);
        Time.timeScale = 1f;
        uiObject.SetActive(false);
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(resetDelay);

        Time.timeScale = 1f;

        yield return null;

        livesTotal = _defaultMaxLives;
        HideUiObjects();

        Debug.Log("Restarting.");
        inputAllowed = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseLife()
    {
        StartCoroutine(LoseLifeCoroutine());
    }

    IEnumerator LoseLifeCoroutine()
    {
        inputAllowed = false;

        // Reduce lives by 1
        livesTotal--;

        Debug.Log("Lives left: " + livesTotal);
        UpdateLivesUIText();

        // Paddle is destroyed every time a life is lost, then is re-created
        if (livesTotal > 0 && bricksTotal > 0)
            StartCoroutine(DestroyPaddle());

        yield return new WaitForSeconds(resetDelay);

        if (livesTotal > 0 && bricksTotal > 0)
        {
            inputAllowed = true;
            SetupPaddle();
        }

        CheckGameEnd();
    }

    private IEnumerator DestroyPaddle()
    {
        GameObject particles = Instantiate(deathParticles, _clonePaddle.gameObject.transform.Find("Paddle Mesh").transform.position, Quaternion.identity);
        Destroy(_clonePaddle); // perhaps, intead of destroying, we should displace and reposition the paddle when it's needed again

        yield return new WaitForSeconds(0.5f);

        Destroy(particles);
    }

    private void UpdateLivesUIText()
    {
        livesUiText.text = "Lives: " + livesTotal;
        Debug.Log("Changing lives text. " + livesUiText.text);
    }

    public void DestroyBrick()
    {
        // This will be called from the brick game object that's to be destroyed
        bricksTotal--;
        CheckGameEnd();
    }
}
