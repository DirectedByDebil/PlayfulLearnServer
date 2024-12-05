using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Server
{
    public sealed class ServerModel
    {

		public string[] Paths
		{
			get;

			private set;
		}


		public event Action<string> GotMessage;


		private const string RegistrationPath = "/register/";

		private const string LoginPath = "/login/";

		private const string KeysPath = "/getData/";


		private readonly Random _random;


		public ServerModel()
        {

			_random = new Random();


			Paths = new string[]
			{

				RegistrationPath,

				LoginPath,

				KeysPath
			};
        }


		public async Task HandleContextAsync(HttpListenerContext context)
		{

			HttpListenerRequest request = context.Request;

			HttpListenerResponse response = context.Response;


			GotMessage?.Invoke(request.RawUrl);


			switch (request.RawUrl)
			{

				case RegistrationPath:

					await Register(request, response);
					break;

				case LoginPath:
					break;


				case KeysPath:

					await GetData(response);
					break;

				case "":

					response.Abort();
					break;
			}
		}



		private async Task GetData(HttpListenerResponse response)
		{

			IdKeys keys = GenerateKeys();


			using (Stream output = response.OutputStream)
			{

				await JsonSerializer.SerializeAsync(output, keys);
			}

			GotMessage?.Invoke("Ключи записаны");
		}


		private IdKeys GenerateKeys()
		{

			IdKeys keys = new IdKeys()
			{

				Salt = new byte[256],

				DateTime = DateTime.Now.ToString()
			};


			_random.NextBytes(keys.Salt);


			return keys;
		}


		private async Task Register(HttpListenerRequest request,

			HttpListenerResponse response)
		{

			RegistrationFields data;

			using (Stream input = request.InputStream)
			{

				using (StreamReader reader = new StreamReader(input))
                {

					data = await JsonSerializer.DeserializeAsync<RegistrationFields>(input);
                }
			}

			GotMessage?.Invoke("Данные приняты для обработки");

			
			//#TODO check data

			using (Stream output = response.OutputStream)
			{

				using (StreamWriter writer = new StreamWriter(output, request.ContentEncoding))
				{

					await writer.WriteLineAsync("Registration completed)");
				}
			}
		}


	}

}
