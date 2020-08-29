using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Menus
{
    interface IClient
    {
        string RenderAlphaMenu();
        string RenderNumericMenu();
    }
}