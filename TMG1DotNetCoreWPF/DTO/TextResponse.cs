using Newtonsoft.Json;

namespace TMG1DotNetCoreWPF.DTO
{
    internal class TextResponse
    {
        [JsonProperty("text")]
        internal string Text { get; set; }
    }
}
