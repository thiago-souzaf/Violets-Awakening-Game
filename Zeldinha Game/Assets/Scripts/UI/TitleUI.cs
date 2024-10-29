using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
	[SerializeField] private GameObject fadeImage;
    [SerializeField] private float fadeDuration;


    private void Start()
    {
        fadeImage.SetActive(false);
    }
    public void StartGame()
	{
		fadeImage.SetActive(true);
		Invoke(nameof(LoadGameScene), fadeDuration);
    }

	private void LoadGameScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
