namespace ServiceModelContrib.IoC.Unity
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    ///<summary>
    /// Unity extension to support proper hierarchical containers;
    /// instances created/resolved by the child container will be
    /// registered in the child container, even if the type is configured
    /// in the parent container.
    ///</summary>
    public class HierarchicalLifetimeExtension : UnityContainerExtension
    {
        /// <summary>
        /// Initializes the Unity extension.
        /// </summary>
        protected override void Initialize()
        {
            // Add to type mapping stage so that it runs before the lifetime stage
            Context.Strategies.AddNew<HierarchicalLifetimeStrategy>(UnityBuildStage.TypeMapping);
        }
    }
}