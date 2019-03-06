using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AElf.Common;
using AElf.Kernel.Blockchain.Application;
using AElf.Kernel.ChainController.Application;
using AElf.Kernel.Node.Domain;
using AElf.Kernel.SmartContractExecution.Application;
using AElf.Kernel.TransactionPool.Infrastructure;
using AElf.Kernel.Types;
using Google.Protobuf;

namespace AElf.Kernel.Node.Application
{
    public class BlockchainNodeContextStartDto
    {
        public int ChainId { get; set; }

        public Transaction[] Transactions { get; set; }
    }

    public interface IBlockchainNodeContextService
    {
        Task<BlockchainNodeContext> StartAsync(BlockchainNodeContextStartDto dto);

        Task StopAsync(BlockchainNodeContext blockchainNodeContext);
    }


    //Maybe we should call it CSharpBlockchainNodeContextService, or we should spilt the logic depended on CSharp
    public class BlockchainNodeContextService : IBlockchainNodeContextService
    {
        private ITxHub _txHub;
        private IBlockchainService _blockchainService;
        private IChainCreationService _chainCreationService;
        private ISmartContractAddressUpdateService _smartContractAddressUpdateService;

        public BlockchainNodeContextService(
            IBlockchainService blockchainService, IChainCreationService chainCreationService, ITxHub txHub)
        {
            _blockchainService = blockchainService;
            _chainCreationService = chainCreationService;
            _txHub = txHub;
        }

        public async Task<BlockchainNodeContext> StartAsync(BlockchainNodeContextStartDto dto)
        {
            var context = new BlockchainNodeContext
            {
                ChainId = dto.ChainId,
                TxHub = _txHub,
            };
            var chain = await _blockchainService.GetChainAsync() ??
                        await _chainCreationService.CreateNewChainAsync(dto.Transactions);

            await _smartContractAddressUpdateService.UpdateSmartContractAddressesAsync(
                await _blockchainService.GetBlockHeaderByHashAsync(chain.BestChainHash));

            return context;
        }

        public async Task StopAsync(BlockchainNodeContext blockchainNodeContext)
        {
        }

        private byte[] ReadContractCode(string path)
        {
            return File.ReadAllBytes(Path.GetFullPath(path));
        }
    }
}