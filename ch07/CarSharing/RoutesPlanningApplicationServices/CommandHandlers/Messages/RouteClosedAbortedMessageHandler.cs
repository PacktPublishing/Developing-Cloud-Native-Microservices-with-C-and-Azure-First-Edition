using DDD.ApplicationLayer;
using DDD.DomainLayer;
using RoutesPlanningApplicationServices.Commands;
using RoutesPlanningDomainLayer.Models.Route;
using SharedMessages.RouteNegotiation;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoutesPlanningDomainLayer.Models.BasicTypes;

namespace RoutesPlanningApplicationServices.CommandHandlers.Messages
{
    internal class RouteClosedAbortedMessageHandler(
        IRouteOfferRepository repo,
        IUnitOfWork uow,
        EventMediator mediator
        ) : ICommandHandler<MessageCommand<RouteClosedAbortedMessage>>
    {
        public async Task HandleAsync(MessageCommand<RouteClosedAbortedMessage> command)
        {
            var message = command.Message;
            await uow.StartAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                var route = await repo.Get(message.RouteId);
                if (route is not null)
                {
                    if(!message.IsAborted)
                    {
                        if(route.Status != RouteStatus.Open)
                        {
                            await uow.RollbackAsync();
                            return;
                        }
                        else route.Close();
                    }
                    else
                    {
                        if(route.Status == RouteStatus.Aborted)
                        {
                            await uow.RollbackAsync();
                            return;
                        }
                        else route.Abort();
                    }
                    if (route.DomainEvents != null && route.DomainEvents.Count > 0)
                        mediator.Equals(route.DomainEvents);
                    await uow.SaveEntitiesAsync();
                    await uow.CommitAsync();
                }
                else
                {
                    await uow.RollbackAsync();
                    return;
                }
            }
            catch
            {
                await uow.RollbackAsync();
                throw;
            }
            
        }
    }
}
