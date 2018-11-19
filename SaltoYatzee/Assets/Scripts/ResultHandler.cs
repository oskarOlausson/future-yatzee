using UnityEngine;

namespace Scripts
{
    public class ResultHandler : MonoBehaviour
    {
        public Result ResultPrefab;
        public float Separation = 1f;
        public float RowSeparation = 1f;
        public ParticleSystem Particles;
        private Result _selected;
        private readonly Connections<Result, Score> _connections = new Connections<Result, Score>();

        public Result Selected
        {
            set { Select(value); }
        }

        private void Start()
        {
            var startPosition = new Vector3(-Separation * 3.5f, 0, 0);
        
            for (var i = 0; i < 15; i++)
            {
                var result = Instantiate(ResultPrefab);
                result.transform.parent = transform;

                var x = i < 7 ? i : i - 7;
                var y = i < 7 ? 0 : 1;

                var diff = new Vector3(x * Separation, y * RowSeparation, result.transform.position.z);
                if (y == 0)
                {
                    diff.x += Separation * 0.5f;
                }
            
                result.transform.localPosition = startPosition + diff;

                if (i == 0)
                {
                    Selected = result;
                }
            
                var fixResult = result;
                result.OnSelected.AddListener(() => { Selected = fixResult; });
                result.OnChanged.AddListener(() =>
                {
                    if (_connections.Has(fixResult))
                    {
                        var score = _connections.Get(fixResult);
                        score.UpdateValues(fixResult.GetValues());
                    }
                });
            }
        }

        public void Select(Result result)
        {
            _selected = result;
            var futureDice = FindObjectOfType<FutureDice>();
            futureDice.NewTurn(result.GetTurn());
        }

        public void Update()
        {
            Particles.transform.position =
                new Vector3(_selected.transform.position.x, _selected.transform.position.y, Particles.transform.position.z);
        }

        public void Disconnect(Result result)
        {
            if (_connections.Has(result))
            {
                var score = _connections.Get(result);
                score.UnSelect();
                _connections.Remove(result);
            }
            
            result.ReturnToStartPosition();
        }

        public void Connect(Result result, Score score)
        {
            
            if (_connections.Has(score))
            {
                var previousResult = _connections.Get(score);
                previousResult.ReturnToStartPosition();
                Disconnect(previousResult);
            }
        
            if (_connections.Has(result))
            {
                var previousScore = _connections.Get(result);
                previousScore.UnSelect();
                Disconnect(result);
            }
        
            score.UpdateValues(result.GetValues());
            _selected.Follow(score.DropZonePosition());
            _connections.Connect(result, score);
        }
    }
}