using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
	public class ScoreHandler : MonoBehaviour
	{

		private readonly Type[] _scoreTypes =
		{ Type.Ettor, Type.Tvåor, Type.Treor, Type.Fyror, Type.Femmor, Type.Sexor
			, Type.Par, Type.Tvåpar, Type.Triss, Type.Fyrtal
			, Type.Liten_stege, Type.Stor_stege, Type.Chans
			, Type.Kåk, Type.Yatzee
		};
	
		public Score ScorePrefab;
		public Extra ExtraPrefab;
		public float Separation = 0.5f;
		public float RowSeparation = 0.5f;
		private readonly List<Score> _scores = new List<Score>();
		private Extra _bonus;

		public TextMesh Total;
	
		private void Start ()
		{
			const int newRowIndex = 6;
		
			var sum = Instantiate(ExtraPrefab);
			sum.transform.parent = transform;
			sum.transform.localPosition = PositionFromIndex(6, 0);
			sum.Type = "Summa";
		
			_bonus = Instantiate(ExtraPrefab);
			_bonus.transform.parent = transform;
			_bonus.transform.localPosition = PositionFromIndex(7, 0);
			_bonus.Type = "Bonus";

			for (uint i = 0; i < _scoreTypes.Length; i++)
			{
				var score = Instantiate(ScorePrefab);
				score.transform.parent = transform;
			
				var x = i < newRowIndex ? i : i - 6;
				var y = i < newRowIndex ? 0 : (uint) 1;
			
				score.transform.localPosition = PositionFromIndex(x, y);
				score.MyType = _scoreTypes[i];
				_scores.Add(score);
			}
		
			foreach (var score in _scores)
			{
				score.OnValueChanged.AddListener(() =>
				{
					uint sumPoints = 0;
					for (var i = 0; i < 6; i++)
					{
						sumPoints += _scores[i].Points;
					}

					sum.Points = sumPoints;
					_bonus.Points = sumPoints >= 63 ? 50u : 0u;

					var total = GetTotal();
					Total.text = "" + total;
				});
			}
		}

		private Vector2 PositionFromIndex(uint x, uint y)
		{
			const float nr = 4f;
			var startPosition = new Vector2(-nr * Separation, 0);

			if (y == 0)
			{
				startPosition.x += Separation * .5f;
			}

			return startPosition + new Vector2(x * Separation, - y * RowSeparation);
		}

		public IEnumerable<Score> GetScores()
		{
			return _scores;
		}

		public uint GetTotal()
		{
			return _scores.Select(a => a.Points).Aggregate(0u, (a, b) => a + b) + _bonus.Points;
		}
	}
}
