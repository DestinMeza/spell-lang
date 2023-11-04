namespace SpellCompiler
{
    [System.Serializable]
    public class ManaResource : IResource
    {
        public static string TypeName => nameof(ManaResource);
        public int ResourceValue { get; set; }
        public string MainType { get; set; }
        public int ElementIndex { get; set; }

        public ManaResource()
        {
            MainType = GetType().Name;
        }
    }
}