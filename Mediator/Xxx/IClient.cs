using System.Collections.Generic;

namespace DesignPatterns.Template.Xxx
{
    interface IClient
    {
        IEnumerable<string> EnterTextAndClickSubmit(string text1Value, string text2Value);
        IEnumerable<string> EnterTextAndClearForm(string v1, string v2);
    }
}