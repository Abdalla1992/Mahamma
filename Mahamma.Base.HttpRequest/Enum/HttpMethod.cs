using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.HttpRequest.Enum
{
    /// <summary>
    /// Http method used to identify the http request method.
    /// </summary>
    public sealed class HttpMethod
    {
        #region Property
        public int Id { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region Enum Values
        public static HttpMethod Get = new(1, nameof(Get).ToUpper().ToLowerInvariant());
        public static HttpMethod Post = new(2, nameof(Post).ToUpper().ToLowerInvariant());
        public static HttpMethod Put = new(3, nameof(Put).ToUpper().ToLowerInvariant());
        public static HttpMethod Patch = new(4, nameof(Patch).ToUpper().ToLowerInvariant());
        public static HttpMethod Delete = new(5, nameof(Delete).ToUpper().ToLowerInvariant());
        #endregion

        #region CTRS
        public HttpMethod(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion

        #region Methods
        public static IEnumerable<HttpMethod> List() => new[] { Get, Post, Put, Patch, Delete };
        public static HttpMethod From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);
            if (state == null)
            {
                throw new Exception($"Possible values for HttpMethod: {String.Join(",", List().Select(s => s.Name))}");
            }
            return state;
        }
        public static HttpMethod From(string name)
        {
            var state = List().SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for HttpMethod: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        #endregion

    }
}
