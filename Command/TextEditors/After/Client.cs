
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

            var copyFirstWord = new CopyCommand(editor, 0, 4);
            var paste = new PasteCommand(editor, 4);

            // Nothing changed
            Assert.AreEqual("What does the fox says?", editor.Content);

            copyFirstWord.Execute();

            // Nothing changed
            Assert.AreEqual("What does the fox says?", editor.Content);

            paste.Execute();

            // Text is changed
            Assert.AreEqual("WhatWhat does the fox says?", editor.Content);



            //Invoker invoker = new Invoker();
            //invoker.SetOnStart(new CopyCommand("Say Hi!"));
            //Receiver receiver = new Receiver();
            //invoker.SetOnFinish(new ComplexCommand(receiver, "Send email", "Save report"));

            //// Det är först här något händer:

            //invoker.DoSomethingImportant();
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


        /*
        Vilken klass som helst kan agera receiver

        Recevern utför någon viktig affärslogik
         */

        class Editor
        {
            public string Content { get; private set; } = "";

            private string _copyMemory = "";

            public Editor(string content)
            {
                Content = content;
            }

            internal void Copy(int _startIndex, int length)
            {
                _copyMemory = Content.Substring(_startIndex, length);
            }

            internal void Paste(int startIndex)
            {
                Content = Content.Insert(startIndex, _copyMemory);
            }
        }


        // Invokern är associerad med en eller flera kommandon. Den skickar en request till kommandot
        class Invoker
        {
            private ICommand _onStart;

            private ICommand _onFinish;

            // Skjut in kommandon, som används i DoSomethingImportant
            public void SetOnStart(ICommand command)
            {
                _onStart = command;
            }

            public void SetOnFinish(ICommand command)
            {
                _onFinish = command;
            }

            public void DoSomethingImportant()
            {
                Console.WriteLine("Invoker: Does anybody want something done before I begin?");
                if (_onStart is ICommand)
                {
                    _onStart.Execute(); // Skickar indirekt ett request till "receivern"
                }

                Console.WriteLine("Invoker: ...doing something really important...");

                Console.WriteLine("Invoker: Does anybody want something done after I finish?");
                if (_onFinish is ICommand)
                {
                    _onFinish.Execute();
                }
            }
        }
    }
}
