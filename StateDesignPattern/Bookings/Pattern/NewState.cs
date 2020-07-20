using System;

namespace DesignPatterns.StateDesignPattern.Bookings.Pattern
{
    class NewState : BookingState
    {
        // Arrangören avblåser evenemanget (eller användaren väljer att göra det)
        public override void Cancel(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("Booking Canceled"));
        }

        // Evenemanget har redan varit
        public override void DatePassed(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("Booking Expired"));
        }

        // Användaren har matat in info och klickat submit
        public override void EnterDetails(BookingContext booking, string attendee, int ticketCount)
        {
            booking.Attendee = attendee;
            booking.TicketCount = ticketCount;
            booking.TransitionToState(new PendingState());
        }

        // "EnterState" är ingången när en byter till nytt state
        public override void EnterState(BookingContext booking)
        {
            booking.BookingID = Guid.NewGuid();
            booking.ShowState("New");    // ge info om state't till användaren
            booking.View.ShowPage("New booking");
        }
    }
}
