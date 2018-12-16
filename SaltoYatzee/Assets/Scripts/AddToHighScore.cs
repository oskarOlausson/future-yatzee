using UnityEngine;
using UnityEngine.UI;

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