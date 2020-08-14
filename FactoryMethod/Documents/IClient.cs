using DesignPatterns.FactoryMethod.Documents.After;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Documents
{
    interface IClient
    {
        List<string> Run(IEnumerable<Document> documents);
    }
}