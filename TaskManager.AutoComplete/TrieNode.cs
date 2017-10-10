using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TaskManager.AutoComplete.Tests")]
namespace TaskManager.AutoComplete
{
    internal sealed class TrieNode
    {
        /// <summary>
        /// Ограничение на размер списка слов
        /// </summary>
        private const int _wordsLimit = 10;

        /// <summary>
        /// Компаратор для сортировки "дочерних" слов. Сортирует слова сначала по встречаемости в порядке убывания, затем - по алфавиту
        /// </summary>
        private static readonly Comparer<KeyValuePair<int, string>> _comparer = Comparer<KeyValuePair<int, string>>.Create((x, y) =>
        {
            int result = y.Key.CompareTo(x.Key);
            return result == 0
                ? x.Value.CompareTo(y.Value)
                : result;
        });

        /// <summary>
        /// Отсортированный список слов, которые можно получить по введенной подстроке
        /// </summary>
        private readonly SortedList<KeyValuePair<int, string>, string> _words;

        /// <summary>
        /// Слово, добавленное в словарь
        /// </summary>
        internal string CurrentWord { get; set; }

        /// <summary>
        /// Листовые элементы
        /// </summary>
        internal Dictionary<char, TrieNode> Leaves { get; }

        /// <summary>
        /// Интерфейс для получения отсортированного списка слов по введенной подстроке
        /// </summary>
        internal IEnumerable<string> Words { get { return _words.Values; } }

        /// <summary>
        /// Конструктор
        /// </summary>
        public TrieNode()
        {
            Leaves = new Dictionary<char, TrieNode>();
            _words = new SortedList<KeyValuePair<int, string>, string>(_comparer);
        }

        /// <summary>
        /// Добавляем слово в отсортированный список.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="occurrence"></param>
        internal void AddWord(string word, int occurrence)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentNullException("Incorrect input word");
            }

            if (occurrence <= 0)
            {
                throw new ArgumentException("Occurrence bust greater than 0");
            }

            _words.Add(new KeyValuePair<int, string>(occurrence, word), word);

            if (_words.Count > _wordsLimit)
            {
                _words.RemoveAt(_wordsLimit);
            }
        }
    }
}
