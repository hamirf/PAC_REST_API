using System.Reflection;
using PAC_Map_Combiner_REST_API.Models;
using PAC_Map_Combiner_REST_API.Utilities;

namespace PAC_Map_Combiner_REST_API.Repositories;

public class MapRepository : IMapRepository
{
    private readonly string _combinedMapDir;

    public MapRepository()
    {
        var root = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        
        _combinedMapDir = root + @"\config\map\pac_full_map.xml";
    }

    public CueMap GetCombinedMap()
    {
        var combinedMap = new MapDeserializer(_combinedMapDir);
        return combinedMap.Execute();
    }

    public string CombineMap(string directory, string filter)
    {
        var mapGatherer = new MapsGatherer(directory);
        var maps = mapGatherer.Gather(Directory.GetFiles(directory, filter));

        var mapCombiner = new MapCombiner(maps);
        var combinedMap = mapCombiner.Combine();
        
        var fullMapSerializer = new MapSerializer(combinedMap, _combinedMapDir);
        fullMapSerializer.SaveToXml();
        
        return $"Map successfully saved to {_combinedMapDir}";
    }
}