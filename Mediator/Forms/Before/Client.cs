using System;
using System.Collections.Generic;

namespace DesignPatterns.Mediator.Forms.Before
{
    class Client : IClient
    {
        public IEnumerable<string> EnterTextAndClearForm(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> EnterTextAndClickSubmit(string text1Value, string text2Value)
        {
            throw new NotImplementedException();
        }
    }
}
