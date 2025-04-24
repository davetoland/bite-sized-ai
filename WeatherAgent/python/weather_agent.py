import os
import json
import requests
from openai import OpenAI

client = OpenAI(api_key=os.environ.get("OPENAI_API_KEY"))
messages = [{"role": "user", "content": "Get the current temperatures in London, Lisbon and Limassol"}]
tools = [{
    "type": "function",
    "function": {
        "name": "temp",
        "parameters": { "type": "object", "properties": { "latLng": { "type": "string" }}}
    }
}]

def get_temp(latlng):
    url = 'https://api.open-meteo.com/v1/forecast?'
    return requests.get(f'{url}latitude={latlng[0]}&longitude={latlng[1]}&current=temperature_2m').text

while True:
    completion = client.chat.completions.create(model="o4-mini", tools=tools, messages=messages)
    if completion.choices[0].message.tool_calls is None: break
    messages.append(completion.choices[0].message)
    for tool_call in completion.choices[0].message.tool_calls:
        result = get_temp(json.loads(tool_call.function.arguments)['latLng'].split(','))
        messages.append({"role": "tool", "tool_call_id": tool_call.id, "content": result})

print(completion.choices[0].message.content)