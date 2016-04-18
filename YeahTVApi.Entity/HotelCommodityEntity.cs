using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 酒店小商品
    /// </summary>
    public class HotelCommodityEntity:HotelCommodityCategoryEntity
    {


        private string BrandField; 
        private string CodeField;


        private System.Nullable<decimal> CostPriceField;


        private System.Nullable<int> DefReceiverTypeField;


        private System.Nullable<decimal> DiscountPriceField;


        private bool HasInventoryField;


        private string HotelIDField;


        private string IdField;


        private System.Nullable<int> InventoryQuantityField;


        private bool IsActiveField;


        private bool IsFixCostField;


        private bool IsSysPriceField;


        private System.Nullable<decimal> MarketingPriceField;


        private string MaterialField;


        private string MnemonicCodeField;


        private string ModelField;


        private string NCAccount1Field;


        private string NCAccount2Field;


        private string NCAccount2CostCenterField;


        private string NameField;


        private string ParentIdField;


        private string ProductIdField;


        private int PurchaseLevelField;


        private string RemarkField;


        private string SpecField;


        private string UOMField;


        public string Brand
        {
            get
            {
                return this.BrandField;
            }
            set
            {
                this.BrandField = value;
            }
        }


      


        public string Code
        {
            get
            {
                return this.CodeField;
            }
            set
            {
                this.CodeField = value;
            }
        }


        public System.Nullable<decimal> CostPrice
        {
            get
            {
                return this.CostPriceField;
            }
            set
            {
                this.CostPriceField = value;
            }
        }


        public System.Nullable<int> DefReceiverType
        {
            get
            {
                return this.DefReceiverTypeField;
            }
            set
            {
                this.DefReceiverTypeField = value;
            }
        }


        public System.Nullable<decimal> DiscountPrice
        {
            get
            {
                return this.DiscountPriceField;
            }
            set
            {
                this.DiscountPriceField = value;
            }
        }


        public bool HasInventory
        {
            get
            {
                return this.HasInventoryField;
            }
            set
            {
                this.HasInventoryField = value;
            }
        }


        public string HotelID
        {
            get
            {
                return this.HotelIDField;
            }
            set
            {
                this.HotelIDField = value;
            }
        }


        public string Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }


        public System.Nullable<int> InventoryQuantity
        {
            get
            {
                return this.InventoryQuantityField;
            }
            set
            {
                this.InventoryQuantityField = value;
            }
        }


        public bool IsActive
        {
            get
            {
                return this.IsActiveField;
            }
            set
            {
                this.IsActiveField = value;
            }
        }


        public bool IsFixCost
        {
            get
            {
                return this.IsFixCostField;
            }
            set
            {
                this.IsFixCostField = value;
            }
        }


        public bool IsSysPrice
        {
            get
            {
                return this.IsSysPriceField;
            }
            set
            {
                this.IsSysPriceField = value;
            }
        }


        public System.Nullable<decimal> MarketingPrice
        {
            get
            {
                return this.MarketingPriceField;
            }
            set
            {
                this.MarketingPriceField = value;
            }
        }


        public string Material
        {
            get
            {
                return this.MaterialField;
            }
            set
            {
                this.MaterialField = value;
            }
        }


        public string MnemonicCode
        {
            get
            {
                return this.MnemonicCodeField;
            }
            set
            {
                this.MnemonicCodeField = value;
            }
        }


        public string Model
        {
            get
            {
                return this.ModelField;
            }
            set
            {
                this.ModelField = value;
            }
        }


        public string NCAccount1
        {
            get
            {
                return this.NCAccount1Field;
            }
            set
            {
                this.NCAccount1Field = value;
            }
        }


        public string NCAccount2
        {
            get
            {
                return this.NCAccount2Field;
            }
            set
            {
                this.NCAccount2Field = value;
            }
        }


        public string NCAccount2CostCenter
        {
            get
            {
                return this.NCAccount2CostCenterField;
            }
            set
            {
                this.NCAccount2CostCenterField = value;
            }
        }


        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }


        public string ParentId
        {
            get
            {
                return this.ParentIdField;
            }
            set
            {
                this.ParentIdField = value;
            }
        }


        public string ProductId
        {
            get
            {
                return this.ProductIdField;
            }
            set
            {
                this.ProductIdField = value;
            }
        }


        public int PurchaseLevel
        {
            get
            {
                return this.PurchaseLevelField;
            }
            set
            {
                this.PurchaseLevelField = value;
            }
        }


        public string Remark
        {
            get
            {
                return this.RemarkField;
            }
            set
            {
                this.RemarkField = value;
            }
        }


        public string Spec
        {
            get
            {
                return this.SpecField;
            }
            set
            {
                this.SpecField = value;
            }
        }


        public string UOM
        {
            get
            {
                return this.UOMField;
            }
            set
            {
                this.UOMField = value;
            }
        }
    }
}
