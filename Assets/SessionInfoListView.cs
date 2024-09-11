
using UnityEngine;
using UnityEngine.UIElements;

using IRSDKSharper;

public class SessionInfoListView : MonoBehaviour
{
	public VisualTreeAsset multiColumnListCellView;

	private MultiColumnListView multiColumnListView;

	private UIDocument uiDocument;
	private Simulator simulator;

	private void OnEnable()
	{
		Debug.Log( "SessionInfoListView - OnEnable" );

		uiDocument = GetComponent<UIDocument>();
		simulator = GetComponent<Simulator>();

		multiColumnListView = uiDocument.rootVisualElement.Q<MultiColumnListView>( "session-info-list-view" );

		multiColumnListView.itemsSource = simulator.irsdk.Data.sessionInfoAsList;

		multiColumnListView.columns[ "key" ].makeCell = () => multiColumnListCellView.Instantiate();
		multiColumnListView.columns[ "value" ].makeCell = () => multiColumnListCellView.Instantiate();

		multiColumnListView.columns[ "key" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = ( (IRacingSdkSessionInfoAsList.Datum) simulator.irsdk.Data.sessionInfoAsList[ index ] ).key;
		multiColumnListView.columns[ "value" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = ( (IRacingSdkSessionInfoAsList.Datum) simulator.irsdk.Data.sessionInfoAsList[ index ] ).value;
	}

	public void Refresh()
	{
		multiColumnListView.RefreshItems();
	}
}
