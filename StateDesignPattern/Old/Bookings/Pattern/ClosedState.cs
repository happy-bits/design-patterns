namespace DesignPatterns.StateDesignPattern.Bookings.Pattern
{
    class ClosedState : BookingState
    {
        private string reasonClosed;

        public ClosedState(string reason)
        {
            reasonClosed = reason;
        }

        // Kan inte göra något när bokningen är Closed
        public override void Cancel(BookingContext booking)
        {
            booking.View.ShowError("Invalid action for this state");
        }

        public override void DatePassed(BookingContext booking)
        {
            booking.View.ShowError("Invalid action for this state");
        }

        public override void EnterDetails(BookingContext booking, string attendee, int ticketCount)
        {
            booking.View.ShowError("Invalid action for this state");
        }

        public override void EnterState(BookingContext booking)
        {
            booking.ShowState("Closed");
            booking.View.ShowPage(reasonClosed);
        }
    }
}
