using System.Collections.Generic;
using Newtonsoft.Json;

public class ASTNode
{
    [JsonProperty("_type")]
    public string Type { get; set; }

    [JsonProperty("body")]
    public List<ASTNode> Body { get; set; }

    [JsonProperty("targets")]
    public List<ASTNode> Targets { get; set; }

    [JsonProperty("value")]
    public object Value { get; set; }

    [JsonProperty("test")]
    public ASTNode Test { get; set; }

    [JsonProperty("comparators")]
    public List<ASTNode> Comparators { get; set; }

    [JsonProperty("args")]
    public List<ASTNode> Args { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("func")]
    public ASTNode Func { get; set; }

    [JsonProperty("left")]
    public ASTNode Left { get; set; }

    [JsonProperty("right")]
    public ASTNode Right { get; set; }

    [JsonProperty("op")]
    public ASTNode Op { get; set; }
}
