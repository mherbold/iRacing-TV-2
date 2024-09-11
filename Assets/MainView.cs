
using System;
using System.Collections;
using System.Diagnostics;

using UnityEngine;
using UnityEngine.UIElements;

using Debug = UnityEngine.Debug;

public class MainView : MonoBehaviour
{
	private static IntPtr windowHandle = IntPtr.Zero;

	private UIDocument uiDocument;
	private Settings settings;
	private Simulator simulator;

	private bool editorIsVisible = true;
	private int hotKeyState = 0;

	private float automaticPositionAndSizeTimer = 0;

	private int windowX;
	private int windowY;
	private int windowWidth;
	private int windowHeight;

	private void Start()
	{
		Debug.Log( "MainView - Start" );

		if ( !Application.isEditor )
		{
			windowHandle = Process.GetCurrentProcess().MainWindowHandle;

			StartCoroutine( UpdateWindowPropertiesCoroutine() );
		}

		uiDocument = GetComponent<UIDocument>();
		settings = GetComponent<Settings>();
		simulator = GetComponent<Simulator>();

		HideAllEditorPanels();

		uiDocument.rootVisualElement.Q<DropdownField>( "main-dropdown" ).SetValueWithoutNotify( "None" );
		uiDocument.rootVisualElement.Q<DropdownField>( "main-dropdown" ).RegisterValueChangedCallback( OnMainDropdownValueChanged );
		uiDocument.rootVisualElement.Q<Button>( "close-button" ).clicked += OnCloseButtonClicked;

		StartCoroutine( settings.LoadOverlaySettingsCoroutine() );
	}

	private void Update()
	{
		ProcessToggleEditorAction();
		ProcessAutomaticPositionAndSizeAction();
	}

	private void ProcessToggleEditorAction()
	{
		var leftMenuKeyState = WinApi.GetKeyState( WinApi.VirtualKeyStates.VK_LMENU );
		var rightMenuKeyState = WinApi.GetKeyState( WinApi.VirtualKeyStates.VK_RMENU );
		var f12KeyState = WinApi.GetKeyState( WinApi.VirtualKeyStates.VK_F12 );

		var menuKeyPressed = ( ( leftMenuKeyState | rightMenuKeyState ) & WinApi.KEY_PRESSED ) != 0;
		var f12KeyPressed = ( f12KeyState & WinApi.KEY_PRESSED ) != 0;

		if ( hotKeyState == 0 )
		{
			if ( menuKeyPressed && !f12KeyPressed )
			{
				hotKeyState = 1;
			}
		}
		else if ( hotKeyState == 1 )
		{
			if ( !menuKeyPressed )
			{
				hotKeyState = 0;
			}
			else if ( f12KeyPressed )
			{
				hotKeyState = 2;
			}
		}
		else if ( hotKeyState >= 2 )
		{
			if ( !menuKeyPressed || !f12KeyPressed )
			{
				hotKeyState = 0;
			}
		}

		if ( hotKeyState == 2 )
		{
			hotKeyState = 3;

			ToggleEditor();
		}
	}

	private void ProcessAutomaticPositionAndSizeAction()
	{
		if ( windowHandle != IntPtr.Zero )
		{
			if ( settings.overlaySettings.data.PositionAndSizeAutomatic )
			{
				if ( simulator.windowHandle != IntPtr.Zero )
				{
					automaticPositionAndSizeTimer -= Time.deltaTime;

					if ( automaticPositionAndSizeTimer < 0 )
					{
						automaticPositionAndSizeTimer = 1;

						WinApi.GetClientRect( simulator.windowHandle, out WinApi.RECT rect );

						WinApi.POINT topLeft = new( rect.Left, rect.Top );
						WinApi.POINT bottomRight = new( rect.Right, rect.Bottom );

						WinApi.ClientToScreen( simulator.windowHandle, ref topLeft );
						WinApi.ClientToScreen( simulator.windowHandle, ref bottomRight );

						var rectX = topLeft.X;
						var rectY = topLeft.Y;
						var rectWidth = bottomRight.X - topLeft.X;
						var rectHeight = bottomRight.Y - topLeft.Y;

						if ( rectX != windowX || rectY != windowY || rectWidth != windowWidth || rectHeight != windowHeight )
						{
							windowX = rectX;
							windowY = rectY;
							windowWidth = rectWidth;
							windowHeight = rectHeight;

							UpdateWindowPositionAndSize();

							settings.overlaySettings.data.PositionAndSizeRect = new( windowX, windowY, windowWidth, windowHeight );

							settings.overlaySettings.Touch();
						}
					}
				}
			}
			else
			{
				var rect = settings.overlaySettings.data.PositionAndSizeRect;

				if ( rect.x != windowX || rect.y != windowY || rect.width != windowWidth || rect.height != windowHeight )
				{
					windowX = rect.x;
					windowY = rect.y;
					windowWidth = rect.width;
					windowHeight = rect.height;

					UpdateWindowPositionAndSize();
				}
			}
		}
	}

