namespace YeahTVApi.Entity
{
    /// <summary>
    /// 功能结果
    /// </summary>
    public class FunResult : IFunResult
    {
        public int pageRecordCount = 0;
        /// <summary>
        /// 返回值 >=0 成功 小于 0 失败
        /// </summary>
        public int ResultType { get; set; }

        /// <summary>
        /// 返回信息，当Code 小于 0 时候，代表错误信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 表示执行结果成功还是失败
        /// </summary>
        /// <returns>True 代表包含一个成功结果 False 代表包含一个错误结果</returns>
        public bool isOk
        {
            get
            {
                return (ResultType >= 0);
            }
        }
        /// <summary>
        /// 清空功能结果
        /// </summary>
        public FunResult()
        {
            this.ResultType = 0;
            this.Message = string.Empty;
        }


        /// <summary>
        /// 功能结果为错误
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="code">错误代码</param>
        /// <returns>返回结果代码与结果信息</returns>
        public FunResult WithError(string message, int code = -1)
        {
            this.ResultType = code;
            this.Message = message;
            return this;
        }

        /// <summary>
        /// 功能结果为正确
        /// </summary>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码与空的结果信息</returns>
        public FunResult WithOk(int code = 0)
        {
            this.ResultType = code;
            this.Message = string.Empty;
            return this;
        }

    }
    
    /// <summary>
    /// 功能结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FunResult<T> : FunResult
    {

        /// <summary>
        /// 返回信息
        /// </summary>
        public T value { get; set; }

        /// <summary>
        /// 功能结果集为错误
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="code">错误代码</param>
        /// <returns>返回错误代码与错误信息</returns>
        public new FunResult<T> WithError(string message, int code = -1)
        {
            this.ResultType = code;
            this.Message = message;
            return this;
        }

        /// <summary>
        /// 功能结果集为正确
        /// </summary>
        /// <param name="val">结果集的数据</param>
        /// <param name="code">结果代码</param>
        /// <returns>返回结果代码与空的结果信息</returns>
        public FunResult<T> WithOk(T val, int code = 0)
        {
            this.ResultType = code;
            this.Message = string.Empty;
            value = val;
            return this;
        }
    }
}
