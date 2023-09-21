using System.Reflection;
using PAC_Map_Combiner_REST_API.Models;
using PAC_Map_Combiner_REST_API.Repositories;
using PAC_Map_Combiner_REST_API.Utilities;

namespace PAC_Map_Combiner_REST_API.Services;

public class MapService : IMapService
{
    private IMapRepository _mapRepository;
    
    public MapService(IMapRepository mapRepository)
    {
        _mapRepository = mapRepository;
    }

    public CueMap GetCombinedMap()
    {
        return _mapRepository.GetCombinedMap();
    }

    public string CombineMap(string directory, string filter)
    {
        return _mapRepository.CombineMap(directory, filter);
    }
}