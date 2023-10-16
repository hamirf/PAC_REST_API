using PAC_Map_Combiner_REST_API.Interfaces;
using PAC_Map_Combiner_REST_API.Models;

namespace PAC_Map_Combiner_REST_API.Services;

public interface IMapService
{
    CueMap? GetCombinedMap();
    string CombineMap( IFormFile[] files );
    string UpdateMap( uint id, Cue coordinate );
    string UpdateMapInBatch( Cue[] coordinates );
}