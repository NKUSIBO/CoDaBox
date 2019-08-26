using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Inocrea.CodaBox.Back.BackGround
{
    public class PeriodicBackgroundService : BackgroundService
    {
        private CancellationTokenSource _cts;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var coda = new CodaProcesse().Start();

                    stoppingToken.ThrowIfCancellationRequested();

                    if (_cts == null || _cts.Token.IsCancellationRequested)
                    {
                        _cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromDays(1), _cts.Token);
                    stoppingToken.ThrowIfCancellationRequested();

                }
                catch (OperationCanceledException)
                {
                    // Log errors
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
