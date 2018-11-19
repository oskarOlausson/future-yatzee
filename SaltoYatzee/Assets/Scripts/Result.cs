using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
	public class Result : MonoBehaviour
	{
		public UnityEvent OnSelected = new UnityEvent();
		public UnityEvent OnChanged = new UnityEvent();

		private Turn _turn;
		private Vector3 _startPosition;
		public Sprite[] DiceImages;
		private List<SpriteRenderer> _dice;
		private Vector3 _goal;
	
		private Sprite GetDiceImage(uint value)
		{
			return DiceImages[value-1];
		}

		private void NewValues()
		{
			var values = _turn.GetValues();

			for (var i = 0; i < _dice.Count; i++)
			{
				_dice[i].sprite = GetDiceImage(values[i]);
			}

			OnChanged.Invoke();
		}

		public void Awake()
		{
			_dice = GetComponentsInChildren<SpriteRenderer>().ToList();
			_dice.Remove(_dice[0]); // the background
		
			_turn = new Turn();
			_turn.OnChange.AddListener(NewValues);
		}

		public void Start()
		{
			_startPosition = transform.position;
			_goal = _startPosition;
		
			var background = GetComponentInChildren<OnClickListener>();
			background.OnClick.AddListener(() => OnSelected.Invoke());
			background.OnDrag.AddListener(() => FindObjectOfType<Dragger>().Dragging = this);
		
			NewValues();
		}

		public Turn GetTurn()
		{
			return _turn;
		}

		public uint[] GetValues()
		{
			return _turn.GetValues();
		}

		public void Follow(Vector3 pos)
		{
			pos.z = transform.position.z;
			_goal = pos;
		}

		public void ReturnToStartPosition()
		{
			_goal = _startPosition;
		}

		public void Update()
		{
			transform.position = Vector3.Lerp(transform.position, _goal, 0.8f);
		}
	}
}
