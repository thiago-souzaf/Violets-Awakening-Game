using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
	[SerializeField] private GameObject fadeImage;
    [SerializeField] private float fadeDuration;
    [SerializeField] private AudioSource musicPlayer;


    private void Start()
    {
        fadeImage.SetActive(false);
    }
    public void StartGame()
	{
		fadeImage.SetActive(true);
		Invoke(nameof(LoadGameScene), fadeDuration);
        StartCoroutine(FadeAudioSource.StartFade(musicPlayer, 0, fadeDuration));
    }

	private void LoadGameScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
