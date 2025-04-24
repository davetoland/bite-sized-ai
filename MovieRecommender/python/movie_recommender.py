import json
import os
from openai import OpenAI
from pinecone import Pinecone, ServerlessSpec

client = OpenAI(api_key=os.environ.get("OPENAI_API_KEY"))
vectordb = Pinecone(api_key=os.getenv("PINECONE_API_KEY"))
model = "text-embedding-ada-002"
if 'movies' not in vectordb.list_indexes().names():
  spec = ServerlessSpec(cloud="aws", region="us-east-1")
  vectordb.create_index(name='movies', dimension=1536, metric="cosine", spec=spec)

index = vectordb.Index('movies')
with open("top_500_movies.json", encoding="utf-8") as f:
  movies = json.load(f)

if index.describe_index_stats().total_vector_count < len(movies):
  for movie in movies:
    index.upsert(vectors=[{
      "id": str(movie["id"]),
      "values": client.embeddings.create(model=model, input=movie['overview']).data[0].embedding,
      "metadata": {"title": movie['original_title'], "overview": movie['overview']}}])

selected_movie = next(m for m in movies if m["original_title"] == "Saving Private Ryan")
embedding = client.embeddings.create(model=model, input=selected_movie['overview'])
result = index.query(
  vector=embedding.data[0].embedding, top_k=5, include_metadata=True,
  filter={ "title": { "$ne": selected_movie['original_title'] }})

print(f"{selected_movie['original_title']}\n {selected_movie['overview']}")
print("\nSimilar movies:")
for similar in result['matches']:
    print(f"{similar['metadata']['title']}\n {similar['metadata']['overview']}\n")
    