using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading;

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
    
    [SerializeField] private Image _fuelBar;
    [SerializeField] private float fuelCooldownTime;

    [SerializeField] private TMP_Text _ammoCountTMP;
    
    private Player _player;

    private GameManager _gameManager;

    void Start()
    {
        _scoreTextTMP.text = "Score: " + 0;
        _ammoCountTMP.text = "Ammo: " + 15;

        _gameOverTMP.gameObject.SetActive(false);
        _restartTMP.gameObject.SetActive(false);
        _quitGameTMP.gameObject.SetActive(false);
        _quitButton.gameObject.SetActive(false);
        _resumeButton.gameObject.SetActive(false);
        
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL.");
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _fuelBar.fillAmount -= 1.0f / fuelCooldownTime * Time.deltaTime;
        }
        else
        {
            _fuelBar.fillAmount += 1.0f / fuelCooldownTime * Time.deltaTime;
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreTextTMP.text = "Score: " + playerScore;
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoCountTMP.text = "Ammo: " + ammo;
    }

    public void QuitGameMenu()
    {
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

    public void ResumePlaying()
    {
        Time.timeScale = 1;
        _quitGameTMP.gameObject.SetActive(false);
        _quitButton.gameObject.SetActive(false);
        _resumeButton.gameObject.SetActive(false);
        _isQuitting = false;
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
