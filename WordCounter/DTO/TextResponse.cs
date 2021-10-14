using Newtonsoft.Json;

namespace TMG1DotNetCoreWPF.DTO
{
    //
    // Summary:
    //     DTO for Json
    internal class TextResponse
    {
        [JsonProperty("text")]
        internal string Text { get; set; }
    }
}
