import fs from "fs";
import openai from "openai";

const client = new openai.OpenAI({apiKey: process.env.OPENAI_API_KEY});
const audioStream = fs.createReadStream('../voice_memo.mp3');

const memo = await client.audio.transcriptions.create({
  model: 'whisper-1',
  file: audioStream,
  response_format: 'verbose_json',
  timestamp_granularities: ['word'],
});

const subtitles = await client.chat.completions.create({
  model: 'gpt-4o-mini',
  messages: [
    {role: 'system', content: 'Turn this into word-level srt formatted text'},
    {role: 'user', content: JSON.stringify(memo.words)}
  ],
});

console.log(subtitles.choices[0].message.content);
