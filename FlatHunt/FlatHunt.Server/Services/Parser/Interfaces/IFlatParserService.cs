using FlatHunt.Server.Models;
using FlatHunt.Server.Services.Parser.Models;

namespace FlatHunt.Server.Services.Parser.Interfaces
{
    public interface IFlatParserService
    {
        Task<List<Advertisement>> ParseFlats(FlatParseFilter filter);
    }

}
