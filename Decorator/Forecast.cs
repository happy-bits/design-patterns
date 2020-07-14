
namespace DesignPatterns.Decorator
{
    class Forecast
    {
        public Forecast(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
