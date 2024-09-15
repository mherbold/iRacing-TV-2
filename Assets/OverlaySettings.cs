
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu]
public class OverlaySettings : ScriptableObject, IDataSourceViewHashProvider, INotifyBindablePropertyChanged
{
	public long HashCode { get; private set; } = 0;
	public bool IsDirty { get; private set; } = false;
	public float DirtyTimer { get; private set; }

	public OverlaySettingsData data;

	public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

	private Settings settings;

	public OverlaySettings()
	{
		data = new OverlaySettingsData( this );

		RefreshFilePathList();
	}

	public void SetSettings(Settings settings)
	{
		this.settings = settings;
	}

	public void RefreshFilePathList()
	{
		Directory.CreateDirectory( Settings.overlaySettingsFolder );

		var filePathList = new List<string>( Directory.GetFiles( Settings.overlaySettingsFolder, "*.xml" ) );

		data.UpdateFilePathList( filePathList );
	}

	public void Load( string filePath )
	{
		Debug.Log( $"OverlaySettings - Load: {filePath}" );

		if ( filePath != settings.settingsFilePaths.overlaySettingsFilePath )
		{
			settings.settingsFilePaths.overlaySettingsFilePath = filePath;

			settings.SaveFilePaths();
		}

		try
		{
			data = (OverlaySettingsData) Serializer.Load( filePath, typeof( OverlaySettingsData ) );

			data.SetOverlaySettings( this );
		}
		catch ( FileNotFoundException )
		{
			var fileStream = File.Create( filePath );

			fileStream.Close();

			RefreshFilePathList();
		}
		catch ( Exception exception )
		{
			Debug.Log( exception.Message );
		}
		finally
		{
			IsDirty = false;

			Touch();

			data.FilePathIndex = data.FilePathList.IndexOf( settings.settingsFilePaths.overlaySettingsFilePath );
		}
	}

	public void Save( string filePath )
	{
		Debug.Log( $"OverlaySettings - Save: {filePath}" );

		try
		{
			Serializer.Save( filePath, data );
		}
		catch ( Exception exception )
		{
			Debug.Log( exception.Message );
		}
		finally
		{
			IsDirty = false;
		}
	}

	public void Update()
	{
		if ( DirtyTimer > 0 )
		{
			DirtyTimer -= Time.deltaTime;
		}
	}

	public void Touch()
	{
		Debug.Log( $"OverlaySettings - Touch" );

		HashCode++;
	}

	public void MarkAsDirty()
	{
		Debug.Log( $"OverlaySettings - MarkAsDirty" );

		IsDirty = true;
		DirtyTimer = 5;
	}

	public void Notify( [CallerMemberName] string property = "" )
	{
		Debug.Log( $"OverlaySettings - Notify: {property}" );

		propertyChanged?.Invoke( this, new BindablePropertyChangedEventArgs( $"data.{property}" ) );
	}

	long IDataSourceViewHashProvider.GetViewHashCode()
	{
		return HashCode;
	}
}
