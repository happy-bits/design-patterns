using System;
using System.Collections.Generic;

namespace DesignPatterns.StateDesignPattern.Bookings.Common
{
    public class MainWindow
    {
        public Queue<string> Debug { get; private set; } = new Queue<string>();

        internal void ShowPage(string message)
        {
            Debug.Enqueue(message);
        }

        internal void ShowError(string message)
        {
            Debug.Enqueue(message);
        }

    }
}
