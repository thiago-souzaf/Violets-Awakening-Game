using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	// Constants
	private static readonly string KEY_HIGHEST_SCORE = "HighestScore";

	public bool IsGameOver { get; private set; }

	[Header("Audio")]
	[SerializeField] private AudioSource music;
	[SerializeField] private AudioSource deadSFX;

	[Header("Score")]
	[SerializeField] private float score;
	[SerializeField] private int highestScore;

	[Space]
	[SerializeField] private float reloadDelay = 6f;

    private void Start()
    {
		score = 0;
		highestScore = PlayerPrefs.GetInt(KEY_HIGHEST_SCORE, 0);
    }

    private void Update()
    {
		if (!IsGameOver)
		{
			score += Time.deltaTime;
		}
    }

	public int GetScore()
	{
		return (int)Mathf.Floor(score);
	}

	public int GetHighestScore()
	{
		return highestScore;
	}

    public void EndGame()
	{
		if (IsGameOver) return;

		IsGameOver = true;

		// Stop music
		music.Stop();

		// Play dead sfx
		deadSFX.Play();

		if (GetScore() > GetHighestScore())
		{
			highestScore = GetScore();
			PlayerPrefs.SetInt(KEY_HIGHEST_SCORE, highestScore);
		}

		Invoke(nameof(ReloadScene), reloadDelay);
	}

	private void ReloadScene()
	{
		string sceneName = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(sceneName);
	}

}
