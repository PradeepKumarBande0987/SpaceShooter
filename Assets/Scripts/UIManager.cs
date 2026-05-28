using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _resetLevelText;

    [SerializeField]
    private Text _restartGameText;

    public float flickerSpeed = 0.5f;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private Sprite[] _liveSprites;

    private GameManager _gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _resetLevelText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);
        _gameManager = FindObjectOfType<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("Game Manager Is Null");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int playerLives)
    {
        _livesImg.sprite = _liveSprites[playerLives];

        if(playerLives == 0)
        {
            UpdateGameSq();
        }
    }

    private void UpdateGameSq()
    {
        _gameOverText.gameObject.SetActive(true);
        _resetLevelText.gameObject.SetActive(true);
        _restartGameText.gameObject.SetActive(true);
        _gameManager.GameOver();
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine() {
        while (true) {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(flickerSpeed);
            _gameOverText.text = "";
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
