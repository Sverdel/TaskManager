using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TaskManager.AutoComplete.Tests
{
    [TestClass]
    public class TrieLoaderTest
    {
        [TestMethod]
        public async Task LoadTest()
        {
            Stopwatch sw = Stopwatch.StartNew();
            int found = 0, notfound = 0;
            FileInfo file = new FileInfo("test.in");

            using (FileStream stream = file.OpenRead())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    long stopBytes = 0;

                    long startBytes = GC.GetTotalMemory(true);
                    Trie result = await TrieLoader.LoadAsync(reader).ConfigureAwait(false);
                    stopBytes = GC.GetTotalMemory(true);

                    Console.WriteLine("Size is " + ((long)(stopBytes - startBytes)).ToString());

                    string currentLine = reader.ReadLine();
                    int count = int.Parse(currentLine);

                    for (int i = 0; i < count; i++)
                    {
                        currentLine = reader.ReadLine();
                        if (result.Get(currentLine) == null)
                        {
                            notfound++;
                        }
                        else
                        {
                            found++;
                        }
                    }

                    Console.WriteLine("{0}, {1}", found, notfound);
                }
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Assert.IsTrue(sw.Elapsed < TimeSpan.FromSeconds(10), "Не уложились по времени выполнения");

            Assert.AreEqual(9379, found, "Не совпало количество найденных слов");
            Assert.AreEqual(5621, notfound, "Не совпало количество ненайденных слов");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TrieLoaderTest_NullStream()
        {
            await TrieLoader.LoadAsync((StreamReader)null).ConfigureAwait(false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TrieLoaderTest_NullString()
        {
            await TrieLoader.LoadAsync((string)null).ConfigureAwait(false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TrieLoaderTest_FakePath()
        {
            await TrieLoader.LoadAsync("E:\\fake\\path\\file.in").ConfigureAwait(false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TrieLoaderTest_EndStream()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    reader.ReadToEnd();
                    await TrieLoader.LoadAsync(reader).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task TrieLoaderTest_IncorrectFirstRow()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("Error");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    await TrieLoader.LoadAsync(reader).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task TrieLoaderTest_IncorrectWordRow()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("5");
                writer.WriteLine("incorrect");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    await TrieLoader.LoadAsync(reader).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task TrieLoaderTest_TooLessLines()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("5");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    await TrieLoader.LoadAsync(reader).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task TrieLoaderTest_TooMuchWordsCount()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("100001");
                writer.WriteLine("test 10");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    await TrieLoader.LoadAsync(reader).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task TrieLoaderTest_TooLessWordsCount()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("0");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    await TrieLoader.LoadAsync(reader).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task TrieLoaderTest_TooBigOccurrence()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("1");
                writer.WriteLine("test 1000001");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    await TrieLoader.LoadAsync(reader).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task TrieLoaderTest_TooLessOccurrence()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("1");
                writer.WriteLine("test 0");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    await TrieLoader.LoadAsync(reader).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public async Task TrieLoaderTest_TooLongWord()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("1");
                writer.WriteLine("veryverylongword 10");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    await TrieLoader.LoadAsync(reader).ConfigureAwait(false);
                }
            }
        }
    }
}
