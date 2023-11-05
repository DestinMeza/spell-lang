using System.IO;
using System.Threading;

namespace Spell.IO
{
    /// <summary>
    /// SpellFile is a C# conversion of a .spl file. That's used to help implement skill functionality.
    /// </summary>
    [System.Serializable]
    public class SpellFile
    {
        private const string EXTENSION = ".spl";
        public string FilePath => _pathToFile + EXTENSION;
        private string _pathToFile;
        private string _sourceText;

        // Avalible Functions that the a Spell Can use.
        public SpellFunction[] SpellFunctions => spellFunctions;

        private SpellFunction[] spellFunctions;

        public SpellFile(string pathToFile)
        {
            _pathToFile = pathToFile;

            if (!File.Exists(pathToFile + EXTENSION)) 
            {
                Diagnostics.LogErrorMessage($"{FilePath} path not found.");
                return;
            }

            _sourceText = File.ReadAllText(FilePath, System.Text.Encoding.UTF8);
        }

        public void ReadSpellFileAsync(CancellationToken ct) 
        {
            if (string.IsNullOrWhiteSpace(_sourceText)) return;

            SpellCompiler spellCompiler = new SpellCompiler();

            spellCompiler.RunAsync(_sourceText, ct);
        }

        public void ReadSpellFile() 
        {
            if (string.IsNullOrWhiteSpace(_sourceText)) return;

            SpellCompiler spellCompiler = new SpellCompiler();

            spellCompiler.Run(_sourceText);
        }

        public override string ToString()
        {
            return _sourceText;
        }
    }
}