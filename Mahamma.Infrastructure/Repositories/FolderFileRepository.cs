using AutoMapper;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.ProjectAttachment.Entity;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Infrastructure.Context;
using System.Collections.Generic;

namespace Mahamma.Infrastructure.Repositories
{
    public class FolderFileRepository : Base.EntityRepository<FolderFile>, IFolderFileRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public FolderFileRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public void AddFolderFiles(List<FolderFile> folderFiles)
        {
            CreateListAsyn(folderFiles);
        }

        public void AddFolderFile(FolderFile folderFile)
        {
            CreateAsyn(folderFile);
        }
    }
}
