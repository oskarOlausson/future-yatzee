using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable RedundantIfElseBlock

public class LoadAndSave : MonoBehaviour
{
	private string _saveFile;
	private const string SaveFileName = "/Savefile.dat";
	public SaveInfo Data = null;

	public ScoreHandler Scores;
	public ResultHandler Results;

	private void Awake()
	{
		_saveFile = Application.persistentDataPath + SaveFileName;
		Debug.Log(_saveFile);
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		Data = Load();
	}

	public bool HasGameSaved()
	{
		return Data != null;
	}

	private SaveInfo Load()
	{
		if (File.Exists(_saveFile))
		{
			var bf = new BinaryFormatter();
			var file = File.Open(_saveFile, FileMode.Open);
			var si =  (SaveInfo) bf.Deserialize(file);
			file.Close();
			return si.TurnInfos.Length > 0 ? si : null;
		}
		else
		{
			return null;
		}
	}

	public void SaveCurrentProgress()
	{
		var data = RetrieveData();
		
		var bf = new BinaryFormatter();
		var file = File.Create(_saveFile);
		bf.Serialize(file, data);
		file.Close();
	}

	public void ClearCurrentProgress()
	{
		if (File.Exists(_saveFile))
		{
			File.Delete(_saveFile);	
		}
	}

	private SaveInfo RetrieveData()
	{
		var turns = Results.GetResults();
		var scores = Scores.GetScores().ToArray();

		var counter = 0;

		var si = new SaveInfo {TurnInfos = new SaveInfo.TurnInfo[turns.Count], SelectedIndex = -1};

		for (var i = 0; i < turns.Count; i++)
		{
			var result = turns[i];
			
			if (result == Results.Selected)
			{
				si.SelectedIndex = i;
			}

			si.TurnInfos[i].FirstRow = result.FirstRow().Select(die => die.Number).ToArray();
			si.TurnInfos[i].NumberOfReRolls = result.FirstRow().Select(die => die.AmountOfReRolls()).ToArray();
			si.TurnInfos[i].Seeds = result.Seeds();

			si.TurnInfos[i].IndexOfConnectedTo = -1;

			if (Results.HasConnection(result))
			{
				var targetScore = Results.GetConnectedTo(result);
				
				for (var scoreIndex = 0; scoreIndex < scores.Length; scoreIndex++)
				{
					if (scores[scoreIndex] == targetScore)
					{
						si.TurnInfos[i].IndexOfConnectedTo = scoreIndex;
						break;
					}
				}

				counter++;
			}
		}
		
		Debug.Log(counter);

		return si;
	}
}
