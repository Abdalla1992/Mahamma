using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Dto
{
    public class SearchProjectDto : SearchDto<ProjectUserDto>, IRequest<PageList<ProjectUserDto>>
    {
    }
}
