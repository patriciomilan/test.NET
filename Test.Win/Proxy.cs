using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Test.Win
{
	public abstract class Proxy
	{
		// AL Obtener una respuesta de etxto (JSON).
		public delegate void OnJsonResponseEventHandler(string json);
		public event OnJsonResponseEventHandler OnJsonResponse;

		// Configuración Url Base
		public delegate void OnConfigureEventHandler(ProxyConfiguracion configuracion);
		public static event OnConfigureEventHandler OnConfigure;

		// Seguridad
		public delegate string OnActivateSecureHandler();
		public event OnActivateSecureHandler OnSecure;

        public delegate string OnConfigureSecureEventHandler();
		public static event OnConfigureSecureEventHandler OnConfigureSecure;

        private ProxyConfiguracion _proxyConfiguracion;

        public ProxyConfiguracion Configuracion
        {
            get { return _proxyConfiguracion; }
            set { _proxyConfiguracion = value; }
        }


        public enum Metodo
		{
			GET,
			POST,
			PUT,
			DELETE
		}

		public string OAuthToken { get; set; }

        private string _resultado = string.Empty;

        public string Resultado
        {
            get { return _resultado; }
            set { _resultado = value; }
        }



        public Proxy() {}

		protected async Task<String> ObtenerRespuesta(string url, Metodo metodo, string json = "")
		{
			if (Configuracion == null)
			{
				Configuracion = new ProxyConfiguracion();
				OnConfigure?.Invoke(Configuracion);
			}

			ConfigurarSeguridad();

			string urlFinal;
			if (url.Contains("http"))
				urlFinal = url;
			else
				urlFinal = Configuracion.UrlBase + url;

			HttpClient cliente = new HttpClient();
			cliente.MaxResponseContentBufferSize = 2000000;
			cliente.Timeout = new TimeSpan(0, 0, 30);

			// Add Token
			if (!string.IsNullOrEmpty(OAuthToken))
				cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OAuthToken);

			try
			{
				if (metodo == Metodo.POST || metodo == Metodo.PUT)
				{
					if (json != string.Empty)
					{
						var content = new StringContent(json, Encoding.UTF8, "application/json");
						if (metodo == Metodo.POST)
                            Respuesta = cliente.PostAsync(urlFinal, content).Result;

						if (metodo == Metodo.PUT)
                            Respuesta = cliente.PutAsync(urlFinal, content).Result;
					}

					Stream receiveStream = await Respuesta.Content.ReadAsStreamAsync();
					StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    Resultado = readStream.ReadToEnd();
					if (Resultado != string.Empty)
						OnJsonResponse?.Invoke(Resultado);
				}
				else
				{
					if (metodo == Metodo.GET)
                        Respuesta = cliente.GetAsync(urlFinal).Result;

					if (metodo == Metodo.DELETE)
                        Respuesta = cliente.DeleteAsync(urlFinal).Result;

					Stream receiveStream = await Respuesta.Content.ReadAsStreamAsync();
					StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    Resultado = readStream.ReadToEnd();
					if (Resultado != string.Empty)
						OnJsonResponse?.Invoke(Resultado);
				}
			}
			catch (Exception ex)
			{
                Resultado = ex.ToString();
			}
			return Resultado;
		}

        protected async Task<Stream> ObtenerRespuestaArchivo(string url, Metodo metodo, string json = "")
        {
            Stream receiveStream = new MemoryStream();
            if (Configuracion == null)
            {
                Configuracion = new ProxyConfiguracion();
                OnConfigure?.Invoke(Configuracion);
            }

            ConfigurarSeguridad();

            string urlFinal;
            if (url.Contains("http"))
                urlFinal = url;
            else
                urlFinal = Configuracion.UrlBase + url;

            HttpClient cliente = new HttpClient();
            cliente.MaxResponseContentBufferSize = 2000000;
            cliente.Timeout = new TimeSpan(0, 0, 30);

            // Add Token
            if (!string.IsNullOrEmpty(OAuthToken))
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OAuthToken);

            try
            {
                if (metodo == Metodo.POST || metodo == Metodo.PUT)
                {
                    if (json != string.Empty)
                    {
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        if (metodo == Metodo.POST)
                            Respuesta = cliente.PostAsync(urlFinal, content).Result;

                        if (metodo == Metodo.PUT)
                            Respuesta = cliente.PutAsync(urlFinal, content).Result;
                    }

                    receiveStream = await Respuesta.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    Resultado = readStream.ReadToEnd();
                    if (Resultado != string.Empty)
                        OnJsonResponse?.Invoke(Resultado);
                }
                else
                {
                    if (metodo == Metodo.GET)
                        Respuesta = cliente.GetAsync(urlFinal).Result;

                    if (metodo == Metodo.DELETE)
                        Respuesta = cliente.DeleteAsync(urlFinal).Result;

                    receiveStream = await Respuesta.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    Resultado = readStream.ReadToEnd();
                    if (Resultado != string.Empty)
                        OnJsonResponse?.Invoke(Resultado);
                }
            }
            catch (Exception ex)
            {
                Resultado = ex.ToString();
            }
            return receiveStream;
        }

        public HttpResponseMessage Respuesta { get; private set; }

        protected virtual void ConfigurarSeguridad()
		{
			if (string.IsNullOrEmpty(OAuthToken))
			{
				OAuthToken = OnSecure?.Invoke();
                if (string.IsNullOrEmpty(OAuthToken))
                    OAuthToken = OnConfigureSecure?.Invoke();
			}
		}
	}
}