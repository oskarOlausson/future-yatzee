using UnityEngine;

public class ScaleOnHover : MonoBehaviour {
	
	private float _goal = 1;
	private float _scale = 1;
	private Vector3 _startScale;

	private void Start()
	{
		_startScale = transform.localScale;
	}

	private void OnMouseEnter()
	{
		_goal = 1.2f;
	}
	
	private void OnMouseExit()
	{
		_goal = 1f;
	}

	private void Update()
	{
		_scale = Mathf.Lerp(_scale, _goal, 0.1f);
		transform.localScale = _startScale * _scale;
	}
}