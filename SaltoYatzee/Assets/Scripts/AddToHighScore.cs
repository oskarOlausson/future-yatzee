using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
	public class AddToHighScore : MonoBehaviour
	{
		public InputField TextInput;

		public void Submit()
		{
			if (TextInput.text.Trim().Length == 0) return;
			FindObjectOfType<HighScore>().SaveHighScore(TextInput.text);
			Destroy(TextInput.gameObject);
			Destroy(gameObject);
		}
	}
}
