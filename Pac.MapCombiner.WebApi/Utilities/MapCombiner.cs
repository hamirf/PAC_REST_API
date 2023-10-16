using PAC_Map_Combiner_REST_API.Models;

namespace PAC_Map_Combiner_REST_API.Utilities;

public class MapCombiner
{
  private readonly CueMap? _combinedMap;
  private int _index;

  public MapCombiner( IEnumerable<CueMap> maps )
  {
      _combinedMap = new CueMap() { Items = new List<Cue>() };
      foreach(var map in maps)
      {
        if( map.Items != null ) _combinedMap.Items?.AddRange( map.Items );
        if( _combinedMap.Items != null ) _combinedMap.Len = _combinedMap.Items.Count;
      }
      _index = 0;
  }

  /// <summary>
  /// Combine input maps into a map, and sort the coordinates from 0
  /// </summary>
  /// <returns>Combined Map, start new coordinates from 0 with 25 diff</returns>
  public CueMap? Combine()
  {
    if( IsAnyDupe( _combinedMap ) )
    {
      return null;
    }

    GenerateCoordinates();
    return _combinedMap;
  }

  private void GenerateCoordinates()
  {
    for ( int i = 0; i < _combinedMap?.Items?.Count; i++ )
    {
      MakeNewCoordinate( i );
      _index++;
    }
  }

  private void MakeNewCoordinate( int i )
  {
    GenerateXCoordinates(i);
    GenerateYCoordinates(i);
    GenerateZCoordinates(i);
  }

  private void GenerateXCoordinates( int i )
  {
    if( _combinedMap?.Items != null )
    {
      _combinedMap.Items[i].Rx = 0;
      _combinedMap.Items[i].Tx = _index * 25;
    }
  }

  private void GenerateYCoordinates( int i )
  {
    if( _combinedMap?.Items != null )
    {
      _combinedMap.Items[i].Ry = 0;
      _combinedMap.Items[i].Ty = 0;
    }
  }

  private void GenerateZCoordinates( int i )
  {
    if( _combinedMap?.Items != null )
    {
      _combinedMap.Items[i].Rz = 0;
      _combinedMap.Items[i].Tz = 0;
    }
  }

  private static bool IsAnyDupe( CueMap? combinedMap )
  { 
    var mapDupe = combinedMap?.Items?.GroupBy( c => c.ID )
                                                      .Where( m => m.Count() > 1 )
                                                      .ToList();
    
    return mapDupe?.Count > 1;
  }
}