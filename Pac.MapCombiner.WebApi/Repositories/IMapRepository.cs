using PAC_Map_Combiner_REST_API.Models;

namespace PAC_Map_Combiner_REST_API.Repositories;

public interface IMapRepository
{
    CueMap? GetCombinedMap();
    string CombineMap( IFormFile[] files );
    string UpdateMap( uint cueId, Cue coordinate );
    string UpdateMapInBatch( Cue[] coordinates );
}