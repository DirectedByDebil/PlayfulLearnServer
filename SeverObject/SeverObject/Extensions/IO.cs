using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Extensions
{

    public static class IO
    {

        public static async Task SerializeAsync<T>(Stream stream, T obj)
        {

            using (Stream output = stream)
            {

                await JsonSerializer.SerializeAsync(output, obj);
            }
        }


        public static async Task WriteAsync(Stream stream,
            
            Encoding encoding, string line)
        {

            using (StreamWriter writer = new StreamWriter(stream, encoding))
            {

                await writer.WriteLineAsync(line);
            }
        }


        public static async Task<string> ReadLineAsync(Stream stream,
            
            Encoding encoding)
        {

            string line = "";


            using (StreamReader reader = new StreamReader(stream, encoding))
            {

                line = await reader.ReadToEndAsync();
            }


            return line;
        }
    }
}
