using System.Text.Json;
using OpenAI.Audio;
using OpenAI.Chat;

var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var client = new AudioClient("whisper-1", key);
var memo = await client.TranscribeAudioAsync("voice_memo.mp3", 
    new AudioTranscriptionOptions
    {
        TimestampGranularities = AudioTimestampGranularities.Word,
        ResponseFormat = AudioTranscriptionFormat.Verbose
    });

var chat = new ChatClient("gpt-4o-mini", key);
var subtitles = await chat.CompleteChatAsync(
    new List<ChatMessage> {
        new SystemChatMessage("Turn this into word-level srt formatted text"),
        new UserChatMessage(JsonSerializer.Serialize(memo.Value.Words))
    });

Console.WriteLine(subtitles.Value.Content[0].Text);
