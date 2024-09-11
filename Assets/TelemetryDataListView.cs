
using UnityEngine;
using UnityEngine.UIElements;

using IRSDKSharper;

public class TelemetryDataListView : MonoBehaviour
{
	public VisualTreeAsset multiColumnListCellView;

	private MultiColumnListView multiColumnListView;

	private UIDocument uiDocument;
	private Simulator simulator;

	private void OnEnable()
	{
		Debug.Log( "TelemetryDataListView - OnEnable" );

		uiDocument = GetComponent<UIDocument>();
		simulator = GetComponent<Simulator>();

		multiColumnListView = uiDocument.rootVisualElement.Q<MultiColumnListView>( "telemetry-data-list-view" );

		multiColumnListView.itemsSource = simulator.irsdk.Data.telemetryDataAsList;

		multiColumnListView.columns[ "key" ].makeCell = () => multiColumnListCellView.Instantiate();
		multiColumnListView.columns[ "value" ].makeCell = () => multiColumnListCellView.Instantiate();
		multiColumnListView.columns[ "unit" ].makeCell = () => multiColumnListCellView.Instantiate();
		multiColumnListView.columns[ "description" ].makeCell = () => multiColumnListCellView.Instantiate();

		multiColumnListView.columns[ "key" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = ( (IRacingSdkTelemetryDataAsList.Datum) simulator.irsdk.Data.telemetryDataAsList[ index ] ).key;
		multiColumnListView.columns[ "value" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = ( (IRacingSdkTelemetryDataAsList.Datum) simulator.irsdk.Data.telemetryDataAsList[ index ] ).value;
		multiColumnListView.columns[ "unit" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = ( (IRacingSdkTelemetryDataAsList.Datum) simulator.irsdk.Data.telemetryDataAsList[ index ] ).unit;
		multiColumnListView.columns[ "description" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = ( (IRacingSdkTelemetryDataAsList.Datum) simulator.irsdk.Data.telemetryDataAsList[ index ] ).description;
	}

	public void Refresh()
	{
		multiColumnListView.RefreshItems();
	}
}
