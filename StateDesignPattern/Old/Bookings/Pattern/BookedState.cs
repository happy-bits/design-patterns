namespace DesignPatterns.StateDesignPattern.Bookings.Pattern
{
    class BookedState : BookingState
    {
        // Arrangören avblåser evenemanget
        public override void Cancel(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("Booking canceled: Expect a refund"));
        }

        // Evenemanget har varit (och användaren har troligen deltagit)
        public override void DatePassed(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("We hope you enjoyed the event!"));
        }

        public override void EnterDetails(BookingContext booking, string attendee, int ticketCount)
        {

        }

        // Körs när vi hamnar på Booked
        public override void EnterState(BookingContext booking)
        {
            booking.ShowState("Booked");
            booking.View.ShowPage("Enjoy the Event");
        }
    }
}
