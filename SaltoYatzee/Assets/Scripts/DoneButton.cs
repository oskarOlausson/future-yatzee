using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
	public class DoneButton : MonoBehaviour {

		
		private void OnMouseUpAsButton()
		{
			Carry.Points = FindObjectOfType<ScoreHandler>().GetTotal();
			SceneManager.LoadScene("HighScore");
		}
	}
}
