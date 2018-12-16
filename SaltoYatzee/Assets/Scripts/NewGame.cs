using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour {

	private void OnMouseUpAsButton()
	{
		var loadAndSave = FindObjectOfType<LoadAndSave>();
		loadAndSave.ClearCurrentProgress();
		SceneManager.LoadScene("TheGame");
	}
}
