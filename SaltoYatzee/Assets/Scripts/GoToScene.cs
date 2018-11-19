using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
	public class GoToScene : MonoBehaviour {
	
		public string SceneName;

		private void OnMouseUpAsButton()
		{
			SceneManager.LoadScene(SceneName);
		}
	}
}
