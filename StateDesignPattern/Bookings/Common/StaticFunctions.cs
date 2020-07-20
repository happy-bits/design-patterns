using DesignPatterns.StateDesignPattern.Bookings.Pattern;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatterns.StateDesignPattern.Bookings.Common
{

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
