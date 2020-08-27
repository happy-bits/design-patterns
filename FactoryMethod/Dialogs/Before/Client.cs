
using System;
using System.Collections.Generic;

namespace DesignPatterns.Template.Dialogs.Before
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

        abstract class Dialog
        {
            public Button OkButton;

            public bool IsVisible { get; private set; }

            protected void CloseDialog()
            {
                IsVisible = false;
                _events.Add("Dialog closed");
            }

            abstract public void Render();
        }

        class WindowsDialog : Dialog
        {
            // Nackdel: upprepning av kod
            public override void Render()
            {
                OkButton = new WindowsButton();

                var renderResult = OkButton.Render();
                _events.Add($"Ok button rendered as: {renderResult}");

                OkButton.OnClick = CloseDialog;
            }

        }

        class WebDialog : Dialog
        {
            public override void Render()
            {
                OkButton = new WebButton();

                var renderResult = OkButton.Render();
                _events.Add($"Ok button rendered as: {renderResult}");

                OkButton.OnClick = CloseDialog;
            }
        }

        abstract class Button
        {
            public Action OnClick { get; set; }

            public abstract string Render();
        }

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