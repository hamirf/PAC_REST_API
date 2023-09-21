using System.Xml;
using System.Xml.Serialization;
using PAC_Map_Combiner_REST_API.Models;

namespace PAC_Map_Combiner_REST_API.Utilities;

public class MapDeserializer
{
    private readonly string _path;
    public MapDeserializer(string path)
    {
        _path = path;
    }

    public CueMap Execute()
    {
        CueMap deserializedMap;
        var serializer = new XmlSerializer(typeof(CueMap));

        var fs = new FileStream(_path, FileMode.Open);
        using (var reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas()))
        {
            deserializedMap = (CueMap)serializer.Deserialize(reader);
        }

        return deserializedMap ?? null;
    }
}