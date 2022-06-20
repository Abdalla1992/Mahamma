using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.Language.Enum
{
   public class Language : Enumeration
    {
        #region Enum Values
        public static Language English = new(1, nameof(English));
        public static Language Arabic = new(2, nameof(Arabic));

        #endregion

        #region CTRS
        public Language(int id, string name)
            : base(id, name)
        {
        }
        #endregion

        #region Methods
        public static IEnumerable<Language> List() => new[] { English, Arabic };
        public static Language FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for LanguageEnum: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static Language From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for LanguageEnum: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
        #endregion
    }
}
