// Factory method

using System;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Documents.After
{
    class Client
    {

        //public List<string> Run(IEnumerable<Document> documents )
        //{
        //    var result = new List<string>();

        //    foreach (Document document in documents)
        //    {
        //        result.Add(document.GetType().Name);
        //        foreach (Page page in document.Pages)
        //        {
        //            result.Add("---" + page.GetType().Name);
        //        }
        //    }

        //    return result;
        //}

        internal Document CreateResume()
        {
            return new Resume();
        }

        // Följande metod behöver inte veta vilka typer av dokument det finns (ex Resume)

        internal List<string> RenderDocument(Document document)
        {
            var result = new List<string>();

            result.Add(document.GetType().Name);
            foreach (Page page in document.Pages)
            {
                result.Add("---" + page.GetType().Name);
            }

            return result;
        }
    }


    /// The 'Creator' abstract class

    abstract class Document
    {
        public Document()
        {
            CreatePages();

            // Möjligt utveckling: att basklassen anropar fler factory methods här
        }

        public List<Page> Pages { get; } = new List<Page>();

        // Factory Method

        public abstract void CreatePages();
    }

    // Basklassen tvingar oss att skapa CreatePages och den anropas automatiskt (bra)

    /// A 'ConcreteCreator' class

    class Resume : Document
    {
        // Factory Method implementation

        public override void CreatePages()
        {
            Pages.Add(new SkillsPage());
            Pages.Add(new EducationPage());
            Pages.Add(new ExperiencePage());
        }
    }

    /// A 'ConcreteCreator' class

    class Report : Document
    {
        // Factory Method implementation

        public override void CreatePages()
        {
            Pages.Add(new IntroductionPage());
            Pages.Add(new ResultsPage());
            Pages.Add(new ConclusionPage());
            Pages.Add(new SummaryPage());
            Pages.Add(new BibliographyPage());
        }
    }
}


