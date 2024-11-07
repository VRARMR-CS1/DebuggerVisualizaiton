using System.Collections.Generic;
using Newtonsoft.Json;

public class CodeFrame
{
    [JsonProperty("line_number")]
    public int Line { get; set; }

    [JsonProperty("locals")]
    public Dictionary<string, object> Locals { get; set; }

    [JsonProperty("globals")]
    public Dictionary<string, object> Globals { get; set; }

    [JsonProperty("functions")]
    public Dictionary<string, object> Functions { get; set; }

    [JsonProperty("print", NullValueHandling = NullValueHandling.Ignore)]
    public string Print { get; set; }
}
