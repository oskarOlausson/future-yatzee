using UnityEngine;

public class DieJump : MonoBehaviour
{
	private float _time;
	private float _ySpeed;
	public float JumpSpeed = 4f;
	public float Gravity = -.5f;
	private float _startY;
		 
	private void Start ()
	{

		_time = Random.Range(2, 3);
		_startY = transform.position.y;
	}
	
	private void Update ()
	{
		_time -= Time.deltaTime;

		if (_time < 0)
		{
			_time = Random.Range(1, 4);
			_ySpeed = Random.Range(JumpSpeed / 4, JumpSpeed);
		}

		_ySpeed += Gravity;
		transform.position += new Vector3(0, _ySpeed, 0);

		if (transform.position.y < _startY)
		{
			transform.position = new Vector3(transform.position.x, _startY, transform.position.z);
		}
	}
}