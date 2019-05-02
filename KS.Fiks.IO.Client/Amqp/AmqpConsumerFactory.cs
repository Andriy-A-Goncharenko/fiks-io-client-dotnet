using KS.Fiks.Crypto;
using KS.Fiks.IO.Client.Asic;
using KS.Fiks.IO.Client.Configuration;
using KS.Fiks.IO.Client.FileIO;
using KS.Fiks.IO.Client.Send;
using RabbitMQ.Client;

namespace KS.Fiks.IO.Client.Amqp
{
    internal class AmqpConsumerFactory : IAmqpConsumerFactory
    {
        private readonly IFileWriter _fileWriter;

        private readonly IAsicDecrypter _decrypter;

        private readonly ISendHandler _sendHandler;

        private readonly string _accountId;

        public AmqpConsumerFactory(ISendHandler sendHandler, AccountConfiguration accountConfiguration)
        {
            _fileWriter = new FileWriter();
            _decrypter = new AsicDecrypter(DecryptionService.Create(accountConfiguration.PrivateKey));
            _sendHandler = sendHandler;
            _accountId = accountConfiguration.AccountId;
        }

        public IAmqpReceiveConsumer CreateReceiveConsumer(IModel channel)
        {
            return new AmqpReceiveConsumer(channel, _fileWriter, _decrypter, _sendHandler, _accountId);
        }
    }
}