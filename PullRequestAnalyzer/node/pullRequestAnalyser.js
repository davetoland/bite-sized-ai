import { z } from "zod";
import openai from "openai";
import { zodResponseFormat } from "openai/helpers/zod";

const client = new openai.OpenAI({apiKey: process.env.OPENAI_API_KEY});
const prApiUrl = "https://api.github.com/repos/jquery/jquery/pulls/1357";
const fetched = await fetch(prApiUrl, { headers: { "Accept": "application/vnd.github.v3.diff" }});
const diff = await fetched.text();

const ImpactAnalysis = z.object({
  impact_level: z.enum(["low", "medium", "high", "critical"]),
  impact_reason: z.string(),
  changed_files: z.array(z.string()),
  affected_subsystems: z.array(z.string()),
  approval_suggestions: z.string()
});

const response = await client.beta.chat.completions.parse({
  model: 'o4-mini',
  messages: [{role: 'user', content: `Perform impact analysis on the following pull request: ${diff}`}],
  response_format: zodResponseFormat(ImpactAnalysis, "impactAnalysis")
});
const analysis = response.choices[0].message.parsed;

console.log(JSON.stringify(analysis, null, 2));
