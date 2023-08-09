using Dictionary.Application.Models.Dictionaries;
using Dictionary.Contacts.Responses;

namespace Dictionary.API.Mapping;

public static class ContractMapping
{
    public static WordResponse MapToWordResponse(this WordOut response)
    {
        return new WordResponse
        {
            Value = response.Value,
            PossibleTranslations = response.PossibleTranslations?.Select(x => new PossibleTranslationResponse
            {
                Translation = x.Translation,
                Explanation = x.Explanation,
                Example = x.Example
            }).ToList(),
            RelatedWords = response.RelatedWords?.ToList(),
            RelatedFromWords = response.RelatedFromWords?.ToList()
        };
    }
}