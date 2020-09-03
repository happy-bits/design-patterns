
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            commands.Add(new UndoCommand(editor));

            Assert.AreEqual("What does the fox says?", editor.Content);

            commands.ExecuteOne();
            Assert.AreEqual("What does the fox says?", editor.Content);

            commands.ExecuteOne();
            Assert.AreEqual("WhatWhat does the fox says?", editor.Content);

            commands.ExecuteOne();
            Assert.AreEqual("What does the fox says?", editor.Content);

        }


        // Deklarerar en metod för att utföra en kommando (normalt som här, en metod utan parameterar)
        public interface ICommand
        {
            void Execute();
        }

        class CopyCommand : ICommand
        {
            private readonly Editor _editor;
            private readonly int _fromIndex;
            private readonly int _length;

            public CopyCommand(Editor editor, int startIndex, int length)
            {
                _editor = editor;
                _fromIndex = startIndex;
                _length = length;
            }

            public void Execute()
            {
                _editor.Copy(_fromIndex, _length);
            }
        }

        class PasteCommand : ICommand
        {

            private readonly int _startIndex;
            private readonly Editor _editor;

            public PasteCommand(Editor editor, int startIndex)
            {
                _startIndex = startIndex;
                _editor = editor;
            }

            public void Execute()
            {
                _editor.Paste(_startIndex);
            }
        }


        class UndoCommand : ICommand
        {
            private readonly Editor _editor;

            public UndoCommand(Editor editor)
            {
                _editor = editor;
            }

            public void Execute()
            {
                _editor.Undo();
            }
        }

        /*
        "Receiver"
         */

        class Editor
        {
            public string Content { get; private set; } = "";

            private string _copyMemory = "";
            private string _lastContent;

            public Editor(string content)
            {
                Content = content;
                _lastContent = Content;
            }

            internal void Copy(int _startIndex, int length)
            {
                _copyMemory = Content.Substring(_startIndex, length);
            }

            internal void Paste(int startIndex)
            {
                _lastContent = Content;
                Content = Content.Insert(startIndex, _copyMemory);
            }

            internal void Undo()
            {
                Content = _lastContent;
            }
        }

        class Commands
        {
            private List<ICommand> _commands = new List<ICommand>();

            private int _nextCommandIndex = 0;

            public void Add(ICommand command)
            {
                _commands.Add(command);
            }

            public void ExecuteOne()
            {
                _commands[_nextCommandIndex].Execute();
                _nextCommandIndex++;
            }
        }

        
    }
}
