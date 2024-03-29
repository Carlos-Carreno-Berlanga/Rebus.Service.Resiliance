﻿using Microsoft.Extensions.Logging;
using Rebus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Consumer
{
    public class MSLoggerAdapter : ILog
    {
        private readonly ILogger _logger;

        public MSLoggerAdapter(ILogger logger)
        {
            _logger = logger;
        }

        public void Debug(string message, params object[] objs)
        {
            _logger.LogDebug(message, objs);
        }

        public void Info(string message, params object[] objs)
        {
            _logger.LogInformation(message, objs);
        }

        public void Warn(string message, params object[] objs)
        {
            _logger.LogWarning(message, objs);
        }

        public void Warn(Exception exception, string message, params object[] objs)
        {
            _logger.LogWarning(message, objs, exception);
        }

        public void Error(Exception exception, string message, params object[] objs)
        {
            _logger.LogError(message, objs, exception);
        }

        public void Error(string message, params object[] objs)
        {
            _logger.LogError(message, objs);
        }
    }
}
