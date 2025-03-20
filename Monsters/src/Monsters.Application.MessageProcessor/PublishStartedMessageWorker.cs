using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monsters.Application.MessageProcessor.Messages;

namespace Monsters.Application.MessageProcessor
{
    public class PublishStartedMessageWorker : IHostedLifecycleService
    {
        private readonly ILogger<PublishStartedMessageWorker> _logger;
        private readonly IBus _bus;
        private readonly IBusControl _busControl;

        public PublishStartedMessageWorker(
            ILogger<PublishStartedMessageWorker> logger,
            IBus bus,
            IBusControl busControl)
        {
            _logger = logger;
            _bus = bus;
            _busControl = busControl;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StartingAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task StartedAsync(CancellationToken cancellationToken)
        {
            await _busControl.WaitForHealthStatus(BusHealthStatus.Healthy, cancellationToken);
            await _bus.Publish(new MonstersMessageProcessorStarted(), cancellationToken);
            _logger.LogInformation("Monsters message processor started, and event published");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StoppingAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StoppedAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
