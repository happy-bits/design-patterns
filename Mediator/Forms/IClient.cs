using System.Collections.Generic;

namespace DesignPatterns.Mediator.Forms
{
    interface IClient
    {
        IEnumerable<string> EnterTextAndClickSubmit(string text1Value, string text2Value);
        IEnumerable<string> EnterTextAndClearForm(string v1, string v2);
    }
}