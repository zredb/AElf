using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AElf.Kernel.SmartContract.Application;
using AElf.Kernel.SmartContract.Domain;
using AElf.Kernel.SmartContractExecution.Application;
using AElf.Types;

namespace AElf.Kernel.SmartContract.Parallel
{
    public class BlockExecutingWithParallelService: BlockExecutingService
    {
        public BlockExecutingWithParallelService(ILocalParallelTransactionExecutingService executingService,
            IBlockchainStateService blockchainStateService) : base(executingService, blockchainStateService)
        {
        }

        protected override async Task<Block> FillBlockAfterExecutionAsync(BlockHeader blockHeader, List<Transaction> transactions, ReturnSetCollection returnSetCollection)
        {
            var block = await base.FillBlockAfterExecutionAsync(blockHeader, transactions, returnSetCollection);
            if(returnSetCollection.Conflict.Count > 0)
            {
                await EventBus.PublishAsync(new ConflictingTransactionsFoundInParallelGroupsEvent(
                    block.Header, returnSetCollection.Executed.Concat(returnSetCollection.Unexecutable).ToList(),
                    returnSetCollection.Conflict
                ));
            }
            return block;
        }
    }
}