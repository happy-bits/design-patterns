using DesignPatterns.StateDesignPattern.Bookings.Common;
using DesignPatterns.StateDesignPattern.Bookings.Pattern;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DesignPatterns.StateDesignPattern.Bookings
{
    [TestClass]
    public class Demo1
    {

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

            Assert.AreEqual("Processing Booking", view.Debug.Dequeue());
            Assert.AreEqual("Pending.TicketCount=5.Attendee=Oscar.", context.Debug.Dequeue());

            Task.Delay(500).Wait();

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

        [TestMethod]
        public void canceled_booking()
        {
            var view = new MainWindow();
            var context = new BookingContext(view);

            Assert.AreEqual("New booking", view.Debug.Dequeue());
            Assert.AreEqual("New.TicketCount=0.Attendee=.", context.Debug.Dequeue());

            context.SubmitDetails("Oscar", 5);

            Assert.AreEqual("Processing Booking", view.Debug.Dequeue());
            Assert.AreEqual("Pending.TicketCount=5.Attendee=Oscar.", context.Debug.Dequeue());

            context.Cancel();

            Task.Delay(500).Wait();

            Assert.AreEqual("Processing Canceled", view.Debug.Dequeue());
            Assert.AreEqual("Closed.TicketCount=5.Attendee=Oscar.", context.Debug.Dequeue());

            AssertQueuesAreEmpty(view, context);

        }

        [TestMethod]
        public void date_passed()
        {
            var view = new MainWindow();
            var context = new BookingContext(view);

            Assert.AreEqual("New booking", view.Debug.Dequeue());
            Assert.AreEqual("New.TicketCount=0.Attendee=.", context.Debug.Dequeue());

            context.DatePassed();

            Assert.AreEqual("Booking Expired", view.Debug.Dequeue());
            Assert.AreEqual("Closed.TicketCount=0.Attendee=.", context.Debug.Dequeue());

            AssertQueuesAreEmpty(view, context);


        }
    }

}