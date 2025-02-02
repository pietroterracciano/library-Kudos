﻿using System;
using Kudos.Serving.KaronteModule.Contexts;
using Kudos.Serving.KaronteModule.Enums;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Kudos.Serving.KaronteModule.Middlewares
{
    public abstract class
        AContexizedKaronteMiddleware<ContextType>
        : AKaronteMiddleware
        where ContextType : AKaronteChildContext
    {
        protected AContexizedKaronteMiddleware(ref RequestDelegate rd) : base(ref rd) { }

        protected override async Task<EKaronteBounce> OnBounceStart(KaronteContext kc)
        {
            ContextType ct = await OnContextFetch(kc);
            return await OnContextReceive(ct);
        }

        protected abstract Task<ContextType> OnContextFetch(KaronteContext kc);
        protected abstract Task<EKaronteBounce> OnContextReceive(ContextType ct);
    }
}