
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DesignPatterns.Template.Xxx.After
{
    class Client : IClient
    {
        public void DoStuff()
        {
            var text1 = new TextField("^\\d{3}$");
            var text2 = new TextField("^[a-z]{3}$");
            var clear = new ClearButton();
            var button = new SubmitButton();
            new Dialog(text1, text2, clear, button);

            text1.Value = "123";
            text2.Value = "abc";

            button.Click();
        }
    }

    public interface IMediator
    {
        void Notify(object sender, string ev);
    }

    /*
     "Concrete Mediator"

     Koordinerar ett par komponenter
    */
    class Dialog : IMediator
    {
        private readonly TextField _text1;
        private readonly TextField _text2;
        private readonly ClearButton _clear;
        private readonly SubmitButton _button;
        private readonly IEnumerable<BaseComponent> _allComponents;

        public Dialog(TextField text1, TextField text2, ClearButton clear, SubmitButton button)
        {
            _text1 = text1;
            _text1.SetMediator(this);

            _text2 = text2;
            _text2.SetMediator(this);

            _clear = clear;
            _clear.SetMediator(this);

            _button = button;
            _button.SetMediator(this);

            _allComponents = new BaseComponent[] { text1, text2, clear, button };
        }

        public void Notify(object sender, string ev)
        {

            if (sender is SubmitButton && ev == "Click")
            {
                if (AllFieldsAreValid)
                {
                    Console.WriteLine("Submit form");
                    Console.WriteLine($"text1={_text1.Value} text2={_text2.Value}");
                }
                else
                {
                    Console.WriteLine("Try submit form, but not all fields are valid");
                }
            }

            if (sender is ClearButton && ev == "Click")
            {
                ClearForm();
            }

        }

        private void ClearForm()
        {
            _allComponents.ToList().ForEach(c => c.Clear());
        }

        private bool AllFieldsAreValid => _allComponents.All(c => c.IsValid);
    }

    /*
     "Base component" sparar instansen av en mediator
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
    "Konkreta komponenter"

    Alla komponenter avslutar med att anropa "mediatorn" (medlare) och skicka med sig själv och info vad som hänt 

    Komponenten behöver inte vara beroende av en konkret mediator.

    Komponenten är inte beroende av andra komponenter
        */
    class SubmitButton : BaseComponent
    {
        public void Click()
        {
            Console.WriteLine("Click on SubmitButton");

            _mediator.Notify(this, "Click");
        }
    }

    class ClearButton : BaseComponent
    {
        public void Click()
        {
            Console.WriteLine("Click on ClearButton");

            _mediator.Notify(this, "Click");
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
