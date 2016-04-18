using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

namespace YeahTVApi.Common
{

    public class EnumItem
    {
        public int Value { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
    }

    public class ListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public static class EnumExtensions
    {
        public static Dictionary<Type, List<EnumItem>> Ht = new Dictionary<Type, List<EnumItem>>();
        public static Dictionary<Type, string> HtTitle = new Dictionary<Type, string>();
        /// <summary> 
        /// 获得枚举类型数据项（不包括空项）
        /// </summary> 
        /// <param name="enumType">枚举类型</param> 
        /// <returns></returns> 
        public static List<EnumItem> GetItems(Type enumType)
        {
            var list = new List<EnumItem>();
            if (Ht.ContainsKey(enumType))
            {
                list = Ht[enumType];
                return list;
            }
            // 获取Description特性 
            Type typeDescription = typeof(DescriptionAttribute);
            // 获取枚举字段
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (!field.FieldType.IsEnum)
                    continue;

                // 获取枚举值
                int value = (int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);

                // 不包括空项
                string text = string.Empty;
                string description = string.Empty;

                object[] array = field.GetCustomAttributes(typeDescription, false);

                text = field.Name; //没有描述，直接取值
                if (array.Length > 0)
                {
                    description = ((DescriptionAttribute)array[0]).Description;
                }
                else
                {
                    description = text;
                }
                //添加到列表
                list.Add(new EnumItem() { Value = value, Text = text, Description = description });

            }
            Ht[enumType] = list;
            return list;
        }

        public static string GetValueStr(this Enum enumValue)
        {
            var list = GetItems(enumValue.GetType());
            var item = GetItem(list, enumValue);
            return item != null ? item.Value.ToString(CultureInfo.InvariantCulture) : "";
        }

        public static bool GetBoolValue(this Enum enumValue)
        {
            return enumValue.GetValueStr() != "0";
        }

        public static string GetDescription(this Enum enumValue)
        {
            var list = GetItems(enumValue.GetType());
            var item = GetItem(list, enumValue);
            if (item == null) return "";
            return item.Description == "" ? item.Text : item.Description;
        }

        public static string GetText(this Enum enumValue)
        {
            var list = GetItems(enumValue.GetType());
            var item = GetItem(list, enumValue);
            return item != null ? item.Text : "";
        }

        private static EnumItem GetItem(List<EnumItem> list, Enum enumValue)
        {
            var item = list.FirstOrDefault(i => i.Value == Convert.ToInt32(enumValue));
            return item;
        }


        public static T ParseAsEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value.Trim());
        }
        public static T ParseAsEnum<T>(this int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static T ParseAsEnum<T>(this bool value)
        {
            return (T)Enum.Parse(typeof(T), value ? "1" : "0");
        }

        public static string GetEnumDescription<T>(this string value)
        {
            return GetEnumDescription<T>(Convert.ToInt32(value));
        }
        public static string GetEnumDescription<T>(this int value)
        {
            var list = GetItems(typeof(T));
            var singleOrDefault = list.SingleOrDefault(_ => _.Value == value);
            if (singleOrDefault != null)
                return singleOrDefault.Description;
            return "";
        }

        public static bool TryParseAsEnum<T>(this string value, out T result) where T : struct
        {
            return Enum.TryParse(value, out result);
        }

        public static void BindEnum<T>(this ListControl listControl, Enum itemToSelect = null, DdlType type = DdlType.none) where T : struct
        {

            var list = GetControlList<T>(type);
            string selectedValue = null;
            if (itemToSelect != null)
            {
                selectedValue = Convert.ToInt32(itemToSelect).ToString();
            }
            listControl.BindList(list, selectedValue);
        }

        public static void BindList(this ListControl listControl, object source, string itemToSelect = null)
        {
            listControl.DataTextField = "Text";
            listControl.DataValueField = "Value";
            listControl.DataSource = source;
            listControl.DataBind();
            if (itemToSelect != null)
            {
                listControl.SelectedValue = itemToSelect;
            }
        }
        public static void BindList(this ListControl listControl, object source, string fieldText, string fieldValue, string itemToSelect = null)
        {
            listControl.DataTextField = fieldText;
            listControl.DataValueField = fieldValue;
            listControl.DataSource = source;
            listControl.DataBind();
            if (itemToSelect != null)
            {
                listControl.SelectedValue = itemToSelect;
            }
        }

        public static List<ListItem> GetControlList<T>(DdlType type = DdlType.none)
        {
            var list = GetItems(typeof(T));

            var controlList = (from i in list
                               select new ListItem() { Text = i.Description, Value = i.Value.ToString() }).ToList();

            if (type == DdlType.all)
            {
                controlList.Insert(0, new ListItem() { Text = "所有", Value = "" });
            }
            else if (type == DdlType.blank)
            {
                controlList.Insert(0, new ListItem() { Text = "", Value = "" });
            }

            return controlList;
        }

        public static string GetClassDescription<T>()
        {
            if (HtTitle.ContainsKey(typeof(T)))
            {
                return HtTitle[typeof(T)];
            }
            else
            {
                Type typeDescription = typeof(DescriptionAttribute);
                var type = typeof(T);
                object[] array = type.GetCustomAttributes(typeDescription, false);

                var rtn = type.Name; //没有描述，直接取值
                if (array.Length > 0)
                {
                    rtn = ((DescriptionAttribute)array[0]).Description;
                }
                HtTitle[typeof(T)] = rtn;
                return rtn;
            }

        }

        //checks to see if an enumerated value contains a type
        public static bool Has<T>(this System.Enum type, T value)
        {
            try
            {
                return (((int)(object)type &
                  (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }
    }


    ///<summary>
    /// 下拉框显示类型
    ///</summary>
    public enum DdlType
    {
        none,
        all,
        blank,
    }
}