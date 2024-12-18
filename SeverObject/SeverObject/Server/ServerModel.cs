using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using Extensions;

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


		private const string KeysPath = "/getKeys/";

		private const string RegistrationPath = "/register/";


		private const string AuthKeysPath = "/getAuthKeys/";

		private const string AuthPath = "/auth/";



		private readonly Random _random;


		public ServerModel()
        {

			_random = new Random();

			Paths = new string[]
			{

				RegistrationPath,

				AuthPath,

				KeysPath,

				AuthKeysPath
			};
        }


		public async Task HandleContextAsync(HttpListenerContext context)
		{

			HttpListenerRequest request = context.Request;

			HttpListenerResponse response = context.Response;


			GotMessage?.Invoke(request.RawUrl);


			switch (request.RawUrl)
			{

				case KeysPath:

					await GetKeysAsync(response);
					break;


				case RegistrationPath:

					await RegisterAsync(request, response);
					break;


				case AuthKeysPath:

					await GetAuthKeysAsync(request, response);
					break;


				case AuthPath:

					await LoginAsync(request, response);
					break;


				case "":

					response.Abort();
					break;
			}
		}


        #region Get Keys

        private async Task GetKeysAsync(HttpListenerResponse response)
		{

			IdKeys keys = GenerateKeys();

			GotMessage?.Invoke("Keys Generated");


			await IO.SerializeAsync(response.OutputStream, keys);
		}


		private async Task GetAuthKeysAsync(HttpListenerRequest request,
			
			HttpListenerResponse response)
        {

			string login = await IO.ReadLineAsync(request.InputStream,
				
				request.ContentEncoding);


			//#TODO check sql-injections

			//#TODO get salt from db

			IdKeys authKeys = GenerateKeys(new byte[256]);


			GotMessage?.Invoke("Keys got");


			await IO.SerializeAsync(response.OutputStream, authKeys);
		}

        #endregion


        #region Generate Keys
        
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


		private IdKeys GenerateKeys(byte[] salt)
		{

			IdKeys keys = new IdKeys()
			{

				Salt = salt,

				DateTime = DateTime.Now.ToString()
			};


			return keys;
		}

        #endregion


		private async Task RegisterAsync(HttpListenerRequest request,

			HttpListenerResponse response)
		{

			ValueTask<RegistrationFields> task = JsonSerializer.DeserializeAsync
				
				<RegistrationFields>(request.InputStream);


			GotMessage?.Invoke("Registration fields are acquired");


			RegistrationFields fields = await task;


			//#TODO check data


			await IO.WriteAsync(response.OutputStream, request.ContentEncoding,
				
				"Registration completed)");
		}


		private async Task LoginAsync(HttpListenerRequest request, 
			
			HttpListenerResponse response)
        {

			ValueTask<LoginFields> task = JsonSerializer.DeserializeAsync

				<LoginFields>(request.InputStream);


			GotMessage?.Invoke("Login fields are acquired");


			LoginFields fields = await task;


			//#TODO check data


			await IO.WriteAsync(response.OutputStream, request.ContentEncoding,

				"Logged in)");
		}
	}

}
