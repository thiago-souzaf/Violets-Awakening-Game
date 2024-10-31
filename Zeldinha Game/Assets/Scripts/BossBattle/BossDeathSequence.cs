using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDeathSequence : MonoBehaviour
{
	public void LoadCreditsScene()
	{
		SceneManager.LoadScene("Scenes/CreditsScene");
		Debug.Log("aa");
    }
}
