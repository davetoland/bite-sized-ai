using OpenAI.Chat;
using System.Text.Json;

var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var client = new ChatClient("o4-mini", key);
List<ChatMessage> messages = [new UserChatMessage("Get current temperatures in London, Lisbon and Limassol")];
const string function = "{ \"type\": \"object\", \"properties\": { \"latLng\": { \"type\": \"string\" }}}";
var tool = ChatTool.CreateFunctionTool(nameof(GetTemperatureAsync), "", BinaryData.FromString(function));

ChatCompletion completion;
while (true)
{
    completion = await client.CompleteChatAsync(messages, new ChatCompletionOptions { Tools = { tool }});
    if (completion.ToolCalls.Count == 0) break;
    messages.Add(new AssistantChatMessage(completion));
    foreach (var toolCall in completion.ToolCalls)
        messages.Add(new ToolChatMessage(toolCall.Id,
            await GetTemperatureAsync(
                JsonDocument.Parse(toolCall.FunctionArguments).RootElement
                    .GetProperty("latLng").GetString()?.Split(",") ?? [])));
}

Console.WriteLine(completion.Content[0].Text);

async Task<string> GetTemperatureAsync(string[] latLng)
{
    using var http = new HttpClient();
    var query = $"latitude={latLng[0]}&longitude={latLng[1]}&current_weather=true";
    return await http.GetStringAsync($"https://api.open-meteo.com/v1/forecast?{query}");
}
