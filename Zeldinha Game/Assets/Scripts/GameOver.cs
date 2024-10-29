using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
	[SerializeField] private GameObject objectToEnable;
    [SerializeField] private float gameOverDuration;
    private bool isTriggered;

    private void Start()
    {
        GlobalEvents.Instance.OnGameOver += OnGameOver;
    }

    private void OnGameOver(object sender, GameOverEventArgs e)
    {
        if (isTriggered) return;
        isTriggered = true;

        objectToEnable.SetActive(true);
        Invoke(nameof(ReloadScene), gameOverDuration);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
