using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Document.Infrastructure.MigrationSetting
{
    public class MigrationScripts
    {
        public static List<string> ApplyDbScripts(string scriptFolderName, DbContext appDbContext)
        {
            List<string> dbObjectName = new();
            string folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), scriptFolderName);
            if (Directory.Exists(folderPath))
            {
                string sqlString = string.Empty;

                foreach (string resourceName in Directory.GetFiles(folderPath))
                {
                    string fileName = Path.GetFileName(resourceName);
                    Console.WriteLine($"Start Apply {fileName}");
                    dbObjectName.Add(fileName);

                    sqlString = File.ReadAllText(resourceName);
                    foreach (string quertPart in SplitQuery(sqlString))
                    {
                        if (!string.IsNullOrWhiteSpace(quertPart))
                            appDbContext.Database.ExecuteSqlRaw(quertPart);
                    }
                }
            }

            return dbObjectName;
        }

        private static List<string> SplitQuery(string query)
        {
            return query.Split("GO").ToList();
        }
    }
}
