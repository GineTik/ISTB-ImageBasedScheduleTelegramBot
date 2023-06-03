﻿namespace ISTB.Framework.BotApplication.Context
{
    public class UpdateContextAccessor
    {
        private static readonly AsyncLocal<UpdateContext> _executorContextCurrent = new AsyncLocal<UpdateContext>();

        public UpdateContext? UpdateContext
        {
            get
            {
                return _executorContextCurrent.Value;
            }
            set
            {
                if (value == null)
                    return;

                if (_executorContextCurrent.Value != null)
                    return;

                _executorContextCurrent.Value = value;
            }
        }

    }
}