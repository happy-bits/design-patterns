
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Observer.TextEditors.After
{
    class Client : IClient
    {
        public static List<string> _events = new List<string>();

        public void DoStuff()
        {
            var editor = new TextEditor();

            CollectionAssert.AreEqual(Array.Empty<string>(), _events); // Nothing happened

            editor.Text = "what";

            CollectionAssert.AreEqual(Array.Empty<string>(), _events); // Nothing happened

            var wordCounter = new WordCounter();
            var spellChecker = new SpellChecker();

            editor.EventManager.Subscribe(wordCounter);
            editor.EventManager.Subscribe(spellChecker);

            editor.Text = "what";

            CollectionAssert.AreEqual(Array.Empty<string>(), _events); // Nothing happened

            editor.Text = "what does";

            CollectionAssert.AreEqual(new string[] {
                "Update gui: Wordcount=2"
            }, _events);


            editor.Text = "what does the fox says";

            CollectionAssert.AreEqual(new string[] {
                "Update gui: Wordcount=2",
                "Update gui: Wordcount=5",
            }, _events);

            editor.Text = "whatt does the foxxxxx says";

            CollectionAssert.AreEqual(new string[] {
                "Update gui: Wordcount=2",
                "Update gui: Wordcount=5",
                "Update gui: Number of incorrect words=2"
            }, _events);


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
        }

        class WordCounter : IListener
        {
            private int _lastCount = -1;

            public void HandleTextChanged(string text)
            {
                int nrOfWords = text.Split(" ").Count();

                if (nrOfWords != _lastCount)
                {
                    _events.Add($"Update gui: Wordcount={nrOfWords}");
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
                    _events.Add($"Update gui: Number of incorrect words={nrOfIncorrectWords}");

            }
        }
    }
}
