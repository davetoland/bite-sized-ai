using Pinecone;
using OpenAI.Embeddings;
using System.Text.Json;
using System.Text.Json.Serialization;

var openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var pineconeKey = Environment.GetEnvironmentVariable("PINECONE_API_KEY");
var client = new EmbeddingClient("text-embedding-ada-002", openAiKey);
var vectordb = new PineconeClient(pineconeKey!);
if ((await vectordb.ListIndexes()).All(x => x.Name != "movies"))
    await vectordb.CreateServerlessIndex("movies", 1536, Metric.Cosine, "aws", "us-east-1");

var index = await vectordb.GetIndex("movies");
var movies = await JsonSerializer.DeserializeAsync<List<Movie>>(File.OpenRead("top_500_movies.json"));
if ((await index.DescribeStats()).TotalVectorCount < movies?.Count)
{
    ReadOnlyMemory<float> Vals(Movie x) => client.GenerateEmbedding(x.Overview).Value.ToFloats();
    MetadataMap Meta(Movie x) => new() { ["title"] = x.Title, ["overview"] = x.Overview };
    await index.Upsert(movies.Select(x => new Vector { Id = x.Id, Values = Vals(x), Metadata = Meta(x) }));
}

var selectedMovie = movies?.SingleOrDefault(x => x.Title == "Saving Private Ryan");
var embedding = client.GenerateEmbedding(selectedMovie?.Overview).Value.ToFloats();
var results = await index.Query(embedding, 5, includeMetadata: true, 
    filter: new MetadataMap { ["title"] = new MetadataMap { ["$ne"] = selectedMovie?.Title }});

Console.WriteLine($"{selectedMovie?.Title}\n {selectedMovie?.Overview}\n");
Console.WriteLine("Similar movies:\n");
foreach (var result in results.Select(x => x.Metadata).Where(x => x != null))
    Console.WriteLine($"{result!["title"].Inner}\n {result["overview"].Inner}\n");

internal record Movie(
    [property: JsonPropertyName("id")]string Id, 
    [property: JsonPropertyName("original_title")]string Title, 
    [property: JsonPropertyName("overview")]string Overview);