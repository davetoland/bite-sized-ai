import openai from "openai";

const client = new openai.OpenAI({apiKey: process.env.OPENAI_API_KEY});
const messages = [{"role": "user", "content": "Get the current temperatures in London, Lisbon and Limassol"}];
const tools = [{
  "type": "function",
  "function": {
    "name": "temp",
    "parameters": { "type": "object", "properties": { "latLng": { "type": "string" }}}
  }  
}];

const getTemp = async (latLng) => {
  const url = 'https://api.open-meteo.com/v1/forecast?'
  const fetched = await fetch(`${url}latitude=${latLng[0]}&longitude=${latLng[1]}&current=temperature_2m`);
  return await fetched.text();
} 

let completion = null;
while (true) {
  completion = await client.chat.completions.create({model: "o4-mini", tools: tools, messages: messages});
  if (completion.choices[0].message.tool_calls === undefined) break;
  messages.push(completion.choices[0].message);
  for (const toolCall of completion.choices[0].message.tool_calls) {
    const result = await getTemp(JSON.parse(toolCall.function.arguments).latLng.split(","));
    messages.push({role: "tool", tool_call_id: toolCall.id, content: result});
  }
}

console.log(completion.choices[0].message.content)
