using System.Xml;
using System.Xml.Serialization;
using PAC_Map_Combiner_REST_API.Models;

namespace PAC_Map_Combiner_REST_API.Utilities;

public class MapSerializer
{
    private readonly CueMap? _cueMap;
    private readonly string _path;

    /// <summary>
    /// Create a map serializer object with a map object and a file path
    /// </summary>
    /// <param name="cueMap">A map object that want to be serialized</param>
    /// <param name="path">A file path where the file will be saved</param>
    public MapSerializer( CueMap? cueMap, string path )
    {
        _cueMap = cueMap;
        _path = path;
    }

    /// <summary>
    /// Save a map object to a xml file in a specified path
    /// </summary>
    public void SaveToXml()
    {
        var serializer = new XmlSerializer( typeof( CueMap ) );
        XmlWriterSettings xmlSettings = AddXmlSettings();
        XmlSerializerNamespaces xmlNs = AddXmlNamespaces();
        
        using var writer = XmlWriter.Create( _path, xmlSettings );
        serializer.Serialize( writer, _cueMap, xmlNs );
        writer.Flush();
    }

    /// <summary>
    /// Add no xml namespaces in xml file
    /// </summary>
    /// <returns>An empty string of namespaces</returns>
    private static XmlSerializerNamespaces AddXmlNamespaces()
    {
      var xmlNs = new XmlSerializerNamespaces();
      xmlNs.Add( "", "" );
      return xmlNs;
    }

    /// <summary>
    /// Add xml settings for the serialization
    /// </summary>
    /// <returns>Xml settings</returns>
    private static XmlWriterSettings AddXmlSettings()
    {
      var xmlSettings = new XmlWriterSettings() { Async = true, Indent = true, OmitXmlDeclaration = true };
      return xmlSettings;
    }
}