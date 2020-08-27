// Fördel: slipper upprepa Render-koden

using System;
using System.Collections.Generic;

namespace DesignPatterns.Template.Dialogs.After
{
    class Client : IClient
    {
        static readonly List<string> _events = new List<string>();

        public IEnumerable<string> DoStuff()
        {
            Dialog dialog = new WebDialog();
            dialog.Render();
            dialog.OkButton.OnClick();

            Dialog dialog2 = new WindowsDialog();
            dialog2.Render();
            dialog2.OkButton.OnClick();

            return _events;
        }

        /*
         "Creator class"

            Här finns "factory method" som returnerar en Product

            Underklasserna till Creator implementerar ofta denna metod

         */
        abstract class Dialog
        {
            public Button OkButton;

            public bool IsVisible { get; private set; }

            private void CloseDialog()
            {
                IsVisible = false;
                _events.Add("Dialog closed");
            }

            // Får även ge default implementation av "factory method" (ej i detta fall) (jag ändrade denna till "protected")
            protected abstract Button CreateButton();

            /*
             Creator är inte främst ansvarig för att skapa produkter (även om det låter så)
             Den innehåller vanligtvis nån business logik som är beroende på Product-objekten
             */
            public void Render()
            {
                // Skapa en produkt
                OkButton = CreateButton();

                var renderResult = OkButton.Render();
                _events.Add($"Ok button rendered as: {renderResult}");

                OkButton.OnClick = CloseDialog;
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
    }

}