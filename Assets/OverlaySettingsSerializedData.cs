
using Unity.Properties;

using UnityEngine;

public class OverlaySettingsSerializedData
{
	private OverlaySettingsDataSource overlaySettingsDataSource;

	public OverlaySettingsSerializedData()
	{
		overlaySettingsDataSource = null;
	}

	public void SetOverlaySettings( OverlaySettingsDataSource overlaySettingsDataSource )
	{
		this.overlaySettingsDataSource = overlaySettingsDataSource;
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

			Debug.Log( $"OverlaySettingsSerializedData.PositionAndSizeAutomatic -> {value}" );

			overlaySettingsDataSource?.QueueForSerialization();
			overlaySettingsDataSource?.Notify();
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

			Debug.Log( $"OverlaySettingsSerializedData.PositionAndSizeRect -> {value}" );

			overlaySettingsDataSource?.QueueForSerialization();
			overlaySettingsDataSource?.Notify();
		}
	}
}
