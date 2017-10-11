using System;
using System.IO;
using System.Threading.Tasks;

namespace TaskManager.AutoComplete
{
    public static class TrieLoader
    {
        /// <summary>
        /// Загрузка словаря файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<Trie> LoadAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("Specified path is null or empty");
            }

            var fullPath = Path.GetFullPath(path);
            if (!File.Exists(fullPath))
            {
                throw new ArgumentException("Incorrect file path");
            }

            using (FileStream stream = File.OpenRead(fullPath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return await LoadAsync(reader).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Загрузка словаря из потока
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static async Task<Trie> LoadAsync(StreamReader reader)
        {
            if (reader == null || reader.EndOfStream)
            {
                throw new ArgumentException("Incorrect StreamReader state");
            }

            Trie result = new Trie();
            string currentLine = await reader.ReadLineAsync().ConfigureAwait(false);

            int wordsCount = 0;

            if (!int.TryParse(currentLine, out wordsCount))
            {
                throw new FormatException("Incorrect input data format. In first line must be dictionary size");
            }

            if (wordsCount <= 0 || wordsCount > 100000)
            {
                throw new FormatException("Incorrect input data format. Dictionary size should be greater than zero and less then 100 000");
            }

            for (int i = 0; i < wordsCount; i++)
            {
                currentLine = await reader.ReadLineAsync().ConfigureAwait(false);
                if (string.IsNullOrEmpty(currentLine))
                {
                    throw new FormatException("Incorrect input data format. Too less lines or empty line");
                }

                string[] current = currentLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (current.Length != 2)
                {
                    throw new FormatException("Incorrect input data format. Lines with words must contains word and occurrence delimeted by space");
                }

                if (current[0].Length > 15)
                {
                    throw new FormatException("Incorrect input data format. Words must contains less than 16 letters");
                }

                int occurrence = int.Parse(current[1]);

                if (occurrence <= 0 || occurrence > 1000000)
                {
                    throw new FormatException("Incorrect input data format. Word Occerrnce should be greater than zero and less then 1 000 000");
                }

                result.Add(current[0], occurrence);
            }

            return result;
        }
    }
}
