using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour {

	private void Start()
	{
		var loadAndSave = FindObjectOfType<LoadAndSave>();
		if (!loadAndSave.HasGameSaved())
		{
			Destroy(gameObject);
		}
	}

	private void OnMouseUpAsButton()
	{
		SceneManager.LoadScene("TheGame");
	}
}
