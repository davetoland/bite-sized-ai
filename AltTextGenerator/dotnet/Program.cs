using OpenAI.Images;
using OpenAI.Chat;

var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var imageClient = new ImageClient("dall-e-3", key);

var image = await imageClient.GenerateImageAsync(
    "A bustling market with colorful fruits and vegetables under string lights",
    new ImageGenerationOptions
    {
        Size = GeneratedImageSize.W1024xH1024, 
        Quality = GeneratedImageQuality.Standard
    });
    
var chatClient = new ChatClient("o4-mini", key);
var response = await chatClient.CompleteChatAsync(
    new UserChatMessage(
        ChatMessageContentPart.CreateTextPart(
        "Generate HTML that displays this with descriptive alt-text to aid visually impaired users."),
        ChatMessageContentPart.CreateImagePart(image.Value.ImageUri)));

Console.WriteLine(response.Value.Content[0].Text);
