using DDD.ApplicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningApplicationServices.Commands
{
    public class MessageCommand<T>(T Message) : ICommand;
    
}
