
using UnityEngine;
using UnityEngine.UIElements;

using System.Collections;

using IRSDKSharper;

public class EventsListView : MonoBehaviour
{
	public VisualTreeAsset multiColumnListCellView;

	private MultiColumnListView multiColumnListView;

	private UIDocument uiDocument;
	private Simulator simulator;

	private void OnEnable()
	{
		Debug.Log( "EventsListView - OnEnable" );

		uiDocument = GetComponent<UIDocument>();
		simulator = GetComponent<Simulator>();

		multiColumnListView = uiDocument.rootVisualElement.Q<MultiColumnListView>( "events-list-view" );

		multiColumnListView.itemsSource = null;

		multiColumnListView.columns[ "sessionnumber" ].makeCell = () => multiColumnListCellView.Instantiate();
		multiColumnListView.columns[ "sessiontime" ].makeCell = () => multiColumnListCellView.Instantiate();
		multiColumnListView.columns[ "value" ].makeCell = () => multiColumnListCellView.Instantiate();

		multiColumnListView.columns[ "sessionnumber" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = ((EventSystem.Event) multiColumnListView.itemsSource[ index ]).SessionNum.ToString();
		multiColumnListView.columns[ "sessiontime" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = ( (EventSystem.Event) multiColumnListView.itemsSource[ index ] ).SessionTimeAsString;
		multiColumnListView.columns[ "value" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = ( (EventSystem.Event) multiColumnListView.itemsSource[ index ] ).ValueAsString;
	}

	public void Refresh()
	{
		multiColumnListView.RefreshItems();
	}

	public void SetItemsSource( IList itemsSource )
	{
		multiColumnListView.itemsSource = itemsSource;
	}
}
