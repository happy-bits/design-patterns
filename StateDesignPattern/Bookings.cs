using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatterns.StateDesignPattern
{
    [TestClass]
    public class Bookings
    {
        [TestMethod]
        public void state_should_be_new_at_start()
        {
            var view = new MainWindow();
            var context = new BookingContext(view);

            Assert.AreEqual("New booking", view.Debug.Dequeue());
            Assert.AreEqual("New.TicketCount=0.Attendee=.", context.Debug.Dequeue());

            AssertQueuesAreEmpty(view, context);

        }

        private void AssertQueuesAreEmpty(MainWindow view, BookingContext context)
        {
            Assert.AreEqual(0, view.Debug.Count);
            Assert.AreEqual(0, context.Debug.Count);
        }

        [TestMethod]
        public void successful_booking()
        {
            var view = new MainWindow();
            var context = new BookingContext(view);

            Assert.AreEqual("New booking", view.Debug.Dequeue());
            Assert.AreEqual("New.TicketCount=0.Attendee=.", context.Debug.Dequeue());

            context.SubmitDetails("Oscar", 5);

            Task.Delay(500).Wait();

            Assert.AreEqual("Processing Booking", view.Debug.Dequeue());
            Assert.AreEqual("Pending.TicketCount=5.Attendee=Oscar.", context.Debug.Dequeue());

            Assert.AreEqual("Enjoy the Event", view.Debug.Dequeue());
            Assert.AreEqual("Booked.TicketCount=5.Attendee=Oscar.", context.Debug.Dequeue());

            AssertQueuesAreEmpty(view, context);

        }

        [TestMethod]
        public void failed_booking()
        {
            var view = new MainWindow();
            var context = new BookingContext(view);

            var b1 = context.BookingID;

            Assert.AreEqual("New booking", view.Debug.Dequeue());
            Assert.AreEqual("New.TicketCount=0.Attendee=.", context.Debug.Dequeue());

            context.SubmitDetails("Oscar", 666);

            Task.Delay(500).Wait();

            Assert.AreEqual("Processing Booking", view.Debug.Dequeue());
            Assert.AreEqual("Pending.TicketCount=666.Attendee=Oscar.", context.Debug.Dequeue());

            // Ett fel uppstod
            Assert.AreEqual("Error processing booking", view.Debug.Dequeue());

            Assert.AreEqual("New booking", view.Debug.Dequeue());
            Assert.AreEqual("New.TicketCount=666.Attendee=Oscar.", context.Debug.Dequeue());

            var b2 = context.BookingID;

            Assert.AreNotEqual(b1, b2); // vi ska ha fått ett nytt bookingid

            AssertQueuesAreEmpty(view, context);

        }

        public class MainWindow
        {
            public Queue<string> Debug { get; private set; } = new Queue<string>();

            internal void ShowEntryPage()
            {
                Debug.Enqueue("New booking");
            }

            internal void ShowStatusPage(string message)
            {
                Debug.Enqueue($"{message}");
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

        public abstract class BookingState
        {
            public abstract void EnterState(BookingContext booking);
            public abstract void Cancel(BookingContext booking);
            public abstract void DatePassed(BookingContext booking);
            public abstract void EnterDetails(BookingContext booking, string attendee, int ticketCount);
        }

        class NewState : BookingState
        {
            public override void Cancel(BookingContext booking)
            {
                booking.TransitionToState(new ClosedState("Booking Canceled"));
            }

            public override void DatePassed(BookingContext booking)
            {
                booking.TransitionToState(new ClosedState("Booking Expired"));
            }

            public override void EnterDetails(BookingContext booking, string attendee, int ticketCount)
            {
                booking.Attendee = attendee;
                booking.TicketCount = ticketCount;
                booking.TransitionToState(new PendingState());
            }

            public override void EnterState(BookingContext booking)
            {
                booking.BookingID = Guid.NewGuid();
                booking.ShowState("New");
                booking.View.ShowEntryPage();
            }
        }

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


        class BookedState : BookingState
        {
            public override void Cancel(BookingContext booking)
            {
                booking.TransitionToState(new ClosedState("Booking canceled: Expect a refund"));
            }

            public override void DatePassed(BookingContext booking)
            {
                booking.TransitionToState(new ClosedState("We hope you enjoyed the event!"));
            }

            public override void EnterDetails(BookingContext booking, string attendee, int ticketCount)
            {

            }

            public override void EnterState(BookingContext booking)
            {
                booking.ShowState("Booked");
                booking.View.ShowStatusPage("Enjoy the Event");
            }
        }

        class ClosedState : BookingState
        {
            private string reasonClosed;

            public ClosedState(string reason)
            {
                reasonClosed = reason;
            }

            public override void Cancel(BookingContext booking)
            {
                booking.View.ShowError("Invalid action for this state", "Closed Booking Error");
            }

            public override void DatePassed(BookingContext booking)
            {
                booking.View.ShowError("Invalid action for this state", "Closed Booking Error");
            }

            public override void EnterDetails(BookingContext booking, string attendee, int ticketCount)
            {
                booking.View.ShowError("Invalid action for this state", "Closed Booking Error");
            }

            public override void EnterState(BookingContext booking)
            {
                booking.ShowState("Closed");
                booking.View.ShowStatusPage(reasonClosed);
            }
        }

        public enum ProcessingResult { Success, Fail, Cancel }

        public static class StaticFunctions
        {
            public static async void ProcessBooking(BookingContext booking, Action<BookingContext, ProcessingResult> callback, CancellationTokenSource token)
            {
                try
                {
                    await Task.Run(async delegate
                    {
                        await Task.Delay(200, token.Token);
                    });
                }
                catch (OperationCanceledException)
                {
                    callback(booking, ProcessingResult.Cancel);
                    return;
                }

                ProcessingResult result = ProcessingResult.Success;

                // Simulera ett fel
                if (booking.TicketCount == 666)
                    result = ProcessingResult.Fail;

                callback(booking, result);
            }


        }
    }
}




//public static class StaticFunctions
//{
//    public static async void ProcessBooking(BookingContext booking, Action<BookingContext, ProcessingResult> callback, CancellationTokenSource token)
//    {
//        try
//        {
//            await Task.Run(async delegate
//            {
//                await Task.Delay(new TimeSpan(0, 0, 3), token.Token);
//            });
//        }
//        catch (OperationCanceledException)
//        {
//            callback(booking, ProcessingResult.Cancel);
//            return;
//        }

//        //ProcessingResult result = new Random().Next(0, 2) == 0 ? ProcessingResult.Success : ProcessingResult.Fail;
//        // todo: fixa
//        ProcessingResult result = ProcessingResult.Success;

//        callback(booking, result);
//    }


//}