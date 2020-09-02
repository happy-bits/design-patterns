

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DesignPatterns.Mediator.Forms.Before
{
    class Client : IClient
    {
        private (TextField, TextField, ClearButton, SubmitButton) SetupDialog()
        {
            var text1 = new TextField("^\\d{3}$");
            var text2 = new TextField("^[a-z]{3}$");
            var clearButton = new ClearButton();
            clearButton.AddFields(text1, text2);

            var submitButton = new SubmitButton();
            submitButton.AddFields(text1, text2);
            return (text1, text2, clearButton, submitButton);
        }

        public IEnumerable<string> EnterTextAndClickSubmit(string text1Value, string text2Value)
        {
            var (text1, text2, _, submit) = SetupDialog();

            text1.Value = text1Value;
            text2.Value = text2Value;

            submit.Click();

            return submit.Events;
        }

        public IEnumerable<string> EnterTextAndClearForm(string text1Value, string text2Value)
        {
            var (text1, text2, clearbutton, _) = SetupDialog();

            text1.Value = text1Value;
            text2.Value = text2Value;

            clearbutton.Click();

            return clearbutton.Events;
        }
    }

    class BaseComponent
    {
        public virtual bool IsValid { get; } = true;

        public virtual void Clear() { }

        public List<string> Events { get; set; } = new List<string>(); // just for debugging
    }

    // Nackdel: komponenten är beroende av andra komponenter. Om ett till textfält läggs till så måste denna uppdateras
    // Nackdel: svårt/omöjligt att återanvända komponenterna
    // Fördel: lite kortare kod, du behöver ingen Dialogklassm koden är nära händelserna (ex Click)
    class SubmitButton : BaseComponent
    {
        private TextField _text1;
        private TextField _text2;

        private bool AllFieldsAreValid => _text1.IsValid && _text2.IsValid;

        public void Click()
        {
            if (AllFieldsAreValid)
            {
                Events.Add("Form submitted");
                Events.Add($"text1={_text1.Value} text2={_text2.Value}");
            }
            else
            {
                Events.Add("Tried submit form, but not all fields are valid");
            }
        }

        public void AddFields(TextField text1, TextField text2)
        {
            _text1 = text1;
            _text2 = text2;
        }
    }

    class ClearButton : BaseComponent
    {
        private TextField _text1;
        private TextField _text2;

        public void Click()
        {
            _text1.Clear();
            _text2.Clear();
            Events.Add("Form cleared");
            Events.Add($"text1={_text1.Value} text2={_text2.Value}");
        }

        public void AddFields(TextField text1, TextField text2)
        {
            _text1 = text1;
            _text2 = text2;
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
