
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Xxx.After
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

        abstract class MenuCreator
        {
            protected abstract Menu CreateMenu();

            public Menu CreateMenu(params string[] choices)
            {
                var menu = CreateMenu();

                foreach (var choice in choices)
                    menu.Add(choice.Trim().ToUpper());

                return menu;
            }
        }
        class AlphaMenuCreator : MenuCreator
        {
            protected override Menu CreateMenu()
            {
                return new AlphaMenu();
            }
        }

        class NumericMenuCreator : MenuCreator
        {
            protected override Menu CreateMenu()
            {
                return new NumericMenu();
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
