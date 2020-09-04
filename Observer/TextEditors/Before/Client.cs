
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Observer.TextEditors.Before
{
    class Client : IClient
    {
        public static Queue<string> _events = new Queue<string>();

        public void DoStuff()
        {
            var wordCounter = new WordCounter();
            var spellChecker = new SpellChecker();

            var editor = new TextEditor(wordCounter, spellChecker);

            Assert.IsTrue(_events.Count == 0); // Nothing happened

            editor.Text = "what";

            Assert.IsTrue(_events.Count == 0); // Nothing happened

            editor.WordCounterIsActive = true;
            editor.SpellCheckerIsActive = true;

            editor.Text = "what";

            Assert.IsTrue(_events.Count == 0); // Nothing happened

            editor.Text = "what does";

            Assert.AreEqual("Update gui: Wordcount=2", _events.Dequeue());

            editor.Text = "what does the fox says";

            Assert.AreEqual("Update gui: Wordcount=5", _events.Dequeue());

            editor.Text = "whatt does the foxxxxx says";

            Assert.AreEqual("Update gui: Number of incorrect words=2", _events.Dequeue());

            editor.WordCounterIsActive = false;
            editor.SpellCheckerIsActive = false;

            editor.Text = "yyyyyyyyyyy";

            Assert.IsTrue(_events.Count == 0); // Nothing happened
        }

        // Nackdel: denna kod är redan stor och ganska komplex. Måste hålla reda på många variabler
        // Nackdel: denna klass kommer växa när nya lyssnare behövs
        class TextEditor
        {
            private readonly WordCounter _wordCounter;
            private readonly SpellChecker _spellChecker;
            private string _lastText = "";
            private string _text = "";

            public bool WordCounterIsActive { get; set; } = false;
            public bool SpellCheckerIsActive { get; set; } = false;

            public TextEditor(WordCounter wordCounter, SpellChecker spellChecker)
            {
                _wordCounter = wordCounter;
                _spellChecker = spellChecker;
            }
            public string Text
            {
                get => _text;
                set
                {
                    if (value != _lastText)
                    {
                        if (WordCounterIsActive)
                            _wordCounter.HandleTextChanged(value);

                        if (SpellCheckerIsActive)
                            _spellChecker.HandleTextChanged(value);

                        _text = value;
                        _lastText = value;
                    }
                }
            }

        }

        class WordCounter 
        {
            private int _lastCount = -1;

            public void HandleTextChanged(string text)
            {
                int nrOfWords = text.Split(" ").Count();

                if (nrOfWords != _lastCount)
                {
                    _events.Enqueue($"Update gui: Wordcount={nrOfWords}");
                    _lastCount = nrOfWords;
                }
            }
        }

        class SpellChecker 
        {

            HashSet<string> _allWords = new HashSet<string> { "what", "does", "the", "fox", "says" };

            public void HandleTextChanged(string text)
            {
                int nrOfIncorrectWords = text.Split(" ").Count(word => !_allWords.Contains(word));

                if (nrOfIncorrectWords > 0)
                    _events.Enqueue($"Update gui: Number of incorrect words={nrOfIncorrectWords}");

            }
        }
    }
}
