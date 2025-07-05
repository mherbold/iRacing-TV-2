
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

using Unity.Properties;

using UnityEngine;
using UnityEngine.UIElements;

public class OverlaySettingsDataSource : IDataSourceViewHashProvider, INotifyBindablePropertyChanged
{
	public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

	private Settings settings;

	public long HashCode { get; private set; } = 0;
	public bool QueuedForSerialization { get; private set; } = false;
	public float SerializationTimer { get; private set; }

	private int _filePathIndex = 0;

	[CreateProperty]
	public int FilePathIndex
	{
		get => _filePathIndex;

		set
		{
			if ( _filePathIndex == value ) return;

			_filePathIndex = value;

			Debug.Log( $"OverlaySettingsSerializedData.FilePathIndex -> {value}" );

			Notify( "FilePathIndex", false );

			LoadSerializedData( FilePathList[ FilePathIndex ] );
		}
	}

	private List<string> _filePathList = new();

	[CreateProperty]
	public List<string> FilePathList
	{
		get => _filePathList;

		private set
		{
			Debug.Log( $"OverlaySettingsSerializedData.FilePathList set" );

			_filePathList = value;

			Notify( "FilePathList", false );
		}
	}

	private OverlaySettingsSerializedData _serializedData;

	[CreateProperty]
	public OverlaySettingsSerializedData SerializedData
	{
		get => _serializedData;

		private set
		{
			Debug.Log( $"OverlaySettingsSerializedData.SerializedData set" );

			_serializedData = value;

			Notify( "SerializedData", false );
		}
	}

	public OverlaySettingsDataSource( Settings settings )
	{
		this.settings = settings;

		var overlaySettingsFilePath = settings.filePathsSerializedData.overlaySettingsFilePath;

		SerializedData = new OverlaySettingsSerializedData();

		SerializedData.SetOverlaySettings( this );

		Directory.CreateDirectory( Settings.overlaySettingsFolder );

		if ( !File.Exists( overlaySettingsFilePath ) )
		{
			SaveSerializedData( overlaySettingsFilePath );
		}

		FilePathList = new List<string>( Directory.GetFiles( Settings.overlaySettingsFolder, "*.xml" ) );

		var overlaySettingsFilePathIndex = FilePathList.IndexOf( overlaySettingsFilePath );

		if ( overlaySettingsFilePathIndex != -1 )
		{
			if ( FilePathIndex != overlaySettingsFilePathIndex )
			{
				FilePathIndex = overlaySettingsFilePathIndex;
			}
			else
			{
				LoadSerializedData( overlaySettingsFilePath );
			}
		}
	}

	public void LoadSerializedData( string filePath )
	{
		Debug.Log( $"OverlaySettingsDataSource - LoadSerializedData: {filePath}" );

		if ( filePath != settings.filePathsSerializedData.overlaySettingsFilePath )
		{
			settings.filePathsSerializedData.overlaySettingsFilePath = filePath;

			settings.SaveFilePaths();
		}

		try
		{
			SerializedData = (OverlaySettingsSerializedData) Serializer.Load( filePath, typeof( OverlaySettingsSerializedData ) );

			SerializedData.SetOverlaySettings( this );
		}
		catch ( Exception exception )
		{
			Debug.Log( exception.Message );
		}
		finally
		{
			QueuedForSerialization = false;

			Touch();
		}
	}

	public void SaveSerializedData( string filePath )
	{
		Debug.Log( $"OverlaySettingsDataSource - SaveSerializedData: {filePath}" );

		try
		{
			Serializer.Save( filePath, SerializedData );
		}
		catch ( Exception exception )
		{
			Debug.Log( exception.Message );
		}
		finally
		{
			QueuedForSerialization = false;
		}
	}

	public void Update()
	{
		if ( SerializationTimer > 0 )
		{
			SerializationTimer -= Time.deltaTime;
		}
	}

	public void Touch()
	{
		Debug.Log( $"OverlaySettingsDataSource - Touch" );

		HashCode++;
	}

	public void QueueForSerialization()
	{
		Debug.Log( $"OverlaySettingsDataSource - QueueForSerialization" );

		QueuedForSerialization = true;
		SerializationTimer = 5;
	}

	public void Notify( [CallerMemberName] string propertyName = "", bool isPropertyOfSerializedData = true )
	{
		Debug.Log( $"OverlaySettingsDataSource - Notify: {propertyName}, {isPropertyOfSerializedData}" );

		if ( isPropertyOfSerializedData )
		{
			propertyName = $"SerializedData.{propertyName}";
		}

		propertyChanged?.Invoke( this, new BindablePropertyChangedEventArgs( propertyName ) );
	}

	long IDataSourceViewHashProvider.GetViewHashCode()
	{
		return HashCode;
	}
}
