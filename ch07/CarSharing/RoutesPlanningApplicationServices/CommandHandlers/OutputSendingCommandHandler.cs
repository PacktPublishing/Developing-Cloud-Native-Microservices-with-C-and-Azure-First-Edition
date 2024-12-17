using DDD.ApplicationLayer;
using DDD.DomainLayer;
using RoutesPlanningApplicationServices.Commands;
using RoutesPlanningDomainLayer.Models.OutputQueue;
using RoutesPlanningDomainLayer.Models.Route;
using SharedMessages.RouteNegotiation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningApplicationServices.CommandHandlers
{
    internal class OutputSendingCommandHandler(
        IOutputQueueRepository repo,
        IUnitOfWork uow
        ): ICommandHandler<OutputSendingCommand<RouteExtensionProposalsMessage>>
    {
        public async Task HandleAsync(OutputSendingCommand<RouteExtensionProposalsMessage> command)
        {
            var aggregates =await repo.Take
                (command.BatchCount, command.RequeueDelay);
            if(aggregates.Count==0)
            {
                command.OutPutEmpty = true;
                return;
            }
            var allTasks = aggregates.Select(
                m => (m, command.Sender(m.GetMessage<RouteExtensionProposalsMessage>()!)))
                .ToDictionary(m => m.Item1!, m => m.Item2 );
            try
            {
                await Task.WhenAll(allTasks.Values.ToArray());
            }
            catch
            {

            }
            repo.Confirm(
                aggregates
                .Where(m => !allTasks[m].IsFaulted && !allTasks[m].IsFaulted)
                .Select(m => m.Id).ToArray());
            await uow.SaveEntitiesAsync();
        }
    }
}
