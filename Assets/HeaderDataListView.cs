
using UnityEngine;
using UnityEngine.UIElements;

using IRSDKSharper;

public class HeaderDataListView : MonoBehaviour
{
	public VisualTreeAsset multiColumnListCellView;

	MultiColumnListView multiColumnListView;

	UIDocument uiDocument;
	Simulator simulator;

	void OnEnable()
	{
		Debug.Log( "HeaderDataListView - OnEnable" );

		uiDocument = GetComponent<UIDocument>();
		simulator = GetComponent<Simulator>();

		multiColumnListView = uiDocument.rootVisualElement.Q<MultiColumnListView>( "header-data-list-view" );

		multiColumnListView.itemsSource = simulator.irsdk.Data.headerDataAsList;

		multiColumnListView.columns[ "key" ].makeCell = () => multiColumnListCellView.Instantiate();
		multiColumnListView.columns[ "value" ].makeCell = () => multiColumnListCellView.Instantiate();

		multiColumnListView.columns[ "key" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = ( (IRacingSdkHeaderDataAsList.Datum) simulator.irsdk.Data.headerDataAsList[ index ] ).key;
		multiColumnListView.columns[ "value" ].bindCell = ( VisualElement element, int index ) => element.Q<Label>().text = ( (IRacingSdkHeaderDataAsList.Datum) simulator.irsdk.Data.headerDataAsList[ index ] ).value;
	}

	public void Refresh()
	{
		multiColumnListView.RefreshItems();
	}
}
