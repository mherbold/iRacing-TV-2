
using System;
using System.Collections;

using System.IO;

using UnityEngine;
using UnityEngine.UIElements;

using IRSDKSharper;

public class Simulator : MonoBehaviour
{
	public static string eventTracksFolder = Path.Combine( Program.documentsFolder, "Event Tracks" );

	public IRacingSdk irsdk;

	public IntPtr windowHandle = IntPtr.Zero;

	private UIDocument uiDocument;

	private HeaderDataListView headerDataListView;
	private SessionInfoListView sessionInfoListView;
	private TelemetryDataListView telemetryDataListView;
	private EventTracksListView eventTracksListView;

	private void Awake()
	{
		Debug.Log( "Simulator - Awake" );

		irsdk = new();
	}

	private void OnEnable()
	{
		Debug.Log( "Simulator - OnEnable" );

		uiDocument = GetComponent<UIDocument>();

		StartCoroutine( OnDisconnectedMainThreadCoroutine() );

		headerDataListView = GetComponent<HeaderDataListView>();
		sessionInfoListView = GetComponent<SessionInfoListView>();
		telemetryDataListView = GetComponent<TelemetryDataListView>();
		eventTracksListView = GetComponent<EventTracksListView>();

		if ( irsdk != null )
		{
			irsdk.OnException += OnException;
			irsdk.OnConnected += OnConnected;
			irsdk.OnDisconnected += OnDisconnected;
			irsdk.OnSessionInfo += OnSessionInfo;
			irsdk.OnTelemetryData += OnTelemetryData;
			irsdk.OnEventSystemDataReset += OnEventSystemDataReset;
			irsdk.OnEventSystemDataLoaded += OnEventSystemDataLoaded;
			irsdk.OnStopped += OnStopped;

			irsdk.Start();

			irsdk.EnableEventSystem( eventTracksFolder );
		}
	}

	private void OnException( Exception exception )
	{
		Debug.Log( $"Simulator - OnException: {exception.Message}" );

		irsdk.Stop();
	}

	private void OnConnected()
	{
		Debug.Log( "Simulator - OnConnected" );

		MainThreadDispatcher.Instance()?.BeginInvoke( OnConnectedMainThread() );
	}

	private IEnumerator OnConnectedMainThread()
	{
		Debug.Log( "Simulator - OnConnectedMainThread" );

		uiDocument.rootVisualElement.Q<VisualElement>( "simulator-not-running-1" ).style.display = DisplayStyle.None;
		uiDocument.rootVisualElement.Q<VisualElement>( "simulator-not-running-2" ).style.display = DisplayStyle.None;
		uiDocument.rootVisualElement.Q<VisualElement>( "simulator-not-running-3" ).style.display = DisplayStyle.None;
		uiDocument.rootVisualElement.Q<VisualElement>( "simulator-not-running-4" ).style.display = DisplayStyle.None;

		uiDocument.rootVisualElement.Q<MultiColumnListView>( "header-data-list-view" ).style.display = DisplayStyle.Flex;
		uiDocument.rootVisualElement.Q<MultiColumnListView>( "session-info-list-view" ).style.display = DisplayStyle.Flex;
		uiDocument.rootVisualElement.Q<MultiColumnListView>( "telemetry-data-list-view" ).style.display = DisplayStyle.Flex;
		uiDocument.rootVisualElement.Q<MultiColumnListView>( "event-tracks-list-view" ).style.display = DisplayStyle.Flex;

		windowHandle = WinApi.FindWindow( null, "iRacing.com Simulator" );

		yield return null;
	}

	private void OnDisconnected()
	{
		Debug.Log( "Simulator - OnDisconnected" );

		MainThreadDispatcher.Instance()?.BeginInvoke( OnDisconnectedMainThreadCoroutine() );
	}

	private IEnumerator OnDisconnectedMainThreadCoroutine()
	{
		Debug.Log( "Simulator - OnDisconnectedMainThread" );

		uiDocument.rootVisualElement.Q<VisualElement>( "simulator-not-running-1" ).style.display = DisplayStyle.Flex;
		uiDocument.rootVisualElement.Q<VisualElement>( "simulator-not-running-2" ).style.display = DisplayStyle.Flex;
		uiDocument.rootVisualElement.Q<VisualElement>( "simulator-not-running-3" ).style.display = DisplayStyle.Flex;
		uiDocument.rootVisualElement.Q<VisualElement>( "simulator-not-running-4" ).style.display = DisplayStyle.Flex;

		uiDocument.rootVisualElement.Q<MultiColumnListView>( "header-data-list-view" ).style.display = DisplayStyle.None;
		uiDocument.rootVisualElement.Q<MultiColumnListView>( "session-info-list-view" ).style.display = DisplayStyle.None;
		uiDocument.rootVisualElement.Q<MultiColumnListView>( "telemetry-data-list-view" ).style.display = DisplayStyle.None;
		uiDocument.rootVisualElement.Q<MultiColumnListView>( "event-tracks-list-view" ).style.display = DisplayStyle.None;

		windowHandle = IntPtr.Zero;

		yield return null;
	}

	private void OnSessionInfo()
	{
		Debug.Log( "Simulator - OnSessionInfo" );

		MainThreadDispatcher.Instance()?.BeginInvoke( OnSessionInfoMainThread() );
	}

	private IEnumerator OnSessionInfoMainThread()
	{
		Debug.Log( "Simulator - OnSessionInfoMainThread" );

		sessionInfoListView.Refresh();

		yield return null;
	}

	private void OnEventSystemDataReset()
	{
		Debug.Log( "Simulator - OnEventSystemDataReset" );
	}

	private void OnEventSystemDataLoaded()
	{
		Debug.Log( "Simulator - OnEventSystemDataLoaded" );

		MainThreadDispatcher.Instance()?.BeginInvoke( OnEventSystemDataLoadedMainThread() );
	}

	private IEnumerator OnEventSystemDataLoadedMainThread()
	{
		Debug.Log( "Simulator - OnEventSystemDataLoadedMainThread" );

		eventTracksListView.Refresh();

		yield return null;
	}

	private void OnStopped()
	{
		Debug.Log( "Simulator - OnStopped" );
	}

	private void OnTelemetryData()
	{
		MainThreadDispatcher.Instance()?.BeginInvoke( OnTelemetryDataMainThread() );
	}

	private IEnumerator OnTelemetryDataMainThread()
	{
		headerDataListView.Refresh();
		telemetryDataListView.Refresh();

		yield return null;
	}
}
