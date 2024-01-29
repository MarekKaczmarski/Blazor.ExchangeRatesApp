namespace ExchangeRatesApp.Client.Helpers
{
    public class ExceptionHandler : Exception
    {
        public ExceptionHandler() : base() { }

        public ExceptionHandler(string message) : base(message) { }

        public ExceptionHandler(string message, Exception innerException) : base(message, innerException) { }

        public static void HandleEmptyTable()
        {
            throw new ExceptionHandler("Nie znaleziono tabeli");
        }

        public static void HandleDeserializationError()
        {
            throw new ExceptionHandler("Żądanie API nie powiodło się lub odpowiedź nie mogła zostać zdeserializowana");
        }

        public static void HandleNotFound()
        {
            throw new ExceptionHandler("Nie znaleziono danych");
        }

        public static void HandleBadRequest(string message)
        {
            if (message.Contains("Przekroczony limit 367 dni", StringComparison.OrdinalIgnoreCase))
            {
                throw new ExceptionHandler("Przekroczony limit 367 dni");
            }
            else
            {
                throw new ExceptionHandler("Błąd 400 Bad Request: " + message);
            }
        }
    }
}
