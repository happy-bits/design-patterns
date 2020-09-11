
namespace DesignPatterns.Decorator.Weathers
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
