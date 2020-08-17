using DesignPatterns.FactoryMethod.Documents.After;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Documents
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new Client[] { 
            //new Before.Client(), 
            new After.Client() 
        };


        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                var result = client.CreateReportAndRenderDocument();

                CollectionAssert.AreEqual(new[] {
                    "Report",
                    "---IntroductionPage",
                    "---ResultsPage",
                    "---ConclusionPage",
                    "---SummaryPage",
                    "---BibliographyPage",
                }, result);
            }
        }

        [TestMethod]
        public void Ex2()
        {
            foreach (var client in AllClients())
            {
                var result = client.CreateResumeAndRenderDocument();

                CollectionAssert.AreEqual(new[] {
                    "Resume",
                    "---SkillsPage",
                    "---EducationPage",
                    "---ExperiencePage",
                }, result);
            }
        }
    }


    /// The 'Product' abstract class
    abstract class Page
    {
    }

    /// A 'ConcreteProduct' class
    class SkillsPage : Page
    {
    }

    /// A 'ConcreteProduct' class
    class EducationPage : Page
    {
    }

    /// A 'ConcreteProduct' class
    class ExperiencePage : Page
    {
    }

    /// A 'ConcreteProduct' class
    class IntroductionPage : Page
    {
    }

    /// A 'ConcreteProduct' class
    class ResultsPage : Page
    {
    }

    /// A 'ConcreteProduct' class
    class ConclusionPage : Page
    {
    }

    /// A 'ConcreteProduct' class
    class SummaryPage : Page
    {
    }

    /// A 'ConcreteProduct' class
    class BibliographyPage : Page
    {
    }

}