	public IEnumerator UpdateWindowPropertiesCoroutine()
	{
		Debug.Log( "MainView - UpdateWindowPropertiesCoroutine" );

		yield return new WaitForSplashScreenToFinish();

		Screen.SetResolution( settings.overlaySettings.data.PositionAndSizeRect.x, settings.overlaySettings.data.PositionAndSizeRect.y, false );

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		WinApi.MARGINS margins = new() { Left = -1, Right = -1, Top = -1, Bottom = -1 };

		WinApi.DwmExtendFrameIntoClientArea( windowHandle, ref margins );

		WinApi.SetWindowLong( windowHandle, WinApi.GWL_STYLE, WinApi.WS_POPUP | WinApi.WS_VISIBLE );
		WinApi.SetWindowLong( windowHandle, WinApi.GWL_EXSTYLE, WinApi.WS_EX_LAYERED | WinApi.WS_EX_TOPMOST );

		windowX = settings.overlaySettings.data.PositionAndSizeRect.x;
		windowY = settings.overlaySettings.data.PositionAndSizeRect.y;
		windowWidth = settings.overlaySettings.data.PositionAndSizeRect.width;
		windowHeight = settings.overlaySettings.data.PositionAndSizeRect.height;

		WinApi.SetWindowPos( windowHandle, WinApi.HWND_TOPMOST, windowX, windowY, windowWidth, windowHeight, 0 );
	}

	private void UpdateWindowPositionAndSize()
	{
		Debug.Log( "MainView - UpdateWindowPositionAndSize" );

		if ( windowWidth < 1280 )
		{
			windowWidth = 1280;
		}

		if ( windowHeight < 720 )
		{
			windowHeight = 720;
		}

		WinApi.SetWindowPos( windowHandle, WinApi.HWND_TOPMOST, windowX, windowY, windowWidth, windowHeight, 0 );
	}

	private void ToggleEditor()
	{
		Debug.Log( "MainView - ToggleEditor" );

		if ( editorIsVisible )
		{
			uiDocument.rootVisualElement.Q<VisualElement>( "main-view" ).style.display = DisplayStyle.None;

			if ( !Application.isEditor )
			{
				WinApi.SetWindowLong( windowHandle, WinApi.GWL_EXSTYLE, WinApi.WS_EX_LAYERED | WinApi.WS_EX_TRANSPARENT | WinApi.WS_EX_TOPMOST );
			}

			editorIsVisible = false;
		}
		else
		{
			uiDocument.rootVisualElement.Q<VisualElement>( "main-view" ).style.display = DisplayStyle.Flex;

			if ( !Application.isEditor )
			{
				WinApi.SetWindowLong( windowHandle, WinApi.GWL_EXSTYLE, WinApi.WS_EX_LAYERED | WinApi.WS_EX_TOPMOST );
			}

			editorIsVisible = true;
		}
	}

	private void HideAllEditorPanels()
	{
		Debug.Log( "MainView - HideAllEditorPanels" );

		uiDocument.rootVisualElement.Q<VisualElement>( "overlay-settings-panel" ).style.display = DisplayStyle.None;
		uiDocument.rootVisualElement.Q<VisualElement>( "header-data-panel" ).style.display = DisplayStyle.None;
		uiDocument.rootVisualElement.Q<VisualElement>( "session-info-panel" ).style.display = DisplayStyle.None;
		uiDocument.rootVisualElement.Q<VisualElement>( "telemetry-data-panel" ).style.display = DisplayStyle.None;
		uiDocument.rootVisualElement.Q<VisualElement>( "event-tracks-panel" ).style.display = DisplayStyle.None;
	}

	private void OnMainDropdownValueChanged( ChangeEvent<string> changeEvent )
	{
		Debug.Log( $"MainView - OnMainDropdownValueChanged: {changeEvent.newValue}" );

		HideAllEditorPanels();

		switch ( changeEvent.newValue )
		{
			case "Overlay Settings":
				uiDocument.rootVisualElement.Q<VisualElement>( "overlay-settings-panel" ).style.display = DisplayStyle.Flex;
				break;

			case "Header Data":
				uiDocument.rootVisualElement.Q<VisualElement>( "header-data-panel" ).style.display = DisplayStyle.Flex;
				break;

			case "Session Info":
				uiDocument.rootVisualElement.Q<VisualElement>( "session-info-panel" ).style.display = DisplayStyle.Flex;
				break;

			case "Telemetry Data":
				uiDocument.rootVisualElement.Q<VisualElement>( "telemetry-data-panel" ).style.display = DisplayStyle.Flex;
				break;

			case "Event Tracks":
				uiDocument.rootVisualElement.Q<VisualElement>( "event-tracks-panel" ).style.display = DisplayStyle.Flex;
				break;
		}
	}

	private void OnCloseButtonClicked()
	{
		Debug.Log( "MainView - OnCloseButtonClicked" );

		StartCoroutine( QuitCoroutine() );
	}

	public IEnumerator QuitCoroutine()
	{
		Debug.Log( "MainView - QuitCoroutine" );

		simulator.irsdk.Stop();

		yield return new WaitForIRSDKToStop( simulator.irsdk );

		Application.Quit();
	}
}
