namespace YeahTVApiLibrary.EntityFrameworkRepository
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using System.Linq;
    using System.Reflection;

    public class UnitOfWorkHelper
    {

        public static bool IsDBConnectionRequired(MethodBase methodInfo)
        {
            bool required = false;
            required = IsRepositoryMethod(methodInfo) || IsUnitOfWorkAttributed(methodInfo);
            return required;
        }

        /// <summary>
        /// check whether the specified method is from a repository class
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static bool IsRepositoryMethod(MethodBase methodInfo)
        {
            return methodInfo.DeclaringType.Name.Contains("Repertory")
                || methodInfo.DeclaringType.Name.Contains("Repository");
        }

        /// <summary>
        /// check whether the method has UnitOfWork attribute
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static bool IsUnitOfWorkAttributed(MethodBase methodInfo)
        {
            return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        }

        public static bool IsCacheAttributed(MethodBase methodInfo)
        {
            return methodInfo.IsDefined(typeof(CacheAttribute), true);
        }

        public static bool NeedTransaction(MethodBase methodInfo)
        {
            return Constant.NeedTransactionMethodNames.Any(n => n.Contains(methodInfo.Name)) || IsUnitOfWorkAttributed(methodInfo);
        }
    }
}
