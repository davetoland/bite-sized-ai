using OpenAI.Chat;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var client = new ChatClient("o4-mini", key);
const string prApiUrl = "https://api.github.com/repos/jquery/jquery/pulls/1357";
using var http = new HttpClient();
http.DefaultRequestHeaders.UserAgent.ParseAdd("none");
http.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3.diff"));

var diff = await (await http.GetAsync(prApiUrl)).Content.ReadAsStringAsync();
var message = new UserChatMessage($"Perform impact analysis on the following pull request: {diff}");
var response = await client.CompleteChatAsync([message], new ChatCompletionOptions
{
    ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat("openApi3",
        BinaryData.FromString(NJsonSchema.JsonSchema.FromType<ImpactAnalysis>().ToJson()))
});

Console.WriteLine(response.Value.Content[0].Text);

record ImpactAnalysis(
    [property: JsonPropertyName("impact_level")] string ImpactLevel,
    [property: JsonPropertyName("impact_reason")] string ImpactReason,
    [property: JsonPropertyName("changed_files")] List<string> ChangedFiles,
    [property: JsonPropertyName("affected_subsystems")] List<string> AffectedSubsystems,
    [property: JsonPropertyName("approval_suggestions")] string ApprovalSuggestions);