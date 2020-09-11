/*
"Behandla kommandon som en egen grej"

Requestet förvandlas till ett eget objekt som innehåller all info om requestet. Det gör att du kan parametrisera metoder med olika request, avvakta eller kö'a utförande av requests och tillåta undo-operationer.

Istället för att GUI anropar businesslagret direkt: ett kommando skapas med info
- objektet som ska anropas
- namn på metod
- parameterar

Du har en Button klass och en Copy-knapp. Det ska gå att kopiera text även från contextmenyn eller Ctrl-C. Koden för kopierande kan inte ligga i CopyButton-klassen
 
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DesignPatterns.Command
{
    [TestClass]
    public class Guru
    {
        [TestMethod]
        public void Ex1()
        {
            Invoker invoker = new Invoker();
            invoker.SetOnStart(new SimpleCommand("Say Hi!"));
            Receiver receiver = new Receiver();
            invoker.SetOnFinish(new ComplexCommand(receiver, "Send email", "Save report"));

            // Det är först här något händer:

            invoker.DoSomethingImportant();
        }
    }

    /*
     
        Invoker: Does anybody want something done before I begin?
        SimpleCommand: See, I can do simple things like printing (Say Hi!)
        Invoker: ...doing something really important...
        Invoker: Does anybody want something done after I finish?
        ComplexCommand: Complex stuff should be done by a receiver object.
        Receiver: Working on (Send email.)
        Receiver: Also working on (Save report.)
         
    */

    // Deklarerar en metod för att utföra en kommando (normalt som här, en metod utan parameterar)
    public interface ICommand
    {
        void Execute();
    }

    // Ett enkelt command, som inte är beroende av någon receiver
    class SimpleCommand : ICommand
    {
        private readonly string _payload = string.Empty;

        public SimpleCommand(string payload)
        {
            _payload = payload;
        }

        public void Execute()
        {
            Console.WriteLine($"SimpleCommand: See, I can do simple things like printing ({_payload})");
        }
    }

    // Detta kommando delegerar operationer till en "receiver"

    class ComplexCommand : ICommand
    {
        private readonly Receiver _receiver;

        // Context data, required for launching the receiver's methods.
        private readonly string _a;

        private readonly string _b;

        // Accepeterar en eller flera receivers + context data
        public ComplexCommand(Receiver receiver, string a, string b)
        {
            _receiver = receiver;
            _a = a;
            _b = b;
        }

        // Kommandona kan delegera till andra metoder på receivern
        public void Execute()
        {
            Console.WriteLine("ComplexCommand: Complex stuff should be done by a receiver object.");
            _receiver.DoSomething(_a);
            _receiver.DoSomethingElse(_b);
        }
    }

    /*
    Vilken klass som helst kan agera receiver

    Receivern utför någon viktig affärslogik
     */
    class Receiver
    {
        public void DoSomething(string a)
        {
            Console.WriteLine($"Receiver: Working on ({a}.)");
        }

        public void DoSomethingElse(string b)
        {
            Console.WriteLine($"Receiver: Also working on ({b}.)");
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
