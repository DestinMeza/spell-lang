namespace SpellCompiler
{
    [System.Serializable]
    public class SimpleTranslation : ITranslation
    {
        public static string TypeName => nameof(SimpleTranslation);
        public SerializableVector3 Direction { get; set; }
        public string MainType { get; set; }
        public int ElementIndex { get; set; }

        public SimpleTranslation(SerializableVector3 direction)
        {
            Direction = direction;
            MainType = GetType().Name;
        }
    }
}