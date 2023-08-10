using Dictionary.Application.Models.Dictionaries;
using Dictionary.Contacts.Responses;

namespace Dictionary.API.Mapping;

public static class ContractMapping
{
    public static List<WordResponse> MapToWordResponse(this IEnumerable<WordOut> response)
    {
        return response.Select(x => new WordResponse
        {
            Value = x.Value,
            LanguagePart = x.LanguagePart,
            PossibleTranslations = x.PossibleTranslations?.Select(p => new PossibleTranslationResponse
            {
                Translation = p.Translation,
                Explanation = p.Explanation,
                Example = p.Examples?.Select(e => e.Value).ToList()
            }).ToList(),
            RelatedWords = x.RelatedWords?.ToList(),
            RelatedFromWords = x.RelatedFromWords?.ToList()
        }).ToList();
    }
}