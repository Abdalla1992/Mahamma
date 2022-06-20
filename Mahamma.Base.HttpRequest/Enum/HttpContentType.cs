using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.HttpRequest.Enum
{
    /// <summary>
    /// Http Content-Type used to identify the http request content type.
    /// </summary>
    public sealed class HttpContentType
    {
        #region Property
        public int Id { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region Enum Values
        public static HttpContentType Text = new(1, "text/plain".ToLowerInvariant());
        public static HttpContentType HTML = new(2, "text/html".ToLowerInvariant());
        public static HttpContentType JSON = new(3, "application/json".ToLowerInvariant());
        public static HttpContentType JavaScript = new(4, "application/javascript".ToLowerInvariant());
        public static HttpContentType XML = new(5, "application/xml".ToLowerInvariant());
        #endregion

        #region CTRS
        public HttpContentType(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion

        #region Methods
        public static IEnumerable<HttpContentType> List() => new[] { Text, HTML, JSON, JavaScript, XML };
        public static HttpContentType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);
            if (state == null)
            {
                throw new Exception($"Possible values for HttpContentType: {String.Join(",", List().Select(s => s.Name))}");
            }
            return state;
        }
        public static HttpContentType From(string name)
        {
            var state = List().SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for HttpContentType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        #endregion

    }
}
