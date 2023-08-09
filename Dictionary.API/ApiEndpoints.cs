namespace Dictionary.API;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Dictionary
    {
        private const string Base = $"{ApiBase}/dictionaries";
        public const string Get = $"{Base}/{{request::string}}";
    }
}