import os
from openai import OpenAI

client = OpenAI(api_key=os.environ.get("OPENAI_API_KEY"))
memo = client.audio.transcriptions.create(
    model="whisper-1",
    file=open("../voice_memo.mp3", "rb"),
    response_format="verbose_json",
    timestamp_granularities=["word"]
)

subtitles = client.chat.completions.create(
    model="gpt-4o-mini",
    messages=[
        {"role":"system","content":"Turn this into word-level srt formatted text"},
        {"role":"user","content":memo.text}
    ],
)

print(subtitles.choices[0].message.content)
