using PAC_Map_Combiner_REST_API.Models;

namespace PAC_Map_Combiner_REST_API.Services;

public interface IMapService
{
    CueMap GetCombinedMap();
    string CombineMap(string directory, string filter);
}