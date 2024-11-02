using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
	private static T instance;
	public static T Instance
	{
		get
		{
			return instance;
		}
	}

	public virtual void Awake()
	{
		RemoveDuplicates();
	}


	private static void SetupInstance()
	{
		GameObject gameObj = new GameObject();
		gameObj.name = typeof(T).Name;
		instance = gameObj.AddComponent<T>();

	}

	private void RemoveDuplicates()
	{
		if (instance == null)
		{
			instance = this as T;
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
