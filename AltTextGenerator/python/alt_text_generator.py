import os
from openai import OpenAI

client = OpenAI(api_key=os.getenv("OPENAI_API_KEY"))

image_url = client.images.generate(
    model="dall-e-3",
    prompt="A bustling market with colorful fruits and vegetables under string lights",
    size="1024x1024",
    quality="standard"
).data[0].url

response = client.chat.completions.create(
    model="o4-mini",
    messages=[{
        "role": "user",
        "content": [{
            "type":"text",
            "text": "Generate HTML that displays this with descriptive alt‑text to aid visually impaired users."
        }, {
            "type":"image_url", "image_url":{ "url": f"{image_url}" }
        }]
    }]
)

print(response.choices[0].message.content)