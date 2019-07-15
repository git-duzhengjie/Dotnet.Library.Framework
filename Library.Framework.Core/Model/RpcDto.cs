namespace Library.Framework.Core.Model
{
    public class RpcDto
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RpcMessageClientDto Content { get; set; }
    }
}
