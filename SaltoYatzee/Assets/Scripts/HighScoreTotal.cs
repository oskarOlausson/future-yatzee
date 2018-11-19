using UnityEngine;

namespace Scripts
{
	[RequireComponent(typeof(TextMesh))]
	public class HighScoreTotal : MonoBehaviour {

		// Use this for initialization
		private void Start ()
		{
			GetComponent<TextMesh>().text = "" + Carry.Points;
		}
	}
}
