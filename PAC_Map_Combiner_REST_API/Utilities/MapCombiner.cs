using PAC_Map_Combiner_REST_API.Models;

namespace PAC_Map_Combiner_REST_API.Utilities;

public class MapCombiner
{
    private readonly CueMap _combinedMap;
    private bool _isAnyDupe;

    public MapCombiner(List<CueMap> maps)
    {
        _combinedMap = new CueMap
        {
            Items = new List<Cue>()
        };
        
        foreach (var map in maps)
        {
            var inMapItem = map.Items;
            if (inMapItem != null)
            {
                _combinedMap.Items?.AddRange(inMapItem);
                if (_combinedMap.Items != null) _combinedMap.Len = _combinedMap.Items.Count;
            }
        }
        
        _isAnyDupe = false;
    }

    /// <summary>
    /// Combine input maps into a map, and sort the coordinates from 0
    /// </summary>
    /// <returns>Combined Map that has coordinates from 0</returns>
    public CueMap Combine()
    {
        _isAnyDupe = IsAnyDupe(_combinedMap);

        if (!_isAnyDupe)
        {
            var mapDetails = _combinedMap.Items;

            if (mapDetails != null)
            {
                int initialMap = 0;
                foreach (var detail in mapDetails)
                {
                    detail.Tx = initialMap;
                    initialMap += 25;
                }
            }
            
            return _combinedMap;
        }

        return null;
    }

    private bool IsAnyDupe(CueMap combinedMap)
    {
        var mapDupe = combinedMap.Items?.GroupBy(m => m.ID)
            .Where(m => m.Count() > 1)
            .ToList();

        if (mapDupe?.Count > 1)
        {
            return true;
        }
        return false;
    }
}