
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Observer.TextEditors.After
{
    class Client : IClient
    {
        public static Queue<string> _events = new Queue<string>();

        public void DoStuff()
        {
            var wordCounter = new WordCounter();
            var spellChecker = new SpellChecker();

            var editor = new TextEditor();

            Assert.IsTrue(_events.Count == 0); // Nothing happened

            editor.Text = "what";

            Assert.IsTrue(_events.Count == 0); // Nothing happened

            editor.EventManager.Subscribe(wordCounter);
            editor.EventManager.Subscribe(spellChecker);

            editor.Text = "what";

            Assert.IsTrue(_events.Count == 0); // Nothing happened

            editor.Text = "what does";

            Assert.AreEqual("Update gui: Wordcount=2", _events.Dequeue());

            editor.Text = "what does the fox says";

            Assert.AreEqual("Update gui: Wordcount=5", _events.Dequeue());

            editor.Text = "whatt does the foxxxxx says";

            Assert.AreEqual("Update gui: Number of incorrect words=2", _events.Dequeue());

            editor.EventManager.UnSubscribe(wordCounter);
            editor.EventManager.UnSubscribe(spellChecker);

            editor.Text = "yyyyyyyyyyy";

            Assert.IsTrue(_events.Count == 0); // Nothing happened


        }

        class TextEditor
        {
            public EventManager EventManager { get; set; } = new EventManager();

            private string _lastText = "";
            private string _text = "";

            public string Text
            {
                get => _text;
                set
                {
                    if (value != _lastText)
                    {
                        EventManager.TextChanged(value);
                        _text = value;
                        _lastText = value;
                    }
                }
            }

        }

        interface IListener
        {
            void HandleTextChanged(string text);
        }

        class EventManager
        {
            private List<IListener> _listeners = new List<IListener>();

            public void Subscribe(IListener listener)
            {
                _listeners.Add(listener);
            }

            internal void TextChanged(string text)
            {
                foreach (var l in _listeners)
                {
                    l.HandleTextChanged(text);
                }
            }

            internal void UnSubscribe(IListener listener)
            {
                _listeners.Remove(listener);
            }
        }

        class WordCounter : IListener
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

        class SpellChecker : IListener
        {

            HashSet<string> _allWords = new HashSet<string> { "what", "does", "the", "fox", "says" };

            public void HandleTextChanged(string text)
            {
                int nrOfIncorrectWords = text.Split(" ").Count(word => !_allWords.Contains(word));

                if (nrOfIncorrectWords>0)
                    _events.Enqueue($"Update gui: Number of incorrect words={nrOfIncorrectWords}");

            }
        }
    }
}
