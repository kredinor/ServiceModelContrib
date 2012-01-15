namespace ServiceModelContrib.IoC.Unity
{
    using System;
    using System.ServiceModel.Configuration;

    /// <summary>
    /// Extension Behavior Element to set up the use of the Unity Container behavior.
    /// </summary>
    public class UnityContainerBehaviorElement : BehaviorExtensionElement
    {
        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/>.</returns>
        public override Type BehaviorType
        {
            get { return typeof (UnityContainerServiceBehavior); }
        }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>The behavior extension.</returns>
        protected override object CreateBehavior()
        {
            return new UnityContainerServiceBehavior();
        }
    }
}