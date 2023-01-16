using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreTextTMP;
    [SerializeField] private Sprite[] _lifeSprite;
    [SerializeField] private Image _LivesImage;
    [SerializeField] private TMP_Text _gameOverTMP;
    [SerializeField] private TMP_Text _restartTMP;
    [SerializeField] private float _flickerRate = 0.5f;
    [SerializeField] private TMP_Text _quitGameTMP;
    [SerializeField] private bool _isQuitting;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _resumeButton;

    private GameManager _gameManager;

    void Start()
    {
        _scoreTextTMP.text = "Score: " + 0;
        _gameOverTMP.gameObject.SetActive(false);
        _restartTMP.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _quitGameTMP.gameObject.SetActive(false);
        _quitButton.gameObject.SetActive(false);
        _resumeButton.gameObject.SetActive(false);
    }

    public void UpdateScore(int playerScore)
    {
        _scoreTextTMP.text = "Score: " + playerScore;
    }

    public void QuitGameMenu()
    {
        Debug.Log("QuitGameMenu activated");
        IsQuitting();
        _quitGameTMP.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
        _resumeButton.gameObject.SetActive(true);
    }

    public void IsQuitting()
    {
        _isQuitting = true;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImage.sprite = _lifeSprite[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
            _gameManager.GameOver();
        }
    }

    void GameOverSequence()
    {
        _gameOverTMP.gameObject.SetActive(true);
        _restartTMP.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverTMP.gameObject.SetActive(false);
            yield return new WaitForSeconds(_flickerRate);
            _gameOverTMP.gameObject.SetActive(true);
            yield return new WaitForSeconds(_flickerRate);
        }
    }
}
