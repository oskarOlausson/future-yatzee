using UnityEngine;

public class GuiEntry : MonoBehaviour
{
	private TextMesh _text;
	
	private void Awake ()
	{
		_text = GetComponentInChildren<TextMesh>();
	}

	public void UpdateEntry(string hName, uint value)
	{
		_text.text = hName + " " + value;
	}

	public void Clear()
	{
		_text.text = "";
	}
}