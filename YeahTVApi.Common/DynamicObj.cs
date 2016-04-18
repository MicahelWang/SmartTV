using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Dynamic;
using System.ComponentModel;

namespace YeahTVApi.Common
{
    public class DynamicObj : DynamicObject, ICustomTypeDescriptor
    {
        //保存对象动态定义的属性值  
        private Dictionary<string, object> _values;
        public object this[string name]
        {
            get { return _values.ContainsKey(name) ? _values[name] : null; }
            set { _values[name] = value; }
        }

        public DynamicObj()
        {
            _values = new Dictionary<string, object>();
        }
        /// <summary>  
        /// 获取属性值  
        /// </summary>  
        /// <param name="propertyName"></param>  
        /// <returns></returns>  
        public object GetPropertyValue(string propertyName)
        {
            if (_values.ContainsKey(propertyName) == true)
            {
                return _values[propertyName];
            }
            return null;
        }
        /// <summary>  
        /// 设置属性值  
        /// </summary>  
        /// <param name="propertyName"></param>  
        /// <param name="value"></param>  
        public void SetPropertyValue(string propertyName, object value)
        {
            if (_values.ContainsKey(propertyName) == true)
            {
                _values[propertyName] = value;
            }
            else
            {
                _values.Add(propertyName, value);
            }
        }
        /// <summary>  
        /// 实现动态对象属性成员访问的方法，得到返回指定属性的值  
        /// </summary>  
        /// <param name="binder"></param>  
        /// <param name="result"></param>  
        /// <returns></returns>  
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetPropertyValue(binder.Name);
            return result == null ? false : true;
        }
        /// <summary>  
        /// 实现动态对象属性值设置的方法。  
        /// </summary>  
        /// <param name="binder"></param>  
        /// <param name="value"></param>  
        /// <returns></returns>  
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetPropertyValue(binder.Name, value);
            return true;
        }
        /// <summary>  
        /// 动态对象动态方法调用时执行的实际代码  
        /// </summary>  
        /// <param name="binder"></param>  
        /// <param name="args"></param>  
        /// <param name="result"></param>  
        /// <returns></returns>  
        /*
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var theDelegateObj = GetPropertyValue(binder.Name) as DelegateObj;
            if (theDelegateObj == null || theDelegateObj.CallMethod == null)
            {
                result = null;
                return false;
            }
            result = theDelegateObj.CallMethod(this, args);
            return true;
        }*/
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return base.TryInvoke(binder, args, out result);
        }

        #region DynamicObjDescriptor
        class DynamicObjDescriptor : PropertyDescriptor
        {
            public DynamicObjDescriptor(string name) : base(name, null) { }
            public override bool CanResetValue(object component)
            {
                return true;
            }
            public override Type ComponentType
            {
                get { return typeof(DynamicObj); }
            }
            public override object GetValue(object component)
            {
                return ((DynamicObj)component)[Name];
            }
            public override void SetValue(object component, object value)
            {
                ((DynamicObj)component)[Name] = value;
            }
            public override bool IsReadOnly
            {
                get { return false; }
            }
            public override Type PropertyType
            {
                get { return typeof(object); }
            }
            public override void ResetValue(object component)
            {
                ((DynamicObj)component)._values.Remove(Name);
            }
            public override bool ShouldSerializeValue(object component)
            {
                return ((DynamicObj)component)._values.ContainsKey(Name);
            }
        }
        #endregion

        #region ICustomTypeDescriptor Members
        public AttributeCollection GetAttributes()
        {
            return AttributeCollection.Empty;
        }
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }
        public TypeConverter GetConverter()
        {
            return null;
        }
        public EventDescriptor GetDefaultEvent()
        {
            return null;
        }
        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }
        public object GetEditor(Type editorBaseType)
        {
            return null;
        }
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return EventDescriptorCollection.Empty;
        }
        public EventDescriptorCollection GetEvents()
        {
            return EventDescriptorCollection.Empty;
        }
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(
              _values.Keys.Select(key => new DynamicObjDescriptor(key)).ToArray());
        }
        public PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
        #endregion
     }  

}
