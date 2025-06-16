# RAGLib

RAGLib is a modular Retrieval-Augmented Generation (RAG) system written in C#. It provides flexible interfaces for building and extending RAG pipelines using pluggable components like vector stores, chunkers, and embedders.

## ✨ Features

- 🧩 Pluggable architecture via key interfaces:
  - `IDocumentChunker` – Converts documents into semantic chunks.
  - `IEmbedder` – Generates vector embeddings from text.
  - `IVectorStore` – Stores and searches vector embeddings.
  - `IRagEngine` – High-level engine that coordinates chunking, embedding, and retrieval.

- 🔌 Default Implementations:
  - **GeminiEmbedder** – Embeds text using the Gemini API.
  - **QdrantVectorStore** – Vector storage and search using Qdrant.
  - **FixedSizeChunker** – Simple chunker for splitting long documents into fixed chunks.

- ✅ Unit Tests:
  - Includes tests for `GeminiEmbedder` and `DefaultDocumentChunker`.

### Installation

Add the project to your solution or reference it via project dependency. NuGet packaging is planned.

## 🧪 Testing

Unit tests are under the `RagLib.Test` project:

```bash
dotnet test
```


## 🔧 Getting Started

### Prerequisites

- .NET 9 or later
- API key for Gemini (for embedding)
- Qdrant instance (local or cloud)

## 📄 License

MIT License – see `LICENSE` for more information.
