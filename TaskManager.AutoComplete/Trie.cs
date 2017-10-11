using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("AutoComplete.Tests")]
namespace TaskManager.AutoComplete
{
    public sealed class Trie
    {
        /// <summary>
        /// Коревой элемент словаря
        /// </summary>
        private readonly TrieNode _root;

        public Trie()
        {
            _root = new TrieNode();
        }

        /// <summary>
        /// Добавляем слово в словарь
        /// </summary>
        /// <param name="word">Слово</param>
        /// <param name="occurrence">встречаемость</param>
        public void Add(string word, int occurrence)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentNullException("Incorrect input word");
            }

            if (word.ToLower() != word || word.Any(x => !char.IsLetter(x)))
            {
                throw new ArgumentException("Word must contains only letters and be in lowwer case");
            }

            if (occurrence <= 0)
            {
                throw new ArgumentException("Occurrence bust greater than 0");
            }

            TrieNode current = _root;
            foreach (char keyPart in word)
            {
                if (!current.Leaves.ContainsKey(keyPart))
                {
                    current.Leaves.Add(keyPart, new TrieNode());
                }

                current = current.Leaves[keyPart];

                if (current.CurrentWord == word)
                {
                    throw new ArgumentException("Current word is already exists");
                }

                current.AddWord(word, occurrence);
            }

            current.CurrentWord = word;
        }

        /// <summary>
        /// Получение вариантов подстановки слов по подстроке
        /// </summary>
        /// <param name="substring"></param>
        /// <returns></returns>
        public IEnumerable<string> Get(string substring)
        {
            if (string.IsNullOrEmpty(substring) || substring.Length > 15)
            {
                throw new ArgumentException("Incorrect input substring");
            }

            string lowwerString = substring.ToLower();
            TrieNode current = _root;
            foreach (char keyPart in lowwerString)
            {
                if (!current.Leaves.ContainsKey(keyPart))
                {
                    return null;
                }

                current = current.Leaves[keyPart];
            }

            return current.Words;
        }

        public async Task<IEnumerable<string>> GetAsync(string substring)
        {
            return await Task.Run(() => Get(substring));
        }
    }
}
