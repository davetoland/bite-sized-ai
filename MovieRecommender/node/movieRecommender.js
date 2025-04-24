import fs from "fs/promises";
import openai from "openai";
import { Pinecone } from "@pinecone-database/pinecone";

const client = new openai.OpenAI({apiKey: process.env.OPENAI_API_KEY});
const vectordb = new Pinecone({apiKey: process.env.PINECONE_API_KEY});
const model = "text-embedding-ada-002";
await vectordb.createIndex({name: 'movies', dimension: 1536, metric: "cosine", suppressConflicts: true,
  spec: { serverless: { cloud: 'aws', region: 'us-east-1' }}})

const index = vectordb.Index('movies');
const movies = JSON.parse(await fs.readFile("top_500_movies.json", "utf-8"));
const stats = await index.describeIndexStats();
if (stats.totalRecordCount < movies.length)
  for (const movie of movies) {
    const res = await client.embeddings.create({model: model, input: movie.overview});
    await index.upsert({
      id: movie.id,
      values: res.data[0].embedding,
      metadata: {'title': movie.original_title, 'overview': movie.overview}
    });
  }

const selected = movies.find(m => m.original_title === "Saving Private Ryan");
const embRes = await client.embeddings.create({model: model, input: selected.overview});
const queryRes = await index.query({ vector: embRes.data[0].embedding, topK: 5, includeMetadata: true,
  filter: { title: { $ne: selected.original_title } }});

console.log(`\n${selected.original_title}\n${selected.overview}\n`);
console.log("Similar movies:");
for (const match of queryRes.matches)
  console.log(`${match.metadata.title}\n${match.metadata.overview}\n`);
