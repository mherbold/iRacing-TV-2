
using System;
using System.IO;

public static class Program
{
	public const string AppName = "iRacing-TV 2";

	public static readonly string documentsFolder = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ), AppName );
}
