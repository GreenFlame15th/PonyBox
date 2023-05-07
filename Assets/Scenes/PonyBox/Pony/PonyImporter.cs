using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

#if UNITY_ANDROID
using NativeFilePickerNamespace;
#else
using SimpleFileBrowser;
#endif

public class PonyImporter : MonoBehaviour
{

#if UNITY_WEBGL
	public void fileExlorer()
	{
		PonyBoxManager.instance.alarte.Invoke("Unable to import ponies", "Importing ponies in not supported in web browser of the game, reload page to restore default ponies");
	}
#else

#if UNITY_ANDROID
	private void loadFromFiles()
	{
		NativeFilePicker.PickMultipleFiles(
			(String[] paths) =>
			{
				if (paths != null)
					loadFilesFromString(paths);
			},
			new string[] { "image/png", "image/jpeg", "image/gif" });
	}
#endif
	void Start()
	{

#if UNITY_ANDROID
#else
		FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png", ".gif"));
		FileBrowser.SetDefaultFilter(".jpg");
		FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
		FileBrowser.AddQuickLink("Downloads", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads", null);
		FileBrowser.AddQuickLink("Game Location", "..\\", null);
#endif
	}

	public void fileExlorer()
	{
#if UNITY_ANDROID

		switch (NativeFilePicker.CheckPermission())
		{
			case NativeFilePicker.Permission.Denied:
				PonyBoxManager.instance.alarte.Invoke("Cannot acess fiels", "Open setting and enable file acess permison to upload your ponies");
				break;
			case NativeFilePicker.Permission.Granted:
				loadFromFiles();
				break;
			case NativeFilePicker.Permission.ShouldAsk:
				loadFromFiles();
				break;
			default:
				break;
		}

#else
		StartCoroutine(ShowLoadDialogCoroutine());
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");
		if (FileBrowser.Success)
		{
			loadFilesFromString(FileBrowser.Result);
		}
#endif
	}

	public void loadFilesFromString(String[] files)
	{
		StringBuilder errorLog = new StringBuilder();

		SpriteMaker spriteMaker = PonyBoxManager.instance.spriteMaker;
		// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
		for (int i = 0; i < files.Length; i++)
		{
			try
			{
				if (files[i].EndsWith(".gif"))
				{
					spriteMaker.makePony(File.ReadAllBytes(files[i]), true);
				}
				else
				{
                    spriteMaker.makePony(File.ReadAllBytes(files[i]), false);
                }
			}
			catch (Exception e)
			{
#if DEVELOPMENT_BUILD
					PonyBoxManager.instance.alarte.Invoke("FileBrowser.Result[i", e.ToString());			
#endif
				errorLog.Append(files + ", ");
				Debug.LogError(files + " coused\n" + e.ToString());
			}
		}

		if (errorLog.Length != 0)
		{
#if DEVELOPMENT_BUILD

#elif UNITY_EDITOR
			Debug.LogError(errorLog.ToString());
#else
			PonyBoxManager.instance.alarte.Invoke("Failed to import some files", errorLog.ToString() + "could not be loaded due to sprite making error");
#endif
        }
	}

#endif
}
