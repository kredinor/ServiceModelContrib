namespace ServiceModelContrib.IoC.Unity
{
    using Microsoft.Practices.ObjectBuilder2;

    ///<summary>
    ///</summary>
    public class HierarchicalLifetimeStrategy : BuilderStrategy
    {
        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        public override void PreBuildUp(IBuilderContext context)
        {
            var activeLifetime = context.PersistentPolicies.Get<ILifetimePolicy>(context.BuildKey);
            if (activeLifetime is HierarchicalLifetimeManager)
            {
                // did this come from the local container or the parent?                
                var localLifetime = context.PersistentPolicies.Get<ILifetimePolicy>(context.BuildKey, true);
                if (localLifetime == null)
                {
                    localLifetime = new HierarchicalLifetimeManager();
                    context.PersistentPolicies.Set(localLifetime, context.BuildKey);
                    context.Lifetime.Add(localLifetime);
                }
            }
        }
    }
}