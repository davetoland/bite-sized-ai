# Bitesized AI

**Bitesized AI** is a 5 part mini-series I've written, exploring concepts of applied AI using bite sized examples that interact with the OpenAI APIs. Each part consists of simplified code snippets, around 20-30 lines, each written in Python, JavaScript and C#, to showcase some of the functionality available for you to use in your applications.

## Bitesized AI: 01 - Voice Transcriber (Whisper Transcription)


Here we're using Whisper, a powerful automatic speech recognition model for fast and accurate audio transcriptions. It can produce granular time-stamping, understand around 100 different languages, and can cope with accents, regional dialects, and many slang expressions.

For this bite sized demo, we're transcribing some spoken audio using word-level timestamps. Then, by applying a single LLM post-processing step, we map those timestamps and text segments into SRT subtitle cues.

Key takeaways:

✅ Accuracy: Whisper is extremely accurate, easily able to transcribe noisy, or multi-speaker audio, word-for-word.  
✅ Language-Agnostic: Whisper can understand almost any language, including accents, slang and regional dialect.  
✅ Timestamp Precision – delivers millisecond‑accurate, word‑level timestamps for perfect alignment or analysis.  
✅ Reasoning Flexibility – once transcribed, you can feed the output into any LLM pipeline for summary, conversion or sentiment analysis.

---

## Bitesized AI: 02 - Pull Request Analyser (Structured Outputs)

Here we’re putting structured outputs front and center, which eliminates brittle regex parsing or flaky string hacks to pull data out of verbose AI replies. Instead, we force the model to match a predefined schema, so you get ready‑to‑use objects every time.

For this bite sized demo, we're using a notorious PR from the jQuery repo that instantly broke backwards compatibility by removing two legacy methods. This code not only recognises the impact risk of the PR, but formats the output into a typed structure which can be easily ingested by dashboards, alert systems or CI gates.

Key takeaways:

✅ Parsing: no more headaches from wrestling with brittle regex or ad‑hoc string parsing.  
✅ Consistency: every response adheres to a schema, fields are always present and correctly typed.  
✅ Integration: plug the resulting objects straight into your codebase, CI pipelines, or analytics dashboards.  
✅ Maintainability: changes to the schema propagate cleanly, so your data contracts stay in sync as your use cases evolve.

---

## Bitesized AI: 03 – Alt Text Generator (Image Generation & Computer Vision)

Here we’re combining AI’s creative power with automated storytelling. First, we prompt the image‑generation model to produce a custom graphic. Then, we feed that into a image-recognition-capable model with an additional prompt for analysis.

For this bite sized demo, we’ll generate a generic picture of a bustling market scene, then have the AI analyse the image and produce HTML markup to display it, complete with accurate auto-generated alt-text to perfectly describe it for visually impaired visitors using a screen reader.

Key takeaways:

✅ Generative Vision – leverage generative image models to create synthetic visual data for downstream tasks.  
✅ Computer Vision Analysis – apply vision-capable language models to semantically interpret image content.  
✅ Multimodal Pipelines – chain text and vision models into a unified, end-to-end multimedia workflow.  
✅ Structured Output – steer AI to emit machine-readable, schema-driven results (e.g. HTML + alt-text).

---

## Bitesized AI: 04 – Movie Recommender (Embeddings & Semantic Search)

Here we’re harnessing embeddings to power semantic similarity search, enabling a lightweight movie-recommendation engine. We compute vector representations for each movie’s overview with OpenAI’s embedding model, upsert them into a Pinecone index, and then query that index for the nearest neighbors, dynamically surfacing films that are similar based on their content rather than their metadata.

For this bite sized demo, we load the top 500 movies, embed each synopsis with text-embedding-ada-002, store the vectors in Pinecone, and finally run a similarity query (excluding the seed movie) to list the five most semantically related titles.

Key takeaways:

✅ Semantic Understanding – go beyond keyword matching by comparing nuanced meaning rather than words.  
✅ Unsupervised Recommendations – build recommendation systems without any labeled training data.  
✅ Vector-based Retrieval – use vector databases for fast, scalable nearest-neighbor search over embeddings.  
✅ Domain Agnostic – apply the same technique across any unstructured data (documents, products, feedback, etc).

---

## Bitesized AI: 05 – Weather Agent (Function Calling)

Here we’re supercharging the model with programmatic actions, giving it the ability to call custom functions, the core building blocks of agentic AI systems. By defining a function schema we enable the model to output structured function‑call objects instead of plain text. Our code then executes those calls, passing each result back into the model for further processing. The AI decides how and when to call those functions, and furthermore what to do with the results.

In this bite sized demo, we’ll register an API function to get weather data, feed a natural‑language user request in, and let the agent call the API as many times as it decides it needs to. Here we ask for info on 3 cities (London, Lisbon and Limassol) the agent realises it needs to provide latitude and longitude inputs to the API (which it produces), the JSON responses it gets are a mixture of relevant and irrelevant data with the key parts (temperature unit and value) in separate sections. It does its magic and extracts them, repeating this process for each city, before stitching them all together to produce a cohesive final structured reply.

Key takeaways:

✅ Advanced Reasoning – the agent ingests raw API outputs, intelligently combining them into a cohesive response.  
✅ Safety – restrict the model’s capabilities to approved functions, reducing unwanted actions.  
✅ Dynamic Chaining – let the agent decide which functions to call and in what sequence for complex workflows.  
✅ Extensibility – add new functions as your use cases grow, and the agent adapts without retraining.
