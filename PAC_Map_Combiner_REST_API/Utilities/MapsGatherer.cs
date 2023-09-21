using System.Reflection;
using PAC_Map_Combiner_REST_API.Models;

namespace PAC_Map_Combiner_REST_API.Utilities;

public class MapsGatherer
{
    private string _dirPath;
    public MapsGatherer(string dirPath)
    {
        _dirPath = dirPath;
    }

    public List<CueMap> Gather(string[] xmlList)
    {
        var maps = new List<CueMap>();
        string originPath = _dirPath;
        
        foreach (var xml in xmlList)
        {
            string fileName = xml;
            string fullPath = Path.Combine(originPath, fileName);
            
            var inputDeserializer = new MapDeserializer(fullPath);
            var inputMap = inputDeserializer.Execute();

            maps.Add(inputMap);
        }

        return maps;
    }
}