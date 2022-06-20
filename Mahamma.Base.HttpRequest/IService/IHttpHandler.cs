using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.HttpRequest.IService
{
    public interface IHttpHandler
    {
        Dto.HttpResponseDto Get(string url, Dictionary<string, string> headers = default);
        Dto.HttpResponseDto Post(string url, object obj, Dictionary<string, string> headers = default);
        Dto.HttpResponseDto Put(string url, object obj, Dictionary<string, string> headers = default);
        Dto.HttpResponseDto Patch(string url, object obj, Dictionary<string, string> headers = default);
        Dto.HttpResponseDto Delete(string url, Dictionary<string, string> headers = default);

        Task<Dto.HttpResponseDto> GetAsync(string url, Dictionary<string, string> headers = default);
        Task<Dto.HttpResponseDto> PostAsync(string url, object obj, Dictionary<string, string> headers = default);
        Task<Dto.HttpResponseDto> PutAsync(string url, object obj, Dictionary<string, string> headers = default);
        Task<Dto.HttpResponseDto> PatchAsync(string url, object obj, Dictionary<string, string> headers = default);
        Task<Dto.HttpResponseDto> DeleteAsync(string url, Dictionary<string, string> headers = default);
    }
}
