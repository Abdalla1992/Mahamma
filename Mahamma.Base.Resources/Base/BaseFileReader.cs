using Mahamma.Base.Resources.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Base.Resources.Base
{
     public class BaseFileReader
    {
        #region Vars
        private List<Mahamma.Base.Resources.Dto.ResourceFileData> ResourceData { get; set; }
        #endregion

        public BaseFileReader(LocalizationType localizationType)
        {
            LoadData(localizationType);
        }

        #region Load Data
        private void LoadData(LocalizationType localizationType)
        {
            string fileName = localizationType.ToString();
            var rootDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            ResourceData = JsonConvert.DeserializeObject<List<Dto.ResourceFileData>>(File.ReadAllText($@"{rootDir}\ResourceFiles\{fileName}.json"));
        }
        #endregion

        #region Get Data
        protected Dictionary<string, string> GetKeyValue(string key)
        {
            return ResourceData.FirstOrDefault(k => k.Key == key).LocalizedValue;
        }

        public string GetKeyValue(string key, int languageId)
        {
            if (languageId == default)
            {
                languageId = LanguageEnum.English.Id;
                //languageId = (int)Core.Enum.LocalizationEnum.Language.English;
            }

            string value = string.Empty;

            Dictionary<string, string> rData = ResourceData.FirstOrDefault(k => k.Key.ToLower() == key.ToLower()).LocalizedValue;

            if (rData != null)
            {
                if (rData.Count >= (int)languageId)
                {
                    KeyValuePair<string, string> kv = rData.ElementAt((int)languageId - 1);
                    value = kv.Value;
                }
            }
            return value;
        }

        #endregion
    }
}
