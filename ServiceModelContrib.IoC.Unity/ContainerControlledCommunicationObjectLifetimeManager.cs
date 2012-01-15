namespace ServiceModelContrib.IoC.Unity
{
    using System;
    using System.ServiceModel;
    using Microsoft.Practices.Unity;

    ///<summary>
    /// A LifetimeManager that holds onto the <c>ICommunicationObject</c> (typically a WCF client channel proxy) given to it. 
    /// When the <c>ContainerControlledCommunicationObjectLifetimeManager</c> is disposed, the instance is first
    /// properly closed, then disposed (if needed).
    ///</summary>
    public class ContainerControlledCommunicationObjectLifetimeManager : SynchronizedLifetimeManager, IDisposable
    {
        private ICommunicationObject _communicationObject;

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Retrieve a value from the backing store associated with this Lifetime policy.
        /// </summary>
        /// <returns>the object desired, or null if no such object is currently stored.</returns>
        protected override object SynchronizedGetValue()
        {
            return _communicationObject;
        }

        /// <summary>
        /// Stores the given value into backing store for retrieval later.
        /// </summary>
        /// <param name="newValue">The object being stored.</param>
        protected override void SynchronizedSetValue(object newValue)
        {
            _communicationObject = newValue as ICommunicationObject;
            if (_communicationObject == null)
                throw new InvalidOperationException(
                    "newValue is not an ICommunicationObject. The ContainerControlledCommunicationObjectLifetimeManager is only meant to be used for WCF channels and other communication objects.");
        }

        ///<summary>
        /// Remove the given object from backing store.
        ///</summary>
        public override void RemoveValue()
        {
            Dispose();
        }

        /// <summary>
        /// Disposes the Lifetime manager an the attached communication object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_communicationObject != null)
            {
                ChannelHelper.ProperClose(_communicationObject);
                var disposable = _communicationObject as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
                _communicationObject = null;
            }
        }
    }
}