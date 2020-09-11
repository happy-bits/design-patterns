
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.Command.TextEditors.After
{
    class Client : IClient
    {
        public void DoStuff()
        {

            var editor = new Editor("What does the fox says?");
            var commands = new Commands();

            commands.Add(new CopyCommand(editor, 0, 4));
            commands.Add(new PasteCommand(editor, 4));

            // Nothing has happened
            Assert.AreEqual("What does the fox says?", editor.Content);

            commands.ExecuteOne();
            Assert.AreEqual("What does the fox says?", editor.Content);

            commands.ExecuteOne();
            Assert.AreEqual("WhatWhat does the fox says?", editor.Content);

        }

        // Fördel: när nya kommando dyker upp så kommer ingen kod behöva ändras. Det räcker att lägga till de nya klasserna

        abstract class Command
        {
            public abstract void Execute();
            protected Editor _editor;
        }

        class CopyCommand : Command
        {
            private readonly int _startIndex;
            private readonly int _length;

            public CopyCommand(Editor editor, int startIndex, int length)
            {
                _editor = editor;
                _startIndex = startIndex;
                _length = length;
            }

            public override void Execute()
            {
                _editor.Copy(_startIndex, _length);
            }
        }

        class PasteCommand : Command
        {

            private readonly int _startIndex;

            public PasteCommand(Editor editor, int startIndex)
            {
                _startIndex = startIndex;
                _editor = editor;
            }

            public override void Execute()
            {
                _editor.Paste(_startIndex);
            }
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

        // "Invoker"

        class Commands
        {
            private readonly Queue<Command> _commands = new Queue<Command>();

            public void Add(Command command)
            {
                _commands.Enqueue(command);
            }

            public void ExecuteOne()
            {
                _commands.Dequeue().Execute();
            }
        }
    }
}
