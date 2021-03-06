﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DesignPatterns.Mediator.Forms.After
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

    public interface IMediator
    {
        void Notify(object ev);
    }


    class Dialog : IMediator
    {
        public TextField Text1 { get; }
        public TextField Text2 { get; }
        public ClearButton ClearButton { get; }
        public SubmitButton SubmitButton { get; }
        private IEnumerable<BaseComponent> _allComponents;

        public List<string> Events { get; } = new List<string>();

        public Dialog(TextField text1, TextField text2, ClearButton clear, SubmitButton button)
        {
            Text1 = text1;
            Text2 = text2;
            ClearButton = clear;
            SubmitButton = button;

            _allComponents = new BaseComponent[] { text1, text2, clear, button };

            // Alla komponenter vet att de hör till denna dialogen

            _allComponents.ToList().ForEach(c => c.SetMediator(this));
        }

        // Nackdel: denna metod kommer växa när fler händelser läggs till (och klassen förvandlas till en "God class")
        // Nackdel: "ev" är inte hårt typat

        public void Notify(object ev)
        {
            switch (ev)
            {
                case "SubmitButtonClicked":
                    if (AllFieldsAreValid)
                    {
                        Events.Add("Form submitted");
                        Events.Add($"text1={Text1.Value} text2={Text2.Value}");
                    }
                    else
                    {
                        Events.Add("Tried submit form, but not all fields are valid");
                    }
                    break;

                case "ClearButtonClicked":
                    ClearForm();
                    Events.Add("Form cleared");
                    Events.Add($"text1={Text1.Value} text2={Text2.Value}");
                    break;
                default: throw new Exception();
            }
        }

        private void ClearForm()
        {
            _allComponents.ToList().ForEach(c => c.Clear());
        }

        private bool AllFieldsAreValid => _allComponents.All(c => c.IsValid);
    }

    /*
     Fördel: Komponterna är inte beroende av varandr utan bara av "mediator"
    */
    class BaseComponent
    {
        protected IMediator _mediator;

        public BaseComponent(IMediator mediator = null)
        {
            _mediator = mediator;
        }

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public virtual bool IsValid { get; } = true;

        public virtual void Clear()
        {
        }

    }

    /*
    Fördel: Dessa klasser blir enkla och kan lätt återanvändas 
    */
    class SubmitButton : BaseComponent
    {
        public void Click()
        {
            _mediator.Notify("SubmitButtonClicked");
        }
    }

    class ClearButton : BaseComponent
    {
        public void Click()
        {
            _mediator.Notify("ClearButtonClicked");
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
