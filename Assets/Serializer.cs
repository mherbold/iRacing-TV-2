
using System;
using System.IO;
using System.Xml.Serialization;

using UnityEngine;

public static class Serializer
{
	public static object Load( string filePath, Type type )
	{
		Debug.Log( $"Serializer - Load: {filePath}" );

		var xmlSerializer = new XmlSerializer( type );

		var fileStream = new FileStream( filePath, FileMode.Open );

		var data = xmlSerializer.Deserialize( fileStream ) ?? throw new Exception();

		fileStream.Close();

		return data;
	}

	public static void Save( string filePath, object data )
	{
		Debug.Log( $"Serializer - Save: {filePath}" );

		Directory.CreateDirectory( Path.GetDirectoryName( filePath ) );

		var xmlSerializer = new XmlSerializer( data.GetType() );

		var streamWriter = new StreamWriter( filePath );

		xmlSerializer.Serialize( streamWriter, data );

		streamWriter.Close();
	}
}
