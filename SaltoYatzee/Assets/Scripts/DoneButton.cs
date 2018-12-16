using UnityEngine;
using UnityEngine.SceneManagement;

public class DoneButton : MonoBehaviour {
	private void OnMouseUpAsButton()
	{
		Carry.Points = FindObjectOfType<ScoreHandler>().GetTotal();
		var autoSave = FindObjectOfType<AutoSave>();
		autoSave.GameDone = true;
		SceneManager.LoadScene("HighScore");
	}
}