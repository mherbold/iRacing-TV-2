
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UIElements;

public class Settings : MonoBehaviour
{
	public static string settingsFilePathsFilePath = Path.Combine( Program.documentsFolder, "SelectedSettings.xml" );

	public FilePathsSerializedData filePathsSerializedData;

	public static string generalSettingsFolder = Path.Combine( Program.documentsFolder, "General Settings" );
	public static string overlaySettingsFolder = Path.Combine( Program.documentsFolder, "Overlay Settings" );
	public static string overlayLayersFolder = Path.Combine( Program.documentsFolder, "Overlay Layers" );

	public GeneralSettingsDataSource generalSettingsDataSource;
	public OverlaySettingsDataSource overlaySettingsDataSource;
	public OverlayLayersDataSource overlayLayersDataSource;

	private bool saveTriggered = false;

	private void Awake()
	{
		Debug.Log( "Settings - Awake" );

		filePathsSerializedData = new();

		LoadFilePaths();

		overlaySettingsDataSource = new OverlaySettingsDataSource( this );
	}

	private void OnEnable()
	{
		Debug.Log( "Settings - OnEnable" );

		var uiDocument = GetComponent<UIDocument>();

		uiDocument.rootVisualElement.Q<VisualElement>( "overlay-settings-panel" ).dataSource = overlaySettingsDataSource;
	}

	private void Update()
	{
		overlaySettingsDataSource.Update();

		if ( !saveTriggered )
		{
		}

		if ( !saveTriggered )
		{
			if ( overlaySettingsDataSource.QueuedForSerialization && ( overlaySettingsDataSource.SerializationTimer <= 0 ) )
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
			filePathsSerializedData = (FilePathsSerializedData) Serializer.Load( settingsFilePathsFilePath, typeof( FilePathsSerializedData ) );
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
			Serializer.Save( settingsFilePathsFilePath, filePathsSerializedData );
		}
		catch ( Exception exception )
		{
			Debug.Log( exception.Message );
		}
	}

	public IEnumerator SaveOverlaySettingsCoroutine()
	{
		Debug.Log( "Settings - SaveOverlaySettingsCoroutine" );

		var task = Task.Run( () => overlaySettingsDataSource.SaveSerializedData( filePathsSerializedData.overlaySettingsFilePath ) );

		yield return new WaitUntil( () => task.IsCompleted );

		saveTriggered = false;
	}
}
