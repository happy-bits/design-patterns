// Tyvärr är denna bättre eller lika bra som After

using System;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Menus.Before2
{
    class Client : IClient
    {

        public string RenderAlphaMenu()
        {
            var menu = new AlphaMenu("  Banana   ", " Apple   ");
            return menu.Render();
        }

        public string RenderNumericMenu()
        {
            var menu = new NumericMenu("  Banana   ", " Apple   ");
            return menu.Render();
        }


        abstract class Menu
        {
            public Menu(params string[] choices)
            {
                foreach (var choice in choices)
                    _choices.Add(choice.Trim().ToUpper());
            }

            protected List<string> _choices = new List<string>();

            public abstract string Render();

        }

        class AlphaMenu : Menu
        {
            public AlphaMenu(params string[] choices):base(choices)
            {
            }

            public override string Render()
            {
                var list = new List<string>();
                for (var index = 0; index < _choices.Count; index++)
                {
                    char firstPart = (char)('A' + index);
                    list.Add($"{firstPart}) {_choices[index]}");
                }
                return string.Join('\n', list);
            }

        }

        class NumericMenu : Menu
        {
            public NumericMenu(params string[] choices) : base(choices)
            {
            }

            public override string Render()
            {
                var list = new List<string>();
                for (var index = 0; index < _choices.Count; index++)
                {
                    int firstPart = index + 1;
                    list.Add($"{firstPart}) {_choices[index]}");
                }
                return string.Join('\n', list);
            }
        }

    }
}
