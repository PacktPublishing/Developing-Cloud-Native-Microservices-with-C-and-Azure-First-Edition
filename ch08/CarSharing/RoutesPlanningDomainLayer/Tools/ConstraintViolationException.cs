﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Tools
{
    public class ConstraintViolationException(Exception ex): Exception(ex.Message, ex);
    
}