namespace Spell
{
    /// <summary>
    /// Serializable data that is managed via custom JsonConverters.
    /// </summary>
    public interface ISerializable
    {
        int ElementIndex { get; set; }
        string MainType { get; set; }
    }
}