using Mahamma.Base.HttpRequest.Dto;
using Mahamma.Base.HttpRequest.IService;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mahamma.Base.HttpRequest.Service
{
    internal class HttpHandler : IHttpHandler
    {

        #region Constructor
        private readonly Settings.HttpRequestSettings _httpRequestSettings;
        public HttpHandler(Settings.HttpRequestSettings httpRequestSettings)
        {
            this._httpRequestSettings = httpRequestSettings;
        }
        #endregion

        #region Non-Async
        public HttpResponseDto Get(string url, Dictionary<string, string> headers = null)
        {
            HttpResponseDto responseDto = new();
            try
            {
                #region Client
                RestClient client = new(url)
                {
                    Timeout = _httpRequestSettings.RequestTimeout
                };

                if (_httpRequestSettings.IgnoreSSL)
                    client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                #endregion

                #region Request
                RestRequest request = new(Method.GET);
                if (headers?.Any() ?? default)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                #endregion

                #region Response
                IRestResponse response = client.Execute(request);
                responseDto = new()
                {
                    StatusCode = response.StatusCode,
                    Content = DeserializeContent(response)
                };

                #endregion
            }
            catch (WebException webex)
            {
                responseDto = WebExceptionHandler(webex, System.Reflection.MethodBase.GetCurrentMethod());
            }
            catch (System.Exception exception)
            {
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
            }
            return responseDto;
        }

        public HttpResponseDto Post(string url, object obj, Dictionary<string, string> headers = null)
        {
            HttpResponseDto responseDto = new();
            try
            {
                #region Client
                RestClient client = new(url)
                {
                    Timeout = _httpRequestSettings.RequestTimeout
                };
                if (_httpRequestSettings.IgnoreSSL)
                    client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                #endregion

                #region Request
                RestRequest request = new(Method.POST);
                request.AddHeader("Content-Type", Enum.HttpContentType.JSON.Name);
                request.AddParameter(Enum.HttpContentType.JSON.Name, JsonConvert.SerializeObject(obj), ParameterType.RequestBody);

                if (headers?.Any() ?? default)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                #endregion

                #region Response
                IRestResponse response = client.Execute(request);
                responseDto = new()
                {
                    StatusCode = response.StatusCode,
                    Content = DeserializeContent(response)
                };

                #endregion
            }
            catch (WebException webex)
            {
                responseDto = WebExceptionHandler(webex, System.Reflection.MethodBase.GetCurrentMethod());
            }
            catch (System.Exception exception)
            {
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
            }
            return responseDto;
        }

        public HttpResponseDto Put(string url, object obj, Dictionary<string, string> headers = null)
        {
            HttpResponseDto responseDto = new();
            try
            {
                #region Client
                RestClient client = new(url)
                {
                    Timeout = _httpRequestSettings.RequestTimeout
                };

                if (_httpRequestSettings.IgnoreSSL)
                    client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                #endregion

                #region Request
                RestRequest request = new(Method.PUT);
                request.AddHeader("Content-Type", Enum.HttpContentType.JSON.Name);
                request.AddParameter(Enum.HttpContentType.JSON.Name, JsonConvert.SerializeObject(obj), ParameterType.RequestBody);

                if (headers?.Any() ?? default)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                #endregion

                #region Response
                IRestResponse response = client.Execute(request);
                responseDto = new()
                {
                    StatusCode = response.StatusCode,
                    Content = DeserializeContent(response)
                };

                #endregion
            }
            catch (WebException webex)
            {
                responseDto = WebExceptionHandler(webex, System.Reflection.MethodBase.GetCurrentMethod());
            }
            catch (System.Exception exception)
            {
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
            }
            return responseDto;
        }

        public HttpResponseDto Patch(string url, object obj, Dictionary<string, string> headers = null)
        {
            HttpResponseDto responseDto = new();
            try
            {
                #region Client
                RestClient client = new(url)
                {
                    Timeout = _httpRequestSettings.RequestTimeout
                };
                if (_httpRequestSettings.IgnoreSSL)
                    client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                #endregion

                #region Request
                RestRequest request = new(Method.PATCH);
                request.AddHeader("Content-Type", Enum.HttpContentType.JSON.Name);
                request.AddParameter(Enum.HttpContentType.JSON.Name, JsonConvert.SerializeObject(obj), ParameterType.RequestBody);

                if (headers?.Any() ?? default)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                #endregion

                #region Response
                IRestResponse response = client.Execute(request);
                responseDto = new()
                {
                    StatusCode = response.StatusCode,
                    Content = DeserializeContent(response)
                };
                #endregion
            }
            catch (WebException webex)
            {
                responseDto = WebExceptionHandler(webex, System.Reflection.MethodBase.GetCurrentMethod());
            }
            catch (System.Exception exception)
            {
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
            }
            return responseDto;
        }

        public HttpResponseDto Delete(string url, Dictionary<string, string> headers = null)
        {
            HttpResponseDto responseDto = new();
            try
            {
                #region Client
                RestClient client = new(url)
                {
                    Timeout = _httpRequestSettings.RequestTimeout
                };
                if (_httpRequestSettings.IgnoreSSL)
                    client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                #endregion

                #region Request
                RestRequest request = new(Method.DELETE);
                if (headers?.Any() ?? default)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                #endregion

                #region Response
                IRestResponse response = client.Execute(request);
                responseDto = new()
                {
                    StatusCode = response.StatusCode,
                    Content = DeserializeContent(response)
                };

                #endregion
            }
            catch (WebException webex)
            {
                responseDto = WebExceptionHandler(webex, System.Reflection.MethodBase.GetCurrentMethod());
            }
            catch (System.Exception exception)
            {
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
            }
            return responseDto;
        }
        #endregion

        #region Async
        public async Task<HttpResponseDto> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            HttpResponseDto responseDto = new();
            try
            {
                #region Client
                RestClient client = new(url)
                {
                    Timeout = _httpRequestSettings.RequestTimeout
                };
                if (_httpRequestSettings.IgnoreSSL)
                    client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                #endregion

                #region Request
                RestRequest request = new(Method.GET);
                if (headers?.Any() ?? default)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                #endregion

                #region Response
                IRestResponse response = await client.ExecuteAsync(request);
                responseDto = new()
                {
                    StatusCode = response.StatusCode,
                    Content = DeserializeContent(response)
                };
                #endregion
            }
            catch (WebException webex)
            {
                responseDto = WebExceptionHandler(webex, System.Reflection.MethodBase.GetCurrentMethod());
            }
            catch (System.Exception exception)
            {
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
            }
            return responseDto;
        }

        public async Task<HttpResponseDto> PostAsync(string url, object obj, Dictionary<string, string> headers = null)
        {
            HttpResponseDto responseDto = new();
            try
            {
                #region Client
                RestClient client = new(url)
                {
                    Timeout = _httpRequestSettings.RequestTimeout
                };
                if (_httpRequestSettings.IgnoreSSL)
                    client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                #endregion

                #region Request
                RestRequest request = new(Method.POST);
                request.AddHeader("Content-Type", Enum.HttpContentType.JSON.Name);
                request.AddParameter(Enum.HttpContentType.JSON.Name, JsonConvert.SerializeObject(obj), ParameterType.RequestBody);

                if (headers?.Any() ?? default)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                #endregion

                #region Response
                IRestResponse response = await client.ExecuteAsync(request);
                responseDto = new()
                {
                    StatusCode = response.StatusCode,
                    Content = DeserializeContent(response)
                };
                #endregion
            }
            catch (WebException webex)
            {
                responseDto = WebExceptionHandler(webex, System.Reflection.MethodBase.GetCurrentMethod());
            }
            catch (System.Exception exception)
            {
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
            }
            return responseDto;
        }

        public async Task<HttpResponseDto> PutAsync(string url, object obj, Dictionary<string, string> headers = null)
        {
            HttpResponseDto responseDto = new();
            try
            {
                #region Client
                RestClient client = new(url)
                {
                    Timeout = _httpRequestSettings.RequestTimeout
                };
                if (_httpRequestSettings.IgnoreSSL)
                    client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                #endregion

                #region Request
                RestRequest request = new(Method.PUT);
                request.AddHeader("Content-Type", Enum.HttpContentType.JSON.Name);
                if (obj != null)
                {
                    request.AddParameter(Enum.HttpContentType.JSON.Name, JsonConvert.SerializeObject(obj), ParameterType.RequestBody);
                }

                if (headers?.Any() ?? default)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                #endregion

                #region Response
                IRestResponse response = await client.ExecuteAsync(request);
                responseDto = new()
                {
                    StatusCode = response.StatusCode,
                    Content = DeserializeContent(response)
                };
                #endregion
            }
            catch (WebException webex)
            {
                responseDto = WebExceptionHandler(webex, System.Reflection.MethodBase.GetCurrentMethod());
            }
            catch (System.Exception exception)
            {
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
            }
            return responseDto;
        }

        public async Task<HttpResponseDto> PatchAsync(string url, object obj, Dictionary<string, string> headers = null)
        {
            HttpResponseDto responseDto = new();
            try
            {
                #region Client
                RestClient client = new(url)
                {
                    Timeout = _httpRequestSettings.RequestTimeout
                };
                if (_httpRequestSettings.IgnoreSSL)
                    client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                #endregion

                #region Request
                RestRequest request = new(Method.PATCH);
                request.AddHeader("Content-Type", Enum.HttpContentType.JSON.Name);
                request.AddParameter(Enum.HttpContentType.JSON.Name, JsonConvert.SerializeObject(obj), ParameterType.RequestBody);

                if (headers?.Any() ?? default)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                #endregion

                #region Response
                IRestResponse response = await client.ExecuteAsync(request);
                responseDto = new()
                {
                    StatusCode = response.StatusCode,
                    Content = DeserializeContent(response)
                };
                #endregion
            }
            catch (WebException webex)
            {
                responseDto = WebExceptionHandler(webex, System.Reflection.MethodBase.GetCurrentMethod());
            }
            catch (System.Exception exception)
            {
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
            }
            return responseDto;
        }

        public async Task<HttpResponseDto> DeleteAsync(string url, Dictionary<string, string> headers = null)
        {
            HttpResponseDto responseDto = new();
            try
            {
                #region Client
                RestClient client = new(url)
                {
                    Timeout = _httpRequestSettings.RequestTimeout
                };
                if (_httpRequestSettings.IgnoreSSL)
                    client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                #endregion

                #region Request
                RestRequest request = new(Method.DELETE);
                if (headers?.Any() ?? default)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                #endregion

                #region Response
                IRestResponse response = await client.ExecuteAsync(request);
                responseDto = new()
                {
                    StatusCode = response.StatusCode,
                    Content = DeserializeContent(response)
                };
                #endregion
            }
            catch (WebException webex)
            {
                responseDto = WebExceptionHandler(webex, System.Reflection.MethodBase.GetCurrentMethod());
            }
            catch (System.Exception exception)
            {
                Log.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, exception);
            }
            return responseDto;
        }

        #endregion

        #region Private - Methods

        private HttpResponseDto WebExceptionHandler(WebException webex, System.Reflection.MethodBase methodBase)
        {
            HttpResponseDto response = null;
            if (webex.Response != null)
            {
                WebResponse errResp = webex.Response;
                HttpWebResponse resp = (HttpWebResponse)webex.Response;
                using Stream respStream = errResp.GetResponseStream();
                //if (resp.StatusCode == HttpStatusCode.BadRequest)
                //{
                string content = new StreamReader(respStream).ReadToEnd();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    response = new()
                    {
                        StatusCode = resp.StatusCode,
                        Content = content
                    };
                }
                else
                {
                    Log.Error(methodBase.Name, webex);
                }
                //}
                //else
                //{
                //    Log.Error(methodBase.Name, webex);
                //}
            }
            else
            {
                Log.Error(methodBase.Name, webex);
            }
            return response;
        }

        private string DeserializeContent(IRestResponse response)
        {
            if (response != null && !string.IsNullOrWhiteSpace(response.Content))
                return response.Content;
            else
                return string.Empty;
        }

        #endregion

    }
}
