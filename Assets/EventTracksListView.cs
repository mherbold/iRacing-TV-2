
using UnityEngine;
using UnityEngine.UIElements;

using System.Collections.Generic;

using IRSDKSharper;
using System.Linq;

public class EventTracksListView : MonoBehaviour
{
	public VisualTreeAsset multiColumnListCellView;

	private MultiColumnListView multiColumnListView;

	private UIDocument uiDocument;
	private Simulator simulator;
	private EventsListView eventsListView;

	private EventTracksAsList eventTracksAsList;

	private void OnEnable()
	{
		Debug.Log( "EventTracksListView - OnEnable" );

		uiDocument = GetComponent<UIDocument>();
		simulator = GetComponent<Simulator>();
		eventsListView = GetComponent<EventsListView>();

		eventTracksAsList = new( simulator.irsdk.EventSystem );

		multiColumnListView = uiDocument.rootVisualElement.Q<MultiColumnListView>( "event-tracks-list-view" );

		multiColumnListView.itemsSource = eventTracksAsList;

		multiColumnListView.columns[ "trackname" ].makeCell = () => multiColumnListCellView.Instantiate();

		multiColumnListView.columns[ "trackname" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = (string) eventTracksAsList[ index ];

		multiColumnListView.selectionChanged += OnSelectionChanged;
	}

	public void Refresh()
	{
		multiColumnListView.RefreshItems();
	}

	private void OnSelectionChanged( IEnumerable<object> enumerable )
	{
		Debug.Log( $"EventTracksListView - OnSelectionChanged: {enumerable}" );

		var selectedEventTrackName = enumerable.First();

		eventsListView.SetItemsSource( simulator.irsdk.EventSystem.Tracks[ selectedEventTrackName.ToString() ].Events );
	}
}
