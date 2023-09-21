namespace PAC_Map_Combiner_REST_API.Models;

public class MapDir
{
    public MapDir()
    {
    }

    public MapDir(string? directory, string? filter)
    {
        Directory = directory;
        Filter = filter;
    }

    public string? Directory { get; set; }
    public string? Filter { get; set; }
}