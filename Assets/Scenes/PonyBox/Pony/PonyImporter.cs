using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class PonyImporter : MonoBehaviour
{
	void Start()
	{
		FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png", ".gif"));
		FileBrowser.SetDefaultFilter(".jpg");
		FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
		FileBrowser.AddQuickLink("Downloads", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads", null);
		FileBrowser.AddQuickLink("Game Location", "..\\", null);
	}

	public void fileExlorer()
	{
		StartCoroutine(ShowLoadDialogCoroutine());
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

		if (FileBrowser.Success)
		{
			StringBuilder errorLog = new StringBuilder();
			SpriteMaker spriteMaker = PonyBoxManager.instance.spriteMaker;
			// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
			for (int i = 0; i < FileBrowser.Result.Length; i++)
			{
				try
				{
					if (FileBrowser.Result[i].EndsWith(".png") || FileBrowser.Result[i].EndsWith(".jpg"))
					{
						spriteMaker.MakePonyFromPng(File.ReadAllBytes(FileBrowser.Result[i]));

					}
					else if (FileBrowser.Result[i].EndsWith(".gif"))
					{
						spriteMaker.MakePonyFromGif(FileBrowser.Result[i]);
					}
				}
				catch(Exception e)
				{
					errorLog.Append(FileBrowser.Result[i]+", ");
					Debug.LogError(FileBrowser.Result[i] + " coused\n" + e.ToString());
				}
			}

			if (errorLog.Length != 0)
            {
				PonyBoxManager.instance.alarte.Invoke("Failed to import some files", errorLog.ToString()+"could not be loaded due to sprite making error");
            }

		}
		else
        {
			PonyBoxManager.instance.alarte.Invoke("Failed to import files", "Faile loader encounter an error while importing your files");
		}

	}
}

