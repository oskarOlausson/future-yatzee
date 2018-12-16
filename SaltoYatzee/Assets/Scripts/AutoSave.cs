using UnityEngine;

// ReSharper disable RedundantDefaultMemberInitializer

public class AutoSave : MonoBehaviour
{
	public bool GameDone = false;

	public void Save()
	{
		var saver = GetComponent<LoadAndSave>();
		
		if (GameDone)
		{
			saver.ClearCurrentProgress();
		}
		else
		{
			saver.SaveCurrentProgress();
		}
	}
	
	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus) Save();
	}

	private void OnDestroy()
	{
		Save();
	}
}
