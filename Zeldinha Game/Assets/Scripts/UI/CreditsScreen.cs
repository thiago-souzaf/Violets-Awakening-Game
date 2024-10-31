using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScreen : MonoBehaviour
{
	public void LoadTitleScene()
	{
		SceneManager.LoadScene("Scenes/TitleScene");
    }
}
