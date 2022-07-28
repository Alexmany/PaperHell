using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour 
{
	
	public static LocalizationManager instance;

    private string LocPathFile;
    private string LocSteamLang;

    private Dictionary<string, string> localizedText;
	private bool isReady = false;
	private string missingTextString = "Localized text not found";

	void Awake () 
	{		
		instance = this;
		BetterStreamingAssets.Initialize();
		
		LoadLocalizedText(PlayerPrefs.GetString("Lang"));
	}

	public void LoadLocalizedText(string fileName)
	{
		localizedText = new Dictionary<string, string> ();

		string filePath = fileName;

		if (BetterStreamingAssets.FileExists(fileName)) 
		{
			string dataAsJson = BetterStreamingAssets.ReadAllText (filePath);
			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData> (dataAsJson);

			for (int i = 0; i < loadedData.items.Length; i++) 
			{
				localizedText.Add (loadedData.items [i].key, loadedData.items [i].value);   
			}
		} 
		else 
		{
			Debug.LogError ("Cannot find file!");
		}

		isReady = true;
	}

	public string GetLocalizedValue(string key)
	{
		string result = missingTextString;

		if (localizedText.ContainsKey (key)) 
		{
			result = localizedText [key];
		}

		return result;
	}

	public bool GetIsReady()
	{
		return isReady;
	}
}