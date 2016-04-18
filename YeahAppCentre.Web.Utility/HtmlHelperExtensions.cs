using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;
using YeahTVApi.Common;

namespace YeahAppCentre.Web.Utility
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Pagination<TListItem>(this HtmlHelper html, IPagedList<TListItem> list, string updateJqId, string callbackurl)
        {
            if (list == null || list.Count == 0)
            {
                return MvcHtmlString.Create("");
            }
            var pagination = new Pagination();
            pagination.TotleCount = list.TotalCount;
            pagination.PageIndex = list.PageIndex;
            pagination.PageSize = list.PageSize;
            pagination.JqUpdateElement = updateJqId;
            pagination.CallBackUrl = callbackurl;
            var str = (html.ViewContext.Controller as Controller).RenderPartialViewToString("_Pagination", pagination);
            return MvcHtmlString.Create(str);
        }

        public static MvcHtmlString Pagination(this HtmlHelper html, int pageIndex, int pageSize, int count, string updateJqId, string callbackurl)
        {

            var pagination = new Pagination();
            pagination.TotleCount = count;
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageSize;
            pagination.JqUpdateElement = updateJqId;
            pagination.CallBackUrl = callbackurl;
            var str = (html.ViewContext.Controller as Controller).RenderPartialViewToString("_Pagination", pagination);
            return MvcHtmlString.Create(str);
        }

        //public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        //{
        //    return LabelFor(html, expression, new RouteValueDictionary(htmlAttributes));
        //}

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder tag = new TagBuilder("label");
            tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(labelText);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static string GetPropertyName<TModel>(this HtmlHelper<TModel> htmlHelper,
  Expression<Func<TModel, object>> expression)
        {
            var operant = (MemberExpression)((UnaryExpression)expression.Body).Operand;
            return operant.Member.Name;
        }

        public static MvcHtmlString B<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
  Expression<Func<TModel, TValue>> expression, object attributes = null)
        {
            // Create tag builder
            var builder = new TagBuilder("b");
            var value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
            builder.InnerHtml = value.ToString();
            builder.MergeAttribute("id", ExpressionHelper.GetExpressionText(expression));
            builder.MergeAttributes(new RouteValueDictionary(attributes), false);
            // Render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        public static HelperResult Once(this HtmlHelper html, string key, Func<object, HelperResult> template)
        {
            var httpContextItems = html.ViewContext.HttpContext.Items;
            var contextKey = "HtmlUtils.Once." + key;
            if (!httpContextItems.Contains(contextKey))
            {
                httpContextItems.Add(contextKey, null);
                return template(null);
            }
            else
            {
                return new HelperResult(writer => { });
            }
        }

        private static readonly SelectListItem[] EmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };
        private static readonly SelectListItem[] AllItem = new[] { new SelectListItem { Text = "所有", Value = "" } };

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes, SelectListItem selectItem = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = GetNonNullableModelType(metadata);
            var values = Enum.GetValues(enumType).Cast<TEnum>();


            var items = (from value in values
                         select new SelectListItem
                         {
                             Text = GetEnumDescription(value),
                             Value = value.ToString(),
                             Selected = value.Equals(metadata.Model)
                         }).ToList();
            if (selectItem != null)
            {
                items.Insert(0, selectItem);
            }


            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }
        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }
        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static MvcHtmlString BasicCheckBoxFor<T>(this HtmlHelper<T> html,
                                                Expression<Func<T, bool>> expression,
                                                object htmlAttributes = null)
        {
            var result = html.CheckBoxFor(expression).ToString();
            const string pattern = @"<input name=""[^""]+"" type=""hidden"" value=""false"" />";
            var single = Regex.Replace(result, pattern, "");
            return MvcHtmlString.Create(single);
        }
    }


}