using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Scripts
{
    public class Dragger : MonoBehaviour
    {
        public Result Dragging;
        private Camera _camera;
        private ScoreHandler _scores;
        private ResultHandler _results;

        private void Start()
        {
            _camera = Camera.main;
            _scores = FindObjectOfType<ScoreHandler>();
            _results = FindObjectOfType<ResultHandler>();
            Assert.IsNotNull(_camera, "No camera in scene");
        }

        private void Update()
        {
            if (Dragging)
            {
                var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = Dragging.transform.position.z;
                
                Dragging.Follow(mousePos);

                if (Input.GetMouseButtonUp(0))
                {
                    var score = GetDropZoneAtPosition(mousePos);

                    if (score)
                    {
                        _results.Connect(Dragging, score);
                    }
                    else
                    {
                        _results.Disconnect(Dragging);
                    }

                    Dragging = null;
                }
            }
        }

        private Score GetDropZoneAtPosition(Vector3 position)
        {
            foreach (var score in _scores.GetScores())
            {
                var zone = score.DropZoneCollider();

                if (zone.OverlapPoint(position))
                {
                    return score;
                }
            }
            
            foreach (var score in _scores.GetScores())
            {
                var zone = score.OwnCollider();

                if (zone.OverlapPoint(position))
                {
                    return score;
                }
            }

            return null;
        }
    }
}