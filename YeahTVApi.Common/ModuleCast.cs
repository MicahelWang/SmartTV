using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YeahTVApi.Common
{
    public class ModuleCast
    {
        private List<CastProperty> mProperties = new List<CastProperty>();

        static Dictionary<Type, Dictionary<Type, ModuleCast>> mCasters = new Dictionary<Type, Dictionary<Type, ModuleCast>>(256);

        private static Dictionary<Type, ModuleCast> GetModuleCast(Type sourceType)
        {
            Dictionary<Type, ModuleCast> result;
            lock (mCasters)
            {
                if (!mCasters.TryGetValue(sourceType, out result))
                {
                    result = new Dictionary<Type, ModuleCast>(8);
                    mCasters.Add(sourceType, result);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取要转换的当前转换类实例
        /// </summary>
        /// <param name="sourceType">要转换的源类型</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static ModuleCast GetCast(Type sourceType, Type targetType)
        {
            Dictionary<Type, ModuleCast> casts = GetModuleCast(sourceType);
            ModuleCast result;
            lock (casts)
            {
                if (!casts.TryGetValue(targetType, out result))
                {
                    result = new ModuleCast(sourceType, targetType);
                    casts.Add(targetType, result);
                }
            }
            return result;
        }

        /// <summary>
        /// 以两个要转换的类型作为构造函数，构造一个对应的转换类
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        public ModuleCast(Type sourceType, Type targetType)
        {
            PropertyInfo[] targetProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo sp in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (PropertyInfo tp in targetProperties)
                {
                    if (sp.Name == tp.Name && sp.PropertyType == tp.PropertyType)
                    {
                        CastProperty cp = new CastProperty();
                        cp.SourceProperty = new PropertyAccessorHandler(sp);
                        cp.TargetProperty = new PropertyAccessorHandler(tp);
                        mProperties.Add(cp);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 将源类型的属性值转换给目标类型同名的属性
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void Cast(object source, object target)
        {
            Cast(source, target, null);
        }

        /// <summary>
        /// 将源类型的属性值转换给目标类型同名的属性，排除要过滤的属性名称
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="filter">要过滤的属性名称</param>
        public void Cast(object source, object target, string[] filter)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");

            for (int i = 0; i < mProperties.Count; i++)
            {
                CastProperty cp = mProperties[i];

                if (cp.SourceProperty.Getter == null) continue;
                object value = cp.SourceProperty.Getter(source, null); //PropertyInfo.GetValue(source,null);
                if (cp.TargetProperty.Setter == null) continue;
                if (filter == null)
                    cp.TargetProperty.Setter(target, value, null);
                else if (!filter.Contains(cp.TargetProperty.PropertyName))
                    cp.TargetProperty.Setter(target, value, null);
            }
        }

        /// <summary>
        /// 转换对象
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        public static void CastObject<TSource, TTarget>(TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            ModuleCast.GetCast(typeof(TSource), typeof(TTarget)).Cast(source, target);
        }


        /// <summary>
        /// 转换属性对象
        /// </summary>
        public class CastProperty
        {
            public PropertyAccessorHandler SourceProperty
            {
                get;
                set;
            }

            public PropertyAccessorHandler TargetProperty
            {
                get;
                set;
            }
        }

        /// <summary>
        /// 属性访问器
        /// </summary>
        public class PropertyAccessorHandler
        {
            public PropertyAccessorHandler(PropertyInfo propInfo)
            {
                this.PropertyName = propInfo.Name;
                if (propInfo.CanRead)
                    this.Getter = propInfo.GetValue;

                if (propInfo.CanWrite)
                    this.Setter = propInfo.SetValue;
            }
            public string PropertyName { get; set; }
            public Func<object, object[], object> Getter { get; private set; }
            public Action<object, object, object[]> Setter { get; private set; }
        }
    }
}
