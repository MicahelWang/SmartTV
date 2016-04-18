using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface ISysAttachmentRepertory : IBsaeRepertory<CoreSysAttachment>
    {

        List<CoreSysAttachment> GetByIds(int[] ids);

    }
}