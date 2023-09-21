using PAC_Map_Combiner_REST_API.Models;

namespace PAC_Map_Combiner_REST_API.Repositories;

public interface IMapRepository
{
    CueMap GetCombinedMap();
    string CombineMap(string directory, string filter);
}