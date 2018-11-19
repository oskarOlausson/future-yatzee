using UnityEngine;

namespace Scripts
{
	public class Extra : MonoBehaviour
	{
		private TextMesh _typeText;
		private TextMesh _pointsText;
		private uint _value;
			
		public string Type
		{
			set
			{
				_typeText.text = value;
			}
		}

		public uint Points
		{
			set
			{
				_value = value;
				_pointsText.text = "" + value;
			}

			get { return _value; }
		}
		
		private void Awake()
		{
			var meshes = GetComponentsInChildren<TextMesh>();
			_typeText = meshes[0];
			_pointsText = meshes[1];			
		}
	}
}
