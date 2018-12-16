using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighScore : MonoBehaviour
{
	private const string HighScoreName = "highScores";
	public uint MaxNumberOfEntries = 10;
	public GuiEntry EntryPrefab;

	public float Offset = -2f;
	public float Separation = 0.8f;

	private struct Entry
	{
		public readonly string Name;
		public readonly uint Points;

		public Entry(string nm, uint val)
		{
			Name = nm;
			Points = val;
		}
	}

	private List<Entry> _entries;
	private readonly List<GuiEntry> _guiEntries = new List<GuiEntry>();

	public void Start()
	{
		var highScores = PlayerPrefs.GetString(HighScoreName, "").Split('\n');

		_entries = highScores
			.Select(row => row.Trim())
			.Where(row => row.Length > 0)
			.Select(row =>
			{
				var words = row.Split(',');
				var nm = words[0];
				var score = uint.Parse(words[1]);
	
				return new Entry(nm, score);
			}).ToList();

		if (Carry.Points <= _entries[_entries.Count - 1].Points)
		{
			Destroy(FindObjectOfType<AddToHighScore>().gameObject);
		}
		
		var start = new Vector3(transform.position.x, transform.position.y + Offset, transform.position.z);
		for (var i = 0; i < MaxNumberOfEntries; i++)
		{
			var entry = Instantiate(EntryPrefab);
			entry.transform.parent = transform;
			entry.transform.localPosition = start + new Vector3(0, -i * Separation, EntryPrefab.transform.position.z);
				
			_guiEntries.Add(entry);
			if (i < _entries.Count)
			{
				var e = _entries[i];
				entry.UpdateEntry(e.Name, e.Points);
			}
			else
			{
				entry.Clear();
			}
		}
	}
	
	public void SaveHighScore(string hName)
	{
		var points = Carry.Points;
			
		var max = Math.Min((uint) _entries.Count, MaxNumberOfEntries);

		var added = false;
		for (var i = 0; i < max; i++)
		{
			if (points > _entries[i].Points)
			{
				_entries.Insert(i, new Entry(hName, points));
				added = true;
				break;
			}
		}

		if (max < MaxNumberOfEntries && !added)
		{
			_entries.Add(new Entry(hName, points));
		}
			
		max = Math.Min((uint) _entries.Count, MaxNumberOfEntries);

		var asString = "";
		for (var i = 0; i < max; i++)
		{
			var entry = _entries[i];
			_guiEntries[i].UpdateEntry(entry.Name, entry.Points);
			asString += FormatHighScoreRow(entry.Name, entry.Points) + "\n";
		}
			
		PlayerPrefs.SetString(HighScoreName, asString);
	}

	private static string FormatHighScoreRow(string hName, uint points)
	{
		hName = hName.Replace(',', ' ');
		return hName + "," + points;
	}
}