using Mahamma.Base.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Language.Enum
{
    public class LanguageEnum : Enumeration
    {
        #region Enum Values
        public static LanguageEnum English = new(1, nameof(English));
        public static LanguageEnum Arabic = new(2, nameof(Arabic));
    
        #endregion

        #region CTRS
        public LanguageEnum(int id, string name)
            : base(id, name)
        {
        }
        #endregion

        #region Methods
        public static IEnumerable<LanguageEnum> List() => new[] {English , Arabic };
        public static LanguageEnum FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for LanguageEnum: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static LanguageEnum From(int id)
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
