using System.Xml.Serialization;

namespace PAC_Map_Combiner_REST_API.Models;

public class Cue
{
    [XmlIgnore] public uint CueId { get; set; }
    [XmlAttribute] public uint ID { get; set; }
    [XmlAttribute] public int Rx { get; set; }
    [XmlAttribute] public int Ry { get; set; }
    [XmlAttribute] public int Rz { get; set; }
    [XmlAttribute] public int Tx { get; set; }
    [XmlAttribute] public int Ty { get; set; }
    [XmlAttribute] public int Tz { get; set; }
}