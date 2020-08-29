
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Xxx.Before
{
    class Client : IClient
    {

        public string RenderAlphaMenu()
        {
            var creator = new AlphaMenuCreator();
            Menu menu = creator.CreateMenu("  Banana   ", " Apple   ");
            return menu.Render();
        }

        public string RenderNumericMenu()
        {
            var creator = new NumericMenuCreator();
            Menu menu = creator.CreateMenu("  Banana   ", " Apple   ");
            return menu.Render();
        }

        class AlphaMenuCreator
        {
            public Menu CreateMenu(params string[] choices)
            {
                var menu = new AlphaMenu();

                // Nackdel: upprepning av kod (foreach)
                foreach (var choice in choices)
                    menu.Add(choice.Trim().ToUpper());

                return menu;
            }
        }

        class NumericMenuCreator
        {
            public Menu CreateMenu(params string[] choices)
            {
                var menu = new NumericMenu();

                // Nackdel: upprepning av kod (foreach)
                foreach (var choice in choices)
                    menu.Add(choice.Trim().ToUpper());

                return menu;
            }
        }

        abstract class Menu
        {
            protected List<string> _choices = new List<string>();

            public void Add(string choice)
            {
                _choices.Add(choice);
            }
            public abstract string Render();
        }

        class AlphaMenu : Menu
        {
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
