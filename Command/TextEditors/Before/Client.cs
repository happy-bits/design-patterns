
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.Command.TextEditors.Before
{
    class Client : IClient
    {
        public void DoStuff()
        {
            var editor = new Editor("What does the fox says?");
            var commands = new Commands(editor);

            commands.Add("Copy 0 4");
            commands.Add("Paste 4"); 

            // Nothing has happened
            Assert.AreEqual("What does the fox says?", editor.Content);

            commands.ExecuteOne();
            Assert.AreEqual("What does the fox says?", editor.Content);

            commands.ExecuteOne();
            Assert.AreEqual("WhatWhat does the fox says?", editor.Content);

        }

        // "Receiver"

        class Editor
        {
            public string Content { get; private set; } = "";

            private string _copyMemory = "";

            public Editor(string content)
            {
                Content = content;
            }

            public void Copy(int _startIndex, int length)
            {
                _copyMemory = Content.Substring(_startIndex, length);
            }

            public void Paste(int startIndex)
            {
                Content = Content.Insert(startIndex, _copyMemory);
            }

        }

        // Nackdel: Denna klass kommer växa (och bli en god-class) när det tillkommer fler kommandon

        class Commands
        {
            private readonly Editor _editor;

            private Queue<string> _commands = new Queue<string>();

            // Nackdel: Vi förutsätter att det är samma editor för alla kommandon

            public Commands(Editor editor)
            {
                _editor = editor;
            }

            public void ExecuteOne()
            {
                string commandText = _commands.Dequeue();

                var commandSplitted = commandText.Split(' ');

                // Nackdel: Lätt att det blir fel i splittandet

                switch (commandSplitted[0])
                {
                    case "Copy":
                        int startIndex = int.Parse(commandSplitted[1]);
                        int length = int.Parse(commandSplitted[2]);
                        _editor.Copy(startIndex, length);
                        break;

                    case "Paste":
                        int startIndexPaste = int.Parse(commandSplitted[1]);
                        _editor.Paste(startIndexPaste);
                        break;

                    default:
                        break;
                }
            }

            // Nackdel: ingen verifiering av kommandona (det kan vara vilken sträng som helst)

            public void Add(string command)
            {
                _commands.Enqueue(command);
            }

        }


    }
}
