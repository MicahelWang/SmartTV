using System.ComponentModel;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 表示业务操作结果的枚举
    /// </summary>
    /// todo:提供的任何action应遵循如下校验规则
    /// 1.用户登录状态
    /// 2.用户权限
    /// 3.必填参数缺失或为空
    /// 4.参数类型或格式
    /// 5.在返回成功数据前,将Data属性中的值/对象进行MD5码 与 传入的MD5 匹配，一致则无需返回Data,将此枚举设置为NoChange返回
    public enum OperationResultType
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        Successed = 0,

        /// <summary>
        /// 操作没有引发任何变化
        /// </summary>
        NoChanged = 1,

        /// <summary>
        /// 操作失败，详见错误信息
        /// </summary>
        Failed = 2,

        /// <summary>
        /// 需要登录后才可访问
        /// </summary>
        NeedLogin = 3,

        /// <summary>
        /// 当前用户权限不足，不能继续操作
        /// </summary>
        PurviewLack = 31,

        /// <summary>
        /// 指定参数的数据不存在
        /// </summary>
        ParamIsNull = 4,

        /// <summary>
        /// 参数类型或格式
        /// </summary>
        ParamError = 41,

        /// <summary>
        /// 非法操作
        /// </summary>
        IllegalOperation = 5,

        /// <summary>
        /// 操作引发错误
        /// </summary>
        Error = 6,

        /// <summary>
        /// 
        /// </summary>
        NeedDoubleCheck = 7,

        /// <summary>
        /// 
        /// </summary>
        DoubleCheckFailed = 71
    }
}
