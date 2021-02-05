using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDGToolkitCLI
{
    public static class ResponseFormatter
    {
        public static async Task RespondWithCollectionAsync(string initialMessage, IEnumerable<string> collection)
        {
            await Console.Out.WriteLineAsync(initialMessage);
            foreach (var element in collection)
            {
                await Console.Out.WriteLineAsync($"- {element}");
            }
        }
    }
}