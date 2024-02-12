namespace ExchangeRatesApp.Client.Helpers
{
    public interface IExceptionHandler
    {
        void HandleHttpRequestException(HttpRequestException ex);
        void HandleDeserializationError(Exception ex);
        void HandleOtherException(Exception ex);
    }
}
