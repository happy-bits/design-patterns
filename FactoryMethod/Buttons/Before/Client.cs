using System.Collections.Generic;

namespace DesignPatterns.Template.Buttons.Before
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
            protected Button _closeButton;
            protected Dialog _dialogToClose;

            public void Click()
            {
                _events.Add("Button clicked");
                _dialogToClose.Hide();
                _closeButton.Hide();
            }

            public abstract void CreateCloseButtonFor(Dialog dialogToClose);

        }

        class WindowsButtonFactory : ButtonFactory
        {
            public override void CreateCloseButtonFor(Dialog dialogToClose)
            {
                _closeButton = new WindowsButton();

                // Nackdel: upprepande kod 
                // (skulle kunna göra en metod av denna men då måste man komma ihåg att anropa den i varje fabrik)
                _closeButton.Render();
                _dialogToClose = dialogToClose;
            }

        }

        class WebButtonFactory : ButtonFactory
        {
            public override void CreateCloseButtonFor(Dialog dialogToClose)
            {
                _closeButton = new WebButton();

                // Nackdel: upprepande kod
                _closeButton.Render();
                _dialogToClose = dialogToClose;
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