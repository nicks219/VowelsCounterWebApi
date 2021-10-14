using Newtonsoft.Json;

namespace WordCounter.DTO
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
