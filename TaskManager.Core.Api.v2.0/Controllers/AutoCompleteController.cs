using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.AutoComplete;

namespace TaskManager.Core.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/autocomplete")]
    public class AutoCompleteController : Controller
    {
        private readonly Lazy<Trie> _trie;

        public AutoCompleteController(IConfiguration configuration)
        {
            string path = configuration["AutoComplete:Path"];
            _trie = new Lazy<Trie>(TrieLoader.LoadAsync(path).Result);
        }

        [HttpGet("{word}")]
        public async Task<IEnumerable<string>> GetAutocompletes([FromRoute]string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return null;
            }

            return await _trie.Value.GetAsync(word).ConfigureAwait(false);
        }
    }
}