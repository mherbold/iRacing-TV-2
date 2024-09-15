
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UIElements;

public class Settings : MonoBehaviour
{
	public static string settingsFilePathsFilePath = Path.Combine( Program.documentsFolder, "SelectedSettings.xml" );

	public SettingsFilePaths settingsFilePaths;

	public static string generalSettingsFolder = Path.Combine( Program.documentsFolder, "General Settings" );
	public static string overlaySettingsFolder = Path.Combine( Program.documentsFolder, "Overlay Settings" );
	public static string overlayLayersFolder = Path.Combine( Program.documentsFolder, "Overlay Layers" );

	public GeneralSettings generalSettings;
	public OverlaySettings overlaySettings;
	public OverlayLayers overlayLayers;

	private bool saveTriggered = false;

	private void Awake()
	{
		Debug.Log( "Settings - Awake" );

		settingsFilePaths = new();
	}

	private void OnEnable()
	{
		Debug.Log( "Settings - OnEnable" );

		var uiDocument = GetComponent<UIDocument>();

		overlaySettings = (OverlaySettings) uiDocument.rootVisualElement.Q<VisualElement>( "overlay-settings-panel" ).dataSource;

		overlaySettings.SetSettings( this );
	}

	private void Update()
	{
		overlaySettings.Update();

		if ( !saveTriggered )
		{
		}

		if ( !saveTriggered )
		{
			if ( overlaySettings.IsDirty && ( overlaySettings.DirtyTimer <= 0 ) )
			{
				Debug.Log( "Triggering save..." );

				saveTriggered = true;

				StartCoroutine( SaveOverlaySettingsCoroutine() );
			}
		}

		if ( !saveTriggered )
		{
		}
	}

	private void LoadFilePaths()
	{
		Debug.Log( "Settings - LoadFilePaths" );

		try
		{
			settingsFilePaths = (SettingsFilePaths) Serializer.Load( settingsFilePathsFilePath, typeof( SettingsFilePaths ) );
		}
		catch ( Exception exception )
		{
			Debug.Log( exception.Message );
		}
	}

	public void SaveFilePaths()
	{
		Debug.Log( "Settings - SaveFilePaths" );

		try
		{
			Serializer.Save( settingsFilePathsFilePath, settingsFilePaths );
		}
		catch ( Exception exception )
		{
			Debug.Log( exception.Message );
		}
	}

	public IEnumerator LoadOverlaySettingsCoroutine()
	{
		Debug.Log( "Settings - LoadOverlaySettingsCoroutine" );

		var task = Task.Run( () => overlaySettings.Load( settingsFilePaths.overlaySettingsFilePath ) );

		yield return new WaitUntil( () => task.IsCompleted );
	}

	public IEnumerator SaveOverlaySettingsCoroutine()
	{
		Debug.Log( "Settings - SaveOverlaySettingsCoroutine" );

		var task = Task.Run( () => overlaySettings.Save( settingsFilePaths.overlaySettingsFilePath ) );

		yield return new WaitUntil( () => task.IsCompleted );

		saveTriggered = false;
	}
}
