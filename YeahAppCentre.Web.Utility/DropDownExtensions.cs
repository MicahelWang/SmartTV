using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;

namespace YeahAppCentre.Web.Utility
{
    public static class DropDownExtensions
    {
        public static IEnumerable<SelectListItem> GetSelectList(Type type, string selectValue = "")
        {
            var source = EnumExtensions.GetItems(type);
            var items= source.ToSelectListItems();
            if (!string.IsNullOrWhiteSpace(selectValue))
            {
                var selectItem = items.FirstOrDefault(m => m.Value.Equals(selectValue));
                if (selectItem != null)
                    selectItem.Selected = true;
            }
            return items;
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(this List<CoreSysGroup> source)
        {
            return source.Select(obj => new SelectListItem
            {
                Text = obj.GroupName,
                Value = obj.Id
            }).ToList().AddRangeDefaultItems();
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(this List<EnumItem> source)
        {
            return source.Select(obj => new SelectListItem
            {
                Text = obj.Description,
                Value = obj.Value.ToString()
            }).ToList().AddRangeDefaultItems();
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(this List<ErpPowerRole> source)
        {
            return source.Select(obj => new SelectListItem
            {
                Text = obj.RoleName,
                Value = obj.Id
            }).ToList().AddRangeDefaultItems();
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(this List<TvTemplateType> source)
        {
            return source.Select(obj => new SelectListItem
            {
                Text = obj.Name,
                Value = obj.Id.ToString()
            }).ToList().AddRangeDefaultItems();
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(this List<TvTemplate> source)
        {
            return source.Select(obj => new SelectListItem
            {
                Text = obj.Name,
                Value = obj.Id.ToString()
            }).ToList().AddRangeDefaultItems();
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(this List<CoreSysCounty> source)
        {
            return source.Select(obj => new SelectListItem
            {
                Text = obj.Name,
                Value = obj.Id.ToString()
            }).ToList().AddRangeDefaultItems();
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(this List<Apps> source)
        {
            return source.Select(obj => new SelectListItem
            {
                Text = obj.Name,
                Value = obj.Id.ToString()
            }).ToList().AddRangeDefaultItems();
        }

        #region 帮助方法
        public static readonly SelectListItem DefaultItem = new SelectListItem
        {
            Text = "-请选择-",
            Value = ""
        };

        private static IEnumerable<SelectListItem> AddRangeDefaultItems(this IEnumerable<SelectListItem> sourceItems, bool hasDefault = true)
        {
            var items = sourceItems.ToList();
            if (!hasDefault) return items;
            items.Insert(0, DefaultItem);
            var data = items.ToList();
            return data;
        }

        #endregion 帮助方法
    }
}
