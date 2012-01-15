namespace ServiceModelContrib
{
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    ///<summary>
    ///</summary>
    public class SilverlightFaultMessageInspector : IDispatchMessageInspector
    {
        #region IDispatchMessageInspector Members

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param><param name="correlationState">The correlation object returned from the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)"/> method.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (reply.IsFault)
            {
                var property = new HttpResponseMessageProperty();
                // Here the response code is changed to 200.
                property.StatusCode = HttpStatusCode.OK;
                reply.Properties[HttpResponseMessageProperty.Name] = property;
            }
        }

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)"/> method.
        /// </returns>
        /// <param name="request">The request message.</param><param name="channel">The incoming channel.</param><param name="instanceContext">The current service instance.</param>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // Do nothing to the incoming message.
            return null;
        }

        #endregion
    }
}