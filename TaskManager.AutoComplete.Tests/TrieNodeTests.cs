using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace TaskManager.AutoComplete.Tests
{
    [TestClass]
    public class TrieNodeTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            TrieNode node = new TrieNode();

            Assert.IsNotNull(node, "node must have a value");
            Assert.IsNotNull(node.Leaves, "node.Childs must have a value");
            Assert.IsNotNull(node.Words, "node.Words must have a value ");
        }

        [TestMethod]
        public void AddWordTest()
        {
            TrieNode node = new TrieNode();
            node.AddWord("test", 100);

            Assert.AreEqual(1, node.Words.Count(), "Incorrect words count");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddWordTest_NullWord()
        {
            TrieNode node = new TrieNode();
            node.AddWord(null, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddWordTest_Zero()
        {
            TrieNode node = new TrieNode();
            node.AddWord("Test", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddWordTest_LessZero()
        {
            TrieNode node = new TrieNode();
            node.AddWord("Test", -100);
        }

        [TestMethod]
        public void AddWordTest_WordsLimit()
        {
            TrieNode node = new TrieNode();

            for (int i = 1; i < 12; i++)
            {
                node.AddWord("test" + i, i);
            }

            Assert.AreEqual(10, node.Words.Count(), "Incorrect words count");
        }
    }
}
