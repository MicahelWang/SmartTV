using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Entity.NewEntity
{
    public class RoomMemberInfo
    {

        public string memberId;
        public string sReceiveId;
        public bool isMainOrder;

        public RoomMemberInfo(string memberId, string sReceiveId,bool isMainOrder)
        {
            this.memberId = memberId;
            this.sReceiveId = sReceiveId;
            this.isMainOrder = isMainOrder;
        }
    }
}
