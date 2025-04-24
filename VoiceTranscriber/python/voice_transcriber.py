import os
import json
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
        {"role":"user","content":json.dumps(memo.words, default=lambda o: o.__dict__, indent=2)}
    ],
)

print(subtitles.choices[0].message.content)
