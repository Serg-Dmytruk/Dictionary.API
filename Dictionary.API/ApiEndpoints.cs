namespace Dictionary.API;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Words
    {
        private const string Base = $"{ApiBase}/words";
        public const string Get = Base;
    }
}