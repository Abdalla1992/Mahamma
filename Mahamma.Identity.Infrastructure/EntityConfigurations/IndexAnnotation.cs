using Microsoft.EntityFrameworkCore;

namespace Mahamma.Identity.Infrastructure.EntityConfigurations
{
    internal class IndexAnnotation
    {
        private IndexAttribute indexAttribute;

        public IndexAnnotation(IndexAttribute indexAttribute)
        {
            this.indexAttribute = indexAttribute;
        }
    }
}