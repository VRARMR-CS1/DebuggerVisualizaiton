using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;

public class ASTNode
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("body")]
    public List<ASTNode> Body { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("value")]
    public JObject Value { get; set; }

    [JsonProperty("func")]
    public ASTNode Func { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("targets")]
    public List<ASTNode> Targets { get; set; }

    [JsonProperty("s")]
    public string S { get; set; }

    [JsonProperty("lineno")]
    public int? LineNumber { get; set; }

    [JsonProperty("col_offset")]
    public int? ColOffset { get; set; }

    [JsonProperty("end_lineno")]
    public int? EndLineNumber { get; set; }

    [JsonProperty("end_col_offset")]
    public int? EndColOffset { get; set; }

    [JsonProperty("left")]
    public ASTNode Left { get; set; }

    [JsonProperty("op")]
    public string Op { get; set; }

    [JsonProperty("right")]
    public ASTNode Right { get; set; }
}

public class Arguments
{
    [JsonProperty("args")]
    public List<ASTNode> Args { get; set; }

    [JsonProperty("posonlyargs")]
    public List<ASTNode> PosOnlyArgs { get; set; }

    [JsonProperty("vararg")]
    public ASTNode VarArg { get; set; }

    [JsonProperty("kwonlyargs")]
    public List<ASTNode> KwOnlyArgs { get; set; }

    [JsonProperty("kw_defaults")]
    public List<ASTNode> KwDefaults { get; set; }

    [JsonProperty("kwarg")]
    public ASTNode KwArg { get; set; }

    [JsonProperty("defaults")]
    public List<ASTNode> Defaults { get; set; }
}
