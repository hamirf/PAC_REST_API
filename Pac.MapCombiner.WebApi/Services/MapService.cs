using PAC_Map_Combiner_REST_API.Interfaces;
using PAC_Map_Combiner_REST_API.Models;
using PAC_Map_Combiner_REST_API.Repositories;

namespace PAC_Map_Combiner_REST_API.Services;

public class MapService : IMapService
{
    private readonly IMapRepository _mapRepository;
    
    /// <summary>
    /// Create an object of map service with an object of map repository as parameter
    /// </summary>
    /// <param name="mapRepository">An object of map repository</param>
    public MapService( IMapRepository mapRepository )
    {
      _mapRepository = mapRepository;
    }

    /// <summary>
    /// Retrieve data of combined map
    /// </summary>
    /// <returns>A combined map object</returns>
    public CueMap? GetCombinedMap()
    {
      var combinedMap = _mapRepository.GetCombinedMap();
      return combinedMap;
    }

    /// <summary>
    /// Combine multiple uploaded file into a xml file
    /// </summary>
    /// <param name="files">Uploaded files</param>
    /// <returns>Status of combining maps whether being successful or not</returns>
    public string CombineMap( IFormFile[] files )
    {
      var createResult = _mapRepository.CombineMap( files );
      return createResult;
    }

    public string UpdateMap( uint cueId, Cue coordinate )
    {
      var updateResult = _mapRepository.UpdateMap( cueId, coordinate );
      return updateResult;
    }

    public string UpdateMapInBatch( Cue[] coordinates )
    {
      var updateResult = _mapRepository.UpdateMapInBatch( coordinates );
      return updateResult;
    }
}