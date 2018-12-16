using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ResultHandler : MonoBehaviour
{
    public Result ResultPrefab;
    public float Separation = 1f;
    public float RowSeparation = 1f;
    public ParticleSystem Particles;
    private Result _selected;
    private readonly Connections<Result, Score> _connections = new Connections<Result, Score>();
    private ScoreHandler _scoreHandler;
    private readonly List<Result> _results = new List<Result>();

    public Result Selected
    {
        set { Select(value); }
        get { return _selected; }
    }

    private void Start()
    {
        _scoreHandler = FindObjectOfType<ScoreHandler>();
        var startPosition = new Vector3(-Separation * 3.5f, 0, 0);

        var saveFile = FindObjectOfType<LoadAndSave>();

        var data = saveFile.Data;
        
        for (var i = 0; i < 15; i++)
        {
            var result = Instantiate(ResultPrefab);
            _results.Add(result);

            if (data != null)
            {
                result.RestoreFromSavedState(data.TurnInfos[i]);
                var scoreIndex = data.TurnInfos[i].IndexOfConnectedTo;
                if (scoreIndex >= 0)
                {
                    StartCoroutine(ConnectLater(result, scoreIndex));
                }
            }
            
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
            
            result.OnChanged.Invoke();
            
            if (data != null)
            {
                if (data.SelectedIndex == i)
                {
                    Select(result);
                }
            }
        }
    }

    private IEnumerator ConnectLater(Result result, int scoreIndex)
    {
        yield return new WaitForFixedUpdate();
        var scores = _scoreHandler.GetScores().ToArray();
        Connect(result, scores[scoreIndex]);
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
        result.MoveTo(score.DropZonePosition());
        _connections.Connect(result, score);
    }

    public Score GetConnectedTo(Result result)
    {
        if (!_connections.Has(result)) return null;
        return _connections.Get(result);
    }

    public List<Result> GetResults()
    {
        return _results;
    }

    public bool HasConnection(Result result)
    {
        return _connections.Has(result);
    }
}