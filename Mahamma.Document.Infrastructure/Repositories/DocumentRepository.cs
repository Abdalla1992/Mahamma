//using AutoMapper;
//using Mahamma.Document.Domain._SharedKernel;
//using Mahamma.Document.Domain.Document.Repository;
//using Mahamma.Document.Infrastructure.Context;

//namespace Mahamma.Document.Infrastructure.Repositories
//{
//    public class DocumentRepository : Base.EntityRepository<Domain.Document.Entity.Document>, IDocumentRepository
//    {
//        public IUnitOfWork UnitOfWork => AppDbContext;
//        public DocumentRepository(IMapper mapper, AppDbContext context) : base(context, mapper)
//        { }
//    }
//}
