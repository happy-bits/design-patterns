// Factory method

using System;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Documents.Before
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

    abstract class Document
    {
        public List<Page> Pages { get; } = new List<Page>();
    }

    // Nackdel: måste komma ihåg att skapa metoden CreatePages

    class Resume : Document
    {
        public Resume()
        {
            CreatePages();
        }

        private void CreatePages()
        {
            Pages.Add(new SkillsPage());
            Pages.Add(new EducationPage());
            Pages.Add(new ExperiencePage());
        }
    }

    /// A 'ConcreteCreator' class

    class Report : Document
    {
        public Report()
        {
            CreatePages();
        }

        private void CreatePages()
        {
            Pages.Add(new IntroductionPage());
            Pages.Add(new ResultsPage());
            Pages.Add(new ConclusionPage());
            Pages.Add(new SummaryPage());
            Pages.Add(new BibliographyPage());
        }
    }
}


