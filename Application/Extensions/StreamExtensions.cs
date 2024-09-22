using Newtonsoft.Json;

namespace Application.Extensions
{
    public static class StreamExtentions
    {
        public async static Task<dynamic> Deserialize<T>(this Stream stream)
        {
            using var reader = new StreamReader(stream, leaveOpen: true);
            var text = await reader.ReadToEndAsync();
            stream.Position = 0; // Set position back to 0, so we can read it again later.
            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}
