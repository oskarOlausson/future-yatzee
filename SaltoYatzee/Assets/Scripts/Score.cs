using UnityEngine;
using UnityEngine.Events;

public class Score : MonoBehaviour
{
	private TextMesh _typeText;
	private TextMesh _pointsText;
	private BoxCollider2D _dropZone;
	public UnityEvent OnValueChanged = new UnityEvent();

	private Type _type;
	private uint _value;
	private BoxCollider2D _ownCollider;

	public Type MyType
	{
		set
		{
			_type = value;
			if (_typeText)
			{
				_typeText.text = TypeFunctions.NameOf(_type);
				name = _typeText.text;
			}
		}
	}
		
	private void Start()
	{
		var meshes = GetComponentsInChildren<TextMesh>();
		_typeText = meshes[0];
		_typeText.text = TypeFunctions.NameOf(_type);
		name = _typeText.text;
			
		_pointsText = meshes[1];
		_pointsText.text = "" + _value;

		_dropZone = GetComponent<BoxCollider2D>();
		UnSelect();
	}

	public uint Points
	{
		set
		{
			_value = value;
			if (_pointsText)
			{
				_pointsText.text = "" + value;
			}
		}

		get
		{
			return _value;
		}
	}
		
	public void UpdateValues(uint[] values)
	{
		Points =  TypeFunctions.PointsFor(values, _type);
		OnValueChanged.Invoke();
	}

	public void UnSelect()
	{
		_pointsText.text = "-";
		_value = 0;
		OnValueChanged.Invoke();
	}

	public BoxCollider2D DropZoneCollider()
	{
		return _dropZone;
	}

	private void OnMouseUpAsButton()
	{
		var resultHandler = FindObjectOfType<ResultHandler>();
		var selected = resultHandler.Selected;

		if (selected)
		{
			resultHandler.Connect(selected, this);
		}
	}

	public Vector3 DropZonePosition()
	{
		return transform.GetChild(0).position;
	}
}