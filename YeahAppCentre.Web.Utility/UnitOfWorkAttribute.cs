namespace YeahTVApiLibrary.EntityFrameworkRepository
{
    using System;

    /// <summary>
    /// Represents a attribute for transactional jobs for the target method 
    /// consuming respository methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute
    {
    }
}
