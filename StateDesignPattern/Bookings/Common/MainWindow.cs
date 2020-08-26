using System;
using System.Collections.Generic;

namespace DesignPatterns.StateDesignPattern.Bookings.Common
{
    public class MainWindow
    {
        public Queue<string> Debug { get; private set; } = new Queue<string>();

        public void ShowPage(string message)
        {
            Debug.Enqueue(message);
        }

        public void ShowError(string message)
        {
            Debug.Enqueue(message);
        }

    }
}
