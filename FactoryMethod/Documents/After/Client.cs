// Factory method

using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Documents.After
{
    class Client : IClient
    {

        public List<string> CreateResumeAndRenderDocument()
        {
            var resume = new Resume();
            var result = RenderDocument(resume);
            return result;
        }

        public List<string> CreateReportAndRenderDocument()
        {
            var report = new Report();
            var result = RenderDocument(report);
            return result;
        }

        private static List<string> RenderDocument(Document resume)
        {
            var result = new List<string>();

            result.Add(resume.GetType().Name);
            foreach (Page page in resume.Pages)
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


