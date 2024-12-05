using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ClientTest
{

    [Serializable]
    public struct SexTest
    {

        public int Index { get; set; }

        public string Jopa { get; set; }
    }



    public class Program
    {

        static void Main(string[] args)
        {
            
            Console.WriteLine("Before post");


            PostAsync().Wait();

            Console.WriteLine("After post");
		}


        static async Task PostAsync()
        {

            HttpClient client = new HttpClient();

  

            SexTest sexTest = new SexTest
            {
                Index = 228,

                Jopa = "ASSSSS"
            };


            HttpResponseMessage message = await 
                
                client.PostAsJsonAsync("http://127.0.0.1:8888/connection/", sexTest);


            string sex = await message.Content.ReadAsStringAsync();


            Console.WriteLine(sex);
        }
    }
}
