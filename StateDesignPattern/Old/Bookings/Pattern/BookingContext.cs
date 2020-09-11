using DesignPatterns.StateDesignPattern.Bookings.Common;
using System;
using System.Collections.Generic;

namespace DesignPatterns.StateDesignPattern.Bookings.Pattern
{
    public class BookingContext
    {
        public MainWindow View { get; private set; }
        public string Attendee { get; set; }
        public int TicketCount { get; set; }
        public Guid BookingID { get; set; }

        public Queue<string> Debug { get; private set; } = new Queue<string>();

        private BookingState currentState;

        public void TransitionToState(BookingState state)
        {
            currentState = state;
            currentState.EnterState(this);
        }

        public BookingContext(MainWindow view)
        {
            View = view;
            TransitionToState(new NewState());
        }

        public void SubmitDetails(string attendee, int ticketCount)
        {
            currentState.EnterDetails(this, attendee, ticketCount);
        }

        public void Cancel()
        {
            currentState.Cancel(this);
        }

        public void DatePassed()
        {
            currentState.DatePassed(this);
        }

        public void ShowState(string stateName)
        {
            Debug.Enqueue($"{stateName}.TicketCount={TicketCount}.Attendee={Attendee}."); // BookingID
        }

    }
}
