namespace ServiceModelContrib.IoC.Unity
{
    using Microsoft.Practices.Unity;

    ///<summary>
    /// Classes that inherit from this base class will get instantiated and ConfigureContainer will be called before the container is used.
    ///</summary>
    public abstract class UnityRegistry
    {
        ///<summary>
        ///</summary>
        ///<param name="container"></param>
        public abstract void ConfigureContainer(IUnityContainer container);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childContainer"></param>
        public virtual void ConfigureChildContainer(IUnityContainer childContainer)
        {
        }
    }
}