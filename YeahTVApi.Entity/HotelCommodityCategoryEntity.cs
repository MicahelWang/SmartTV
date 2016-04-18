using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 酒店小商品目录
    /// </summary>
    public class HotelCommodityCategoryEntity
    {

        private string CategoryCodeField;
        private string CategoryIdField;
        private string CategoryNameField;

        public string CategoryCode
        {
            get
            {
                return this.CategoryCodeField;
            }
            set
            {
                this.CategoryCodeField = value;
            }
        }
        public string CategoryId
        {
            get
            {
                return this.CategoryIdField;
            }
            set
            {
                this.CategoryIdField = value;
            }
        }


        public string CategoryName
        {
            get
            {
                return this.CategoryNameField;
            }
            set
            {
                this.CategoryNameField = value;
            }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            HotelCommodityEntity entity = (HotelCommodityEntity)obj;
            if (
                entity.CategoryId == CategoryId && entity.CategoryCode ==
                CategoryCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return CategoryCode.GetHashCode();
        }
    }
}
