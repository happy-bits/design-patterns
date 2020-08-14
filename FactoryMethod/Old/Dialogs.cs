/*
 
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Dialogs
    {

        [TestMethod]
        public void Ex1()
        {
            var client = new Client();

            //Assert.AreEqual("",  client.ClientCode(new WindowsDialog()));

            CollectionAssert.AreEqual(new[] {

                "<button>Ok</button>",
                "OnClick WebButton"

            }, client.ClientCode(new WebDialog()));

            CollectionAssert.AreEqual(new[] {

                "[Ok]",
                "OnClick WindowsButton"

            }, client.ClientCode(new WindowsDialog()));

        }
        /*
         "Creator class"

            Här finns "factory method" som returnerar en Product

            Underklasserna till Creator implementerar ofta denna metod
             
         */
        abstract class Dialog
        {
            private Button _okButton;

            public bool IsVisible { get; private set; }

            private void CloseDialog()
            {
                IsVisible = false;
            }

            // Får även ge default implementation av "factory method" (ej i detta fall) (jag ändrade denna till "protected")
            protected abstract Button CreateButton();

            /*
             Creator är inte främst ansvarig för att skapa produkter (även om det låter så)
             Den innehåller vanligtvis nån business logik som är beroende på Product-objekten
             */
            public string[] Render()
            {
                // Skapa en produkt
                _okButton = CreateButton();
                
                var renderResult = _okButton.Render();

                _okButton.OnClick = CloseDialog;

                return new[] { renderResult };
            }


            internal void Close()
            {
                throw new NotImplementedException();
            }
        }

        // "Concrete Creators" 
        // Kan själv välja exakt vilken typ som ska skapas
        class WindowsDialog : Dialog
        {
            // Skapar en konkret produkt. Samtidigt är Creatorn inte beroende av någon konkret produkt (bara IProduct)
            protected override Button CreateButton()
            {
                return new WindowsButton();
            }
        }

        class WebDialog : Dialog
        {
            protected override Button CreateButton()
            {
                return new WebButton();
            }
        }

        // De operationer som alla konkreta produkter måste ha
        abstract class Button
        {
            public Action OnClick { get; set; }

            public abstract string Render();
        }

        // "Concrete Product"
        class WindowsButton : Button
        {

            public override string Render()
            {
                return "[Ok]";
            }
        }

        class WebButton : Button
        {
            public override string Render()
            {
                return "<button>Ok</button>";
            }
        }

        class Client
        {
            /*
            Klienten jobber med en instans av en "concreate creator"
            */

            public string[] ClientCode(Dialog dialog)
            {
                // Client: I'm not aware of the creator's class,but it still works
                // Behöver inte veta vilken typ av dialog
                string[] eventz = dialog.Render();

                dialog.Close();

                return eventz;
            }
        }
    }
}
