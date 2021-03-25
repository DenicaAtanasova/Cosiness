namespace Cosiness.Servies.Data.Tests.Common
{
    public static class ErrorMessage
    {
        private static string incorrectIdMessage = "{0} - Incorrect id!";
        private static string invalidParameterMessage = "{0} - Parameter cannot be null or empty!";
        private static string emptyCollectionMessage = "{0} - Collection is empty!";

        public static string GetIncorrectIdMessage(string service)
            => string.Format(incorrectIdMessage, service);

        public static string GetInvalidParameterMessage(string service)
            => string.Format(invalidParameterMessage, service);

        public static string GetEmptyCollectionMessage(string service)
            => string.Format(emptyCollectionMessage, service);
    }
}