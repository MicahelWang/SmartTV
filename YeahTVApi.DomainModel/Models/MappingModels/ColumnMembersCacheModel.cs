using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class ColumnMembersCacheModel
    {
        //TVModelColumnMember
        public int TVModelColumnMemberId { get; set; }
        public int TVModelColumnMemberModelColumnId { get; set; }
        public int TVModelColumnMemberModelCloumnItemId { get; set; }
        public Nullable<double> TVModelColumnMemberWeight { get; set; }
        public Nullable<int> TVModelColumnMemberColumnItemIndex { get; set; }

        //TVModelColumn
        public int TVModelColumnId { get; set; }
        public string TVModelColumnName { get; set; }
        public string TVModelColumnCode { get; set; }
        public int TVModelColumnModelId { get; set; }
        public Nullable<double> TVModelColumnWeight { get; set; }
        public Nullable<int> TVModelColumnColumnIndex { get; set; }
        public Nullable<bool> TVModelColumnActive { get; set; }
        public string TVModelColumnLastUpdater { get; set; }
        public Nullable<long> TVModelColumnLastUpdateTime { get; set; }
        public Nullable<long> TVModelColumnCreateTime { get; set; }

        //TVmodelColumnItem
        public int TVmodelColumnItemId { get; set; }
        public string TVmodelColumnItemCode { get; set; }
        public Nullable<int> TVmodelColumnItemActionType { get; set; }
        public string TVmodelColumnItemAction { get; set; }
        public Nullable<int> TVmodelColumnItemUseNetwork { get; set; }
        public string TVmodelColumnItemTitle { get; set; }
        public string TVmodelColumnItemEnTitle { get; set; }
        public string TVmodelColumnItemBackgroundImageUrl { get; set; }
        public string TVmodelColumnItemEnBackgroundImageUrl { get; set; }
        public string TVmodelColumnItemIconImageUrl { get; set; }
        public Nullable<int> TVmodelColumnItemModelId { get; set; }
        public Nullable<bool> TVmodelColumnItemActive { get; set; }
        public string TVmodelColumnItemLastUpdater { get; set; }
        public Nullable<long> TVmodelColumnItemLastUpdateTime { get; set; }
        public long TVmodelColumnItemCreateTime { get; set; }

        //TVModel
        public int TVModelId { get; set; }
        public string TVModelName { get; set; }
        public string TVModelCode { get; set; }
        public Nullable<bool> TVModelIsTopLevel { get; set; }
        public string TVModelRemark { get; set; }
        public string TVModelLastUpdater { get; set; }
        public Nullable<long> TVModelLastUpdateTime { get; set; }
        public Nullable<long> TVModelCreateTime { get; set; }
    }
}
