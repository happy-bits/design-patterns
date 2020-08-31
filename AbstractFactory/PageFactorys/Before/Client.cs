using System;
using System.Linq;

namespace DesignPatterns.AbstractFactory.PageFactorys.Before
{
    class Client : IClient
    {
        enum PageFactory 
        { 
            Round, Html 
        }

        PageFactory _factory;

        public void SetFactory(string factoryname)
        {
            _factory = factoryname switch
            {
                "Round" => PageFactory.Round,
                "Html" => PageFactory.Html,
                _ => throw new Exception()
            };
        }

        public string[] RenderPage(string headerText, string introText)
        {

            // Nackdel: komplex kod (lätt att råka skriva Round istället för Html)
            // Nackdel: om det tillkommer en till sort (ex RoundButton) så behövs kod uppdateras på många ställen
            Header header = _factory switch
            {
                PageFactory.Round => new RoundHeader(headerText),
                PageFactory.Html => new HtmlHeader(headerText),
                _ => throw new Exception()
            };

            // Nackdel: komplex kod
            Intro intro = _factory switch
            {
                PageFactory.Round => new RoundIntro(introText),
                PageFactory.Html => new HtmlIntro(introText),
                _ => throw new Exception()
            };

            string[] a = header.Render();
            string[] b = intro.Render();

            return a.ToList().Concat(b).ToArray();
        }

    }
}
