using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public GameObject titleScreen;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject pauseScreen;

    public bool isGameActive;
    public int score;
    private float spawnRate = 1f;
    public int totalLives = 3;

    public AudioSource mainAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        mainAudioSource = GetComponent<AudioSource>();
        mainAudioSource.Play();
        
        if (PlayerPrefs.HasKey("musicVolume")) {
            Load();
        }
        else {
            PlayerPrefs.SetFloat("musicVolume", 0.5f);
            Load();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGameActive) {
            pauseScreen.SetActive(true);
            isGameActive = false;
            StartCoroutine(AudioFade(mainAudioSource, 2, 0.1f));
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isGameActive) {
            pauseScreen.SetActive(false);
            isGameActive = true;
            StartCoroutine(AudioFade(mainAudioSource, 2, volumeSlider.value));
            Time.timeScale = 1f;
        }
    }
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int randomIndex = Random.Range(0, targets.Count);
            Instantiate(targets[randomIndex]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public int UpdateLives() {
        totalLives--;
        livesText.SetText("Lives: " + totalLives);

        return totalLives;
    }

    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        //scoreText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        isGameActive = false;

       // StartCoroutine(StartFade());
        StartCoroutine(AudioFade(mainAudioSource, 2, 0)); //StartCoroutine(FadeAudioSource.StartFade(AudioSource audioSource, float duration, float targetVolume)); //duration is in seconds not milliseconds
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        StartCoroutine(AudioFade(mainAudioSource, 2, ChangeVolume()));

        spawnRate /= difficulty;

        isGameActive = true;
        StartCoroutine(SpawnTarget());

        score = 0;
        UpdateScore(0);

        titleScreen.SetActive(false);
        scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
    }

    public IEnumerator AudioFade(AudioSource audioSource, float duration, float targetVolume) {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration) {
            currentTime += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    private float ChangeVolume() {
        float temp = AudioListener.volume = volumeSlider.value;
        Save();
        return temp;
    }
    private void Save() {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    private void Load() {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
}