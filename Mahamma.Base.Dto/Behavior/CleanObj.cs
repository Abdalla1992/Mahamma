using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mahamma.Base.Dto.Behavior
{
    public class CleanObj
    {
        public void Clean()
        {
            var stringProperties = this.GetType().GetProperties().Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties.Where(a => a.SetMethod != null))
            {
                string currentValue = (string)stringProperty.GetValue(this, null);
                stringProperty.SetValue(this, RemoveWhiteSpace(currentValue));
            }
        }

        protected string RemoveWhiteSpace(string text)
        {
            string newText = text;
            if (text != null)
            {
                string[] spaceDecimals = ("9,10,11,13,32,115,133,160,5760,8192,8193,8194,8195,8196,8197,8198,8199,8200,8201,8202,8203,8204,8205,8230,8232,8233,8236,8237,8239,8287,12288,6158,8288,65279").Split(',');
                List<int> spaceUnicodes = new(spaceDecimals.Length);
                List<Regex> rejSpace = new(spaceDecimals.Length);
                spaceDecimals.ToList().ForEach(code => spaceUnicodes.Add(int.Parse(code)));
                spaceUnicodes.ForEach(code => rejSpace.Add(new Regex(Convert.ToString(Convert.ToChar(code)))));
                rejSpace.ForEach(rejex => rejex.Replace(text, string.Empty).Replace("?", string.Empty));
                newText = text.Trim();
            }
            return newText;
        }
    }
}
