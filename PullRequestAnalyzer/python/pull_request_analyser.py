import os
import requests
from pydantic import BaseModel
from typing import Literal
from openai import OpenAI

client = OpenAI(api_key=os.getenv("OPENAI_API_KEY"))
pr_api_url = "https://api.github.com/repos/jquery/jquery/pulls/1357"
diff = requests.get(pr_api_url, headers={ "Accept": "application/vnd.github.v3.diff" }).text

class ImpactAnalysis(BaseModel):
    impact_level: Literal["low", "medium", "high", "critical"]
    impact_reason: str
    changed_files: list[str]
    affected_subsystems: list[str]
    approval_suggestions: str

response = client.beta.chat.completions.parse(
    model="o4-mini",
    messages=[{"role": "user", "content": f"Perform impact analysis on the following pull request: {diff}"}],
    response_format=ImpactAnalysis
)
analysis = response.choices[0].message.parsed

print(analysis.model_dump())
