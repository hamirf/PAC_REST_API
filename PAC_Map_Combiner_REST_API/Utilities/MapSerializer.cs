using System.Xml;
using System.Xml.Serialization;
using PAC_Map_Combiner_REST_API.Models;

namespace PAC_Map_Combiner_REST_API.Utilities;

public class MapSerializer
{
    private readonly CueMap _cueMap;
    private readonly string _path;

    public MapSerializer(CueMap cueMap, string path)
    {
        _cueMap = cueMap;
        _path = path;
    }

    public void SaveToXml()
    {
        var serializer = new XmlSerializer(typeof(CueMap));
        using var xml = XmlWriter.Create( _path, new XmlWriterSettings()
        {
            Async = true,
            Indent = true, 
            OmitXmlDeclaration = true
        });
        serializer.Serialize(xml, _cueMap);
    }
}