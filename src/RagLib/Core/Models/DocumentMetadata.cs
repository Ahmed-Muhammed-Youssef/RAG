namespace RagLib.Core.Models
{
    /// <summary>
    /// Represents metadata associated with a document or a document chunk.
    /// Used to preserve source context for retrieval, filtering, or display.
    /// </summary>
    public class DocumentMetadata
    {
        /// <summary>
        /// Globally unique identifier for the document. Generated via Guid.NewGuid().
        /// </summary>
        public Guid? DocumentId { get; set; }
        
        /// <summary>
        /// The original name of the file (e.g., "document.pdf"). Optional.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// The full or relative path to the file's source location. Optional.
        /// </summary>
        public string? FilePath { get; set; }

        /// <summary>
        /// The page number from which this chunk originates, if the document was paginated
        /// (e.g., PDF, Word). This value refers to the physical page layout.
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// A dictionary for any custom metadata. Use this to store additional tags,
        /// language, content type, timestamps, user tags, or app-specific values.
        /// </summary>
        public Dictionary<string, string>? Custom { get; set; }
    }
}
