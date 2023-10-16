using System.Xml;
using System.Xml.Serialization;
using PAC_Map_Combiner_REST_API.Models;

namespace PAC_Map_Combiner_REST_API.Utilities;

public class MapDeserializer
{
    private readonly string? _path;
    private readonly Stream? _stream;

    /// <summary>
    /// Create map deserializer object with stream as a parameter
    /// </summary>
    /// <param name="stream">A stream object</param>
    public MapDeserializer( Stream? stream )
    {
      _stream = stream;
    }

    /// <summary>
    /// Create map deserializer object with path as a parameter
    /// </summary>
    /// <param name="path">A string of file path</param>
    public MapDeserializer( string path )
    {
        _path = path;
    }

    /// <summary>
    /// Execute map deserializer
    /// </summary>
    /// <returns>Object of deserialized map</returns>
    public CueMap? Execute()
    {
        var serializer = new XmlSerializer( typeof( CueMap ) );

        CueMap? deserializedMap = GetMapDeserializer(serializer);
        
        return deserializedMap;
    }

    /// <summary>
    /// Get a parameter of map deserializer, whether it is a path or a stream
    /// </summary>
    /// <param name="serializer"></param>
    /// <returns>Object of deserialized map</returns>
    private CueMap? GetMapDeserializer(XmlSerializer serializer)
    {
      CueMap? deserializedMap = CheckDeserializerPathParam(serializer) ?? CheckDeserializerStreamParam(serializer);

      return deserializedMap;
    }

    /// <summary>
    /// Check whether the path parameter is null or not 
    /// </summary>
    /// <param name="serializer"></param>
    /// <returns>Object of deserialized map</returns>
    private CueMap? CheckDeserializerPathParam(XmlSerializer serializer)
    {
      CueMap? deserializedMap = null;
      if (_path != null)
      {
        deserializedMap = DeserializeMapPath(serializer);
      }

      return deserializedMap;
    }

    /// <summary>
    /// Deserialize a map with a path as parameter
    /// </summary>
    /// <param name="serializer"></param>
    /// <returns>Object of deserialized map</returns>
    private CueMap? DeserializeMapPath( XmlSerializer serializer )
    {
      CueMap? deserializedMap = null;
      if( _path != null )
      {
        using var reader = XmlDictionaryReader.CreateTextReader( new FileStream( _path, FileMode.OpenOrCreate ), new XmlDictionaryReaderQuotas());
        deserializedMap = ( CueMap? )serializer.Deserialize( reader );
        for( int i = 0; i < deserializedMap?.Items!.Count; i++ )
        {
          Cue coordinate = deserializedMap.Items![i];
          coordinate.CueId = (uint)i;
        }
      }
      return deserializedMap;
    }
    
    /// <summary>
    /// Check whether the stream parameter is null or not 
    /// </summary>
    /// <param name="serializer"></param>
    /// <returns>Object of deserialized map</returns>
    private CueMap? CheckDeserializerStreamParam(XmlSerializer serializer)
    {
      CueMap? deserializedMap = null;
      if (_stream != null)
      {
        deserializedMap = DeserializeMapStream(serializer);
      }

      return deserializedMap;
    }
    
    /// <summary>
    /// Deserialize a map with a stream as parameter
    /// </summary>
    /// <param name="serializer"></param>
    /// <returns>Object of deserialized map</returns>
    private CueMap? DeserializeMapStream(XmlSerializer serializer)
    {
      CueMap? deserializedMap = null;
      if (_stream != null)
      {
        using var xmlReader = new StreamReader( _stream );
        deserializedMap = (CueMap?)serializer.Deserialize( xmlReader );
      }
      return deserializedMap;
    }
}