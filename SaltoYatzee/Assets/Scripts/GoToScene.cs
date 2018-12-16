using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour {
	
	public string SceneName;

	private void OnMouseUpAsButton()
	{
		SceneManager.LoadScene(SceneName);
	}
}