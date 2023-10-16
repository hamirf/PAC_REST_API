using System.Text.Json.Nodes;
using PAC_Map_Combiner_REST_API.Models;
using PAC_Map_Combiner_REST_API.Utilities;

namespace PAC_Map_Combiner_REST_API.Repositories;

public class MapRepository : IMapRepository
{
    private string? _mapXml;

    /// <summary>
    /// Create an object of map repository with a file path of encoder config file as parameter
    /// </summary>
    /// <param name="encoderPath">A root path of encoder config json file</param>
    public MapRepository( string encoderPath )
    {
      InitMapDirectory( encoderPath );
    }

    /// <summary>
    /// Get data of a combined map
    /// </summary>
    /// <returns>A combined map object</returns>
    public CueMap? GetCombinedMap()
    {
        if( !File.Exists( _mapXml ) ) _mapXml = null;
        
        return _mapXml != null ? new MapDeserializer( _mapXml ).Execute() : null;
    }
    
    /// <summary>
    /// Combine files of map into a new file
    /// </summary>
    /// <param name="files">Uploaded files</param>
    /// <returns>Status when combining map, whether it's success or fail</returns>
    public string CombineMap( IFormFile[] files )
    {
        if( NoFileSelected( files ) ) return "please upload at least 1 file";
        if( FileExtNotXml( files ) ) return "please upload xml file(s) only";
        List<CueMap?> maps = MergeMaps( files ).ToList();
        CueMap? combinedMap = new MapCombiner( maps! ).Combine();
        string combineStatus = GetCombineMapStatus( combinedMap );
        if( combineStatus.Contains( "saved" ) ) new MapSerializer( combinedMap, _mapXml! ).SaveToXml();
        
        return combineStatus;
    }

    // Update Single Coordinate
    public string UpdateMap( uint cueId, Cue coordinate )
    {
      CueMap? oldMap = new MapDeserializer( _mapXml! ).Execute();
      if( DuplicateID( oldMap?.Items!, cueId, coordinate.ID ) ) return "ID already exists";
      if( DuplicateTx( oldMap?.Items!, cueId, coordinate.Tx ) ) return "Tx already exists";
      UpdatedMap( oldMap?.Items!, cueId, coordinate );
      new MapSerializer( oldMap, _mapXml! ).SaveToXml();

      return "map successfully updated";
    }

    // Update Multiple Coordinates at once
    public string UpdateMapInBatch( Cue[] coordinates )
    {
      CueMap? oldMap = new MapDeserializer( _mapXml! ).Execute();
      string updateMapInBatch = CheckBatchDupe( coordinates );
      if ( !String.IsNullOrEmpty( updateMapInBatch ) ) return updateMapInBatch;
      UpdatedCoordinateOfMap( oldMap?.Items!, coordinates );
      new MapSerializer( oldMap, _mapXml! ).SaveToXml();
      
      return "map successfully updated";
    }

    // Constructor InitMapDirectory Support Methods
    /// <summary>
    /// Initialize a new directory for the combined map
    /// </summary>
    /// <param name="encoderPath">A root path of encoder config json file</param>
    private void InitMapDirectory( string encoderPath )
    {
      string configFile = GetConfigFile( encoderPath );
      string config = ReadFileConfig( configFile );
      GetPathForMapXml( config );
    }

    /// <summary>
    /// Get a file path of json file of encoder config
    /// </summary>
    /// <param name="encoderPath">A root path of encoder config json file</param>
    /// <returns>A file path of json file of encoder config</returns>
    private static string GetConfigFile( string encoderPath )
    {
      string file = Path.Combine( encoderPath, @"EncoderCamera\encoder_config.json" );
      
      return file;
    }

    /// <summary>
    /// Read the encoder config file
    /// </summary>
    /// <param name="file">A file path of json file of encoder config</param>
    /// <returns>A string of config file content with form of json</returns>
    private static string ReadFileConfig( string file )
    {
      using var stream = new StreamReader( file );
      var outputFile = stream.ReadToEnd();
      
      return outputFile;
    }

    /// <summary>
    /// Get the map path from json string
    /// </summary>
    /// <param name="config">A json string of encoder config file</param>
    private void GetPathForMapXml( string config )
    {
      JsonNode jsonString = JsonNode.Parse( config )!;
      _mapXml = jsonString["pathMap"]?.GetValue<string>().Trim( '"' ) ?? String.Empty;
      CreateMapDirectory( _mapXml );
    }

    /// <summary>
    /// Create a directory for the map
    /// </summary>
    /// <param name="mapPath">A file path destination for the map</param>
    private static void CreateMapDirectory( string mapPath )
    {
      string? directory = Path.GetDirectoryName( mapPath );
      Directory.CreateDirectory( directory ?? String.Empty );
    }

    // CombineMap Support Methods
    /// <summary>
    /// Check whether there is any file uploaded or not 
    /// </summary>
    /// <param name="files">Uploaded files</param>
    /// <returns>True if there's not any file uploaded, otherwise false</returns>
    private static bool NoFileSelected( IEnumerable<IFormFile> files )
    {
      if ( !files.Any() )
      {
        return true;
      }

      return false;
    }

    /// <summary>
    /// Check whether the uploaded files extension are xml or not
    /// </summary>
    /// <param name="files">Uploaded files</param>
    /// <returns>True if the uploaded files extension are xml, otherwise false</returns>
    private static bool FileExtNotXml( IEnumerable<IFormFile> files )
    {
      if ( files.Any( file => !file.FileName.Contains( ".xml" ) ) )
      {
        return true;
      }

      return false;
    }
    
    /// <summary>
    /// Merge uploaded maps from stream and create a list of it
    /// </summary>
    /// <param name="files">Uploaded files</param>
    /// <returns>List of maps</returns>
    private static IEnumerable<CueMap?> MergeMaps( IEnumerable<IFormFile> files )
    {
      return files.Select( file => new MapDeserializer( file.OpenReadStream() ).Execute() )
                                                                               .Where( map => map != null )
                                                                               .ToList();
    }

    /// <summary>
    /// Get a status while combining map
    /// </summary>
    /// <param name="map">Combined map object</param>
    /// <returns>Status while combining map</returns>
    private string GetCombineMapStatus( CueMap? map )
    {
      string success = $"map saved to {_mapXml}";
      string fail = "there are duplicate ID on the map";

      return map != null ? success : fail;
    }
    
    // Auxiliary methods of { Update Single Coordinate } method
    private static bool DuplicateID( List<Cue> oldMapItems, uint cueId, uint coordinateId )
    {
      return oldMapItems.Any( coordinate => coordinate.CueId != cueId && coordinate.ID == coordinateId );
    }

    private static bool DuplicateTx( List<Cue> oldMapItems, uint cueId, int coordinateTx )
    {
      return oldMapItems.Any( coordinate => coordinate.CueId != cueId && coordinate.Tx == coordinateTx );
    }

    private static void UpdatedMap( List<Cue> oldMapItems, uint cueId, Cue coordinate )
    {
      foreach( var mapCoordinate in oldMapItems.Where( c => c.CueId == cueId ) )
      {
        mapCoordinate.ID = coordinate.ID;
        mapCoordinate.Tx = coordinate.Tx;
      }
    }

    // Auxiliary methods of { Update Multiple Coordinates at once } method
    private static string CheckBatchDupe( Cue[] coordinates )
    {
      IsInputStatusNotEmpty( coordinates, out string inputStatus );
      return !( inputStatus.Equals( String.Empty ) ) ? inputStatus : String.Empty;
    }

    private static void IsInputStatusNotEmpty( Cue[] coordinates, out string inputDupeStatus )
    {
      if ( DuplicateInputIDs( coordinates ) ) inputDupeStatus = "ID(s) already exists";
      else if ( DuplicateInputTxs( coordinates ) ) inputDupeStatus = "Tx(s) already exists";
      else inputDupeStatus = String.Empty;
    }

    private static bool DuplicateInputIDs( Cue[] coordinates )
    {
      var dupeTx = coordinates.GroupBy( coordinate => coordinate.ID )
                                                    .Where( coordinate => coordinate.Count() > 1 );
      
      return dupeTx.Any();
    }

    private static bool DuplicateInputTxs( Cue[] coordinates )
    {
      var dupeTx = coordinates.GroupBy( coordinate => coordinate.Tx )
                                                    .Where( coordinate => coordinate.Count() > 1 );
      return dupeTx.Any();
    }

    private static void UpdatedCoordinateOfMap( List<Cue> oldMapItems, Cue[] coordinates )
    {
      foreach( var oldCoordinate in oldMapItems )
      {
        FindOldCoordinatesByCueId(coordinates, oldCoordinate);
      }
    }

    private static void FindOldCoordinatesByCueId(Cue[] coordinates, Cue oldCoordinate)
    {
      foreach (var coordinate in coordinates)
      {
        ChangeOldCoordinateValues(oldCoordinate, coordinate);
      }
    }

    private static void ChangeOldCoordinateValues(Cue oldCoordinate, Cue coordinate)
    {
      if (coordinate.CueId == oldCoordinate.CueId)
      {
        oldCoordinate.ID = coordinate.ID;
        oldCoordinate.Tx = coordinate.Tx;
      }
    }
}