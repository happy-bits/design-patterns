
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Documents
{
    interface IClient
    {
        List<string> CreateReportAndRenderDocument();
        List<string> CreateResumeAndRenderDocument();
    }
}