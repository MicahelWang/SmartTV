namespace YeahTVApi.Behavior
{
    using YeahTVApi.DomainModel;
    using YeahTVApi.EntityFrameworkRepository.Models;
    using YeahTVApiLibrary.Behavior;
    using YeahTVApiLibrary.EntityFrameworkRepository;
    using YeahTVApiLibrary.Infrastructure;

    public sealed class UnitOfWorkInterceptionBehavior : UnitOfWorkInterceptionBehaviorBase
    {
    
        protected override EFUnitOfWork CreateUnitOfWork()
        {
            return new EFUnitOfWork(new YeahTVContext(Constant.NameOrConnectionString));
        }
    }
}