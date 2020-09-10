using System.Collections.Generic;

namespace DesignPatterns.Iterator.Grids
{
    interface IClient
    {
        List<string> ForEachFromLeftToRight();
        List<string> ForEachFromUpToDown();
    }
}