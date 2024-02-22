using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Serilog;
using System.Net.NetworkInformation;
using static MudBlazor.CategoryTypes;

namespace CurrencyApplication.Client.Helpers
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
            throw new ExceptionHandler("Nie można zdeserializować danych");
        }

        public static void HandleNotFound()
        {
            throw new ExceptionHandler("Brak danych");
        }

        public static void HandleBadRequest(string message)
        {
            throw new ExceptionHandler("Błąd 400 Bad Request: " + message);
        }
    }
}