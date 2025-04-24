import openai from "openai";

const client = new openai.OpenAI({apiKey: process.env.OPENAI_API_KEY});

const image = await client.images.generate({
  model: "dall-e-3",
  prompt: "A bustling market with colorful fruits and vegetables under string lights",
  size: "1024x1024",
  quality: "standard",
})
const imageUrl = image.data[0].url

const response = await client.chat.completions.create({
  model: "o4-mini",
  messages: [{
      role: "user",
      content: [{
          type: "text",
          text: "Generate HTML that displays this with descriptive alt-text to aid visually impaired users."
        }, {
          type: "image_url", image_url: { url: imageUrl }
        }]
    }]
})

console.log(response.choices[0].message.content)
