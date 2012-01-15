namespace ServiceModelContrib
{
    using System.ServiceModel.Channels;

    ///<summary>
    ///</summary>
    public static class MessageExtensions
    {
        ///<summary>
        /// Creates a copy of the message.
        ///</summary>
        ///<param name="message">Message to copy.</param>
        ///<returns>A copy of the original message.</returns>
        public static Message Copy(this Message message)
        {
            MessageBuffer buffer = message.CreateBufferedCopy(int.MaxValue);
            return buffer.CreateMessage();
        }
    }
}