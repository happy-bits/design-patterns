using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Buttons.After
{
    class Client : IClient
    {
        static readonly List<string> _events = new List<string>();

        public IEnumerable<string> DoStuff()
        {
            var factory = new WindowsButtonFactory();
            var dialog = new Dialog();

            factory.CreateCloseButtonFor(dialog);

            factory.Click();

            return _events;
        }

        abstract class ButtonFactory
        {
            private Button _closeButton;
            private Dialog _dialogToClose;

            public void CreateCloseButtonFor(Dialog dialogToClose)
            {
                _closeButton = CreateButton(); // Anrop av factory method
                _closeButton.Render();
                _dialogToClose = dialogToClose;
            }

            public void Click()
            {
                _events.Add("Button clicked");
                _dialogToClose.Hide();
                _closeButton.Hide();
            }

            // Factory method
            protected abstract Button CreateButton();
        }

        class WindowsButtonFactory : ButtonFactory
        {
            protected override Button CreateButton()
            {
                return new WindowsButton();
            }
        }

        class WebButtonFactory : ButtonFactory
        {
            protected override Button CreateButton()
            {
                return new WebButton();
            }
        }

        class Dialog
        {
            public void Hide()
            {
                _events.Add("Hide Dialog");
            }
        }

        abstract class Button
        {
            public abstract void Render();

            public void Hide()
            {
                _events.Add("Hide Button");
            }
        }

        class WindowsButton : Button
        {
            public override void Render()
            {
                _events.Add("Render Windows Button");
            }
        }

        class WebButton : Button
        {
            public override void Render()
            {
                _events.Add("Render Web Button");
            }
        }
    }

}