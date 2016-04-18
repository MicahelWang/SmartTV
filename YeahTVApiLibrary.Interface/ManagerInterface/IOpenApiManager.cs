using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.ScoreStoreModels;

namespace YeahTVApiLibrary.Infrastructure.ManagerInterface
{
    public interface IOpenApiManager
    {
        bool VerificationToken(RequestTokenParameter requestTokenParameter);

    }
}
