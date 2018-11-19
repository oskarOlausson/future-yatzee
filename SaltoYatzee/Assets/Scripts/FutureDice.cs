using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
	public class FutureDice : MonoBehaviour
	{
		private Turn _turn;
		public GuiDie GuiDiePrefab;
		private readonly Dictionary<Die, GuiDie> _modelToView = new Dictionary<Die, GuiDie>();
		public float Separation = 1f;
		
		private static Die ChildGetter(Die die, int nr)
		{
			while (nr > 0 && die != null)
			{
				die = die.NextRoll;
				nr--;
			}

			return die;
		}


		public void NewTurn(Turn turn)
		{
			if (turn == _turn) return;
			
			_turn = turn;
			
			foreach (var guiDie in _modelToView.Values)
			{
				Destroy(guiDie.gameObject);
			}
			
			_modelToView.Clear();

			var dice = turn.GetTopRow();
			for (var x = 0; x < dice.Length; x++)
			{
				var die = dice[x];
				
				var guiDie = Instantiate(GuiDiePrefab);
				guiDie.transform.position = GetPositionFromIndex((uint) x, 0);
				guiDie.transform.parent = transform.parent;
				
				_modelToView.Add(die, guiDie);
				
				guiDie.OnClick.AddListener(() =>
				{
					die.Toggle();
					UpdateDice();
				});

				var firstReRoll = Instantiate(GuiDiePrefab);
				firstReRoll.transform.parent = guiDie.transform;
				firstReRoll.transform.position = GetPositionFromIndex((uint) x, 1);
				firstReRoll.OnClick.AddListener(() =>
				{
					var child = ChildGetter(die, 1);
					if (child != null)
					{
						child.Toggle();
						UpdateDice();
					}
				});

				guiDie.Child = firstReRoll;

				var secondReRoll = Instantiate(GuiDiePrefab);
				secondReRoll.transform.parent = firstReRoll.transform;
				secondReRoll.transform.position = GetPositionFromIndex((uint) x, 2);
				firstReRoll.Child = secondReRoll;
			}
			
			UpdateDice();
		}

		private Vector2 GetPositionFromIndex(uint xi, uint yi)
		{
			var start = transform.position.x - (Separation * (-0.5f + 5 / 2f));
			var x = start + (Separation * xi);
			var y = transform.position.y + (Separation) - (Separation * yi);
			return new Vector2(x, y);
		}
		
		private void UpdateDice()
		{
			_turn.UpdateDice();
			
			foreach (var die in _turn.GetTopRow())
			{
				var guiDie = _modelToView[die];
				guiDie.Number = die.Number;

				var nextRoll = die.NextRoll;
				var nextGui = guiDie.Child;
				
				while (nextGui != null)
				{
					if (nextRoll == null)
					{
						nextGui.Hide();
					}
					else
					{
						nextGui.Number = nextRoll.Number;
						nextRoll = nextRoll.NextRoll;	
					}
					nextGui = nextGui.Child;
				}
			}
		}
	}
	
	
}
