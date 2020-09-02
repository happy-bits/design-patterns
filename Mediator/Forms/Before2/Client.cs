
// Lite åt mediator-hållet (eftersom kommunikationen går via Dialog). Så rätt bra lösning
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DesignPatterns.Mediator.Forms.Before2
{
    class Client : IClient
    {
        private Dialog SetupDialog()
        {
            var text1 = new TextField("^\\d{3}$");
            var text2 = new TextField("^[a-z]{3}$");
            var clear = new ClearButton();
            var button = new SubmitButton();
            var dialog = new Dialog(text1, text2, clear, button);
            return dialog;
        }

        public IEnumerable<string> EnterTextAndClickSubmit(string text1Value, string text2Value)
        {
            Dialog dialog = SetupDialog();

            dialog.Text1.Value = text1Value;
            dialog.Text2.Value = text2Value;

            dialog.SubmitButton.Click();

            return dialog.Events;
        }

        public IEnumerable<string> EnterTextAndClearForm(string text1Value, string text2Value)
        {
            Dialog dialog = SetupDialog();

            dialog.Text1.Value = text1Value;
            dialog.Text2.Value = text2Value;

            dialog.ClearButton.Click();

            return dialog.Events;
        }
    }

    interface IDialog
    {
        void ClearForm();
        void SubmitForm();
    }

    class Dialog : IDialog
    {
        public TextField Text1 { get; }
        public TextField Text2 { get; }
        public ClearButton ClearButton { get; }
        public SubmitButton SubmitButton { get; }
        private IEnumerable<BaseComponent> _allComponents;

        public List<string> Events { get; } = new List<string>();

        public Dialog(TextField text1, TextField text2, ClearButton clearButton, SubmitButton submitButton)
        {
            Text1 = text1;
            Text2 = text2;
            ClearButton = clearButton;
            SubmitButton = submitButton;

            _allComponents = new BaseComponent[] { text1, text2, clearButton, submitButton };

            // Alla komponenter vet att de hör till denna dialogen

            _allComponents.ToList().ForEach(c => c.Dialog = this);
        }

        private bool AllFieldsAreValid => _allComponents.All(c => c.IsValid);

        public void ClearForm()
        {
            _allComponents.ToList().ForEach(c => c.Clear());
            Events.Add("Form cleared");
            Events.Add($"text1={Text1.Value} text2={Text2.Value}");
        }


        public void SubmitForm()
        {
            if (AllFieldsAreValid)
            {
                Events.Add("Form submitted");
                Events.Add($"text1={Text1.Value} text2={Text2.Value}");
            }
            else
            {
                Events.Add("Tried submit form, but not all fields are valid");
            }
        }
    }


    class BaseComponent
    {
        public BaseComponent()
        {
        }
        public BaseComponent(IDialog dialog)
        {
            Dialog = dialog;
        }
        public virtual bool IsValid { get; } = true;

        public virtual void Clear() { }

        public IDialog Dialog { get; set; }

    }

    class SubmitButton : BaseComponent
    {
        public void Click()
        {
            Dialog.SubmitForm();
        }
    }

    class ClearButton : BaseComponent
    {
        public void Click()
        {
            Dialog.ClearForm();
        }
    }


    class TextField : BaseComponent
    {
        private readonly string _validationRegex;

        public TextField(string validationRegex)
        {
            _validationRegex = validationRegex;
        }

        public string Value { get; set; } = "";

        public override bool IsValid => Regex.IsMatch(Value, _validationRegex);

        public override void Clear() => Value = "";
    }
}
