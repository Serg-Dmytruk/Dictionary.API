using Dictionary.Application.Services.ParseServices.Layers;
using Microsoft.Extensions.Logging;

namespace Dictionary.Application.Services.ParseServices.Factories;

public static class ParserFactory
{
    public static PageParser CreatePageParser(int layer, string baseUrl, ILogger logger)
    {
        return layer switch
        {
            1 => new Layer1PageParser(layer, baseUrl, logger),
            2 => new Layer2PageParser(layer, baseUrl, logger),
            3 => new Layer3PageParser(layer, baseUrl, logger),
            4 => new Layer4PageParser(layer, baseUrl, logger),
            _ => throw new ArgumentException("Invalid level"),
        };
    }
}