﻿using System;
using System.Threading.Tasks;
using AElf.CSharp.Core;
using Google.Protobuf;


namespace AElf.Kernel.SmartContract.ExecutionPluginForAcs8
{
    public class MethodStubFactory : IMethodStubFactory
    {
        private readonly IHostSmartContractBridgeContext _context;

        public MethodStubFactory(IHostSmartContractBridgeContext context)
        {
            _context = context;
        }

        public IMethodStub<TInput, TOutput> Create<TInput, TOutput>(Method<TInput, TOutput> method)
            where TInput : IMessage<TInput>, new() where TOutput : IMessage<TOutput>, new()
        {
            Task<IExecutionResult<TOutput>> SendAsync(TInput input)
            {
                throw new NotSupportedException();
            }

            var context = _context;

            Task<TOutput> CallAsync(TInput input)
            {
                return Task.FromResult(_context.Call<TOutput>(context.Self, method.Name,
                    input.ToByteString()));
            }

            return new MethodStub<TInput, TOutput>(method, SendAsync, CallAsync);
        }
    }
}