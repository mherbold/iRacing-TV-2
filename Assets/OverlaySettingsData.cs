
using System.Collections.Generic;

using Unity.Properties;

using UnityEngine;

public class OverlaySettingsData
{
	private OverlaySettings overlaySettings;

	public OverlaySettingsData()
	{
		overlaySettings = null;
	}

	public OverlaySettingsData( OverlaySettings overlaySettings )
	{
		this.overlaySettings = overlaySettings;
	}

	public void SetOverlaySettings( OverlaySettings overlaySettings )
	{
		this.overlaySettings = overlaySettings;
	}

	private List<string> _filePathList = new();

	[CreateProperty]
	public List<string> FilePathList
	{
		get => _filePathList;
	}

	public void UpdateFilePathList( List<string> filePathList )
	{
		_filePathList = filePathList;

		overlaySettings?.Notify( "FilePathList" );
	}

	private bool _positionAndSizeAutomatic = true;

	[CreateProperty]
	public bool PositionAndSizeAutomatic
	{
		get => _positionAndSizeAutomatic;

		set
		{
			if ( _positionAndSizeAutomatic == value ) return;

			_positionAndSizeAutomatic = value;

			overlaySettings?.MarkAsDirty();
			overlaySettings?.Notify();

			Debug.Log( $"PositionAndSizeAutomatic -> {value}" );
		}
	}

	private RectInt _positionAndSizeRect = new( 0, 0, 1920, 1080 );

	[CreateProperty]
	public RectInt PositionAndSizeRect
	{
		get => _positionAndSizeRect;

		set
		{
			if ( _positionAndSizeRect == value ) return;

			_positionAndSizeRect = value;

			overlaySettings?.MarkAsDirty();
			overlaySettings?.Notify();

			Debug.Log( $"PositionAndSizeRect -> {value}" );
		}
	}
}
