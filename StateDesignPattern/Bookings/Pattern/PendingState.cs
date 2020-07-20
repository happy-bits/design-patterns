﻿using DesignPatterns.StateDesignPattern.Bookings.Common;
using System.Threading;

namespace DesignPatterns.StateDesignPattern.Bookings.Pattern
{
    class PendingState : BookingState
    {
        CancellationTokenSource cancelToken;

        public override void Cancel(BookingContext booking)
        {
            cancelToken.Cancel();
        }

        public override void DatePassed(BookingContext booking)
        {

        }

        public override void EnterDetails(BookingContext booking, string attendee, int ticketCount)
        {

        }

        public override void EnterState(BookingContext booking)
        {
            cancelToken = new CancellationTokenSource();

            booking.ShowState("Pending");
            booking.View.ShowStatusPage("Processing Booking");

            StaticFunctions.ProcessBooking(booking, ProcessingComplete, cancelToken);
        }

        public void ProcessingComplete(BookingContext booking, ProcessingResult result)
        {
            switch (result)
            {
                case ProcessingResult.Success:
                    booking.TransitionToState(new BookedState());
                    break;
                case ProcessingResult.Fail:
                    booking.View.ShowProcessingError();
                    booking.TransitionToState(new NewState());
                    break;
                case ProcessingResult.Cancel:
                    booking.TransitionToState(new ClosedState("Processing Canceled"));
                    break;
            }
        }
    }
}
