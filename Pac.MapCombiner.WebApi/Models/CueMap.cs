using System.Xml.Serialization;

namespace PAC_Map_Combiner_REST_API.Models;

[XmlType ( "Map" )]
public class CueMap
{
    [XmlAttribute] public int Len { get; set; }
    [XmlElement( "Cue" )] public List<Cue>? Items { get; set; }
    public CueMap() { }

    public CueMap( List<Cue> items )
    {
        Items = items;
        Len = items.Count;
    }
}