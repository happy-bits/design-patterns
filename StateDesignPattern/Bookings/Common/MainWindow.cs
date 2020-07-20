using System;
using System.Collections.Generic;

namespace DesignPatterns.StateDesignPattern.Bookings.Common
{
    public class MainWindow
    {
        public Queue<string> Debug { get; private set; } = new Queue<string>();

        internal void ShowEntryPage()
        {
            Debug.Enqueue("New booking");
        }

        internal void ShowStatusPage(string message)
        {
            Debug.Enqueue(message);
        }

        internal void ShowProcessingError()
        {
            Debug.Enqueue($"Error processing booking");
        }

        internal void ShowError(string v1, string v2)
        {

            throw new NotImplementedException();
        }
    }
}
