
using System;
using System.IO;

[Serializable]
public class SettingsFilePaths
{
	public string generalSettingsFilePath = Path.Combine( Settings.generalSettingsFolder, "Default.xml" );
	public string overlaySettingsFilePath = Path.Combine( Settings.overlaySettingsFolder, "Default.xml" );
	public string overlayLayersFilePath = Path.Combine( Settings.overlayLayersFolder, "Default.xml" );
}
