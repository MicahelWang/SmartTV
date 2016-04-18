namespace HZTVApi.Handler
{
    using Microsoft.Practices.Unity.InterceptionExtension;
    using System;

    public class LogHandler : ICallHandler
    {
        public int Order { get; set; }//这是ICallHandler的成员，表示执行顺序
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            Console.WriteLine("方法名: {0}", input.MethodBase.Name);
            Console.WriteLine("参数:");
            for (var i = 0; i < input.Arguments.Count; i++)
            {
                Console.WriteLine("{0}: {1}", input.Arguments.ParameterName(i), input.Arguments[i]);
            }
            Console.WriteLine("执行");
            //这之前插入方法执行前的处理
            var retvalue = getNext()(input, getNext);//在这里执行方法
            //这之后插入方法执行后的处理
            Console.WriteLine("完成");
            return retvalue;
        }
    }
}