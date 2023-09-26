using Microsoft.AspNetCore.Mvc;
using PAC_Map_Combiner_REST_API.Models;
using PAC_Map_Combiner_REST_API.Services;
using PAC_Map_Combiner_REST_API.Utilities;

namespace PAC_Map_Combiner_REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MapCombinerController : ControllerBase
{
    private readonly IMapService _mapService;
    
    public MapCombinerController(IMapService mapService)
    {
        _mapService = mapService;
    }

    [HttpGet(Name = "GetCombinedMap")]
    public ActionResult<CueMap> GetMap()
    {
        return _mapService.GetCombinedMap();
    }
    
    [HttpPost(Name = "CombiningMap")]
    public ActionResult<string> CombineMap(string directory, string filter)
    {
        return _mapService.CombineMap(directory, filter);
    }
}