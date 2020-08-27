/*
 
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Dialogs
    {

        [TestMethod]
        public void Ex1()
        {
            var client = new Client();

            //Assert.AreEqual("",  client.ClientCode(new WindowsDialog()));

            CollectionAssert.AreEqual(new[] {

                "<button>Ok</button>",
                "OnClick WebButton"

            }, client.ClientCode(new WebDialog()));

            CollectionAssert.AreEqual(new[] {

                "[Ok]",
                "OnClick WindowsButton"

            }, client.ClientCode(new WindowsDialog()));

        }

      

        class Client
        {
            /*
            Klienten jobber med en instans av en "concreate creator"
            */

            public string[] ClientCode(Dialog dialog)
            {
                // Client: I'm not aware of the creator's class,but it still works
                // Behöver inte veta vilken typ av dialog
                string[] eventz = dialog.Render();

                dialog.Close();

                return eventz;
            }
        }
    }
}
