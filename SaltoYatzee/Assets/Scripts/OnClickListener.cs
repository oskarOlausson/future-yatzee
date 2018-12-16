using UnityEngine;
using UnityEngine.Events;

public class OnClickListener : MonoBehaviour {
		
	public readonly UnityEvent OnClick = new UnityEvent();
	public readonly UnityEvent OnDrag = new UnityEvent();

	private void OnMouseUpAsButton()
	{
		OnClick.Invoke();
	}

	private void OnMouseDrag()
	{
		OnDrag.Invoke();
	}
}