using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AElf.Common;
using AElf.Kernel.KernelAccount;
using AElf.Kernel.SmartContract.Application;
using AElf.Kernel.SmartContract.Infrastructure;
using AElf.Types.CSharp;
using Google.Protobuf;

namespace AElf.Kernel.SmartContractExecution.Application
{
    public interface ISmartContractAddressUpdateService
    {
        Task UpdateSmartContractAddressesAsync(BlockHeader blockHeader);
    }

    public class SmartContractAddressUpdateService : ISmartContractAddressUpdateService
    {
        private readonly ITransactionExecutingService _transactionExecutingService;
        private readonly IDefaultContractZeroCodeProvider _defaultContractZeroCodeProvider;

        private readonly IEnumerable<ISmartContractAddressNameProvider> _smartContractAddressNameProviders;

        private readonly ISmartContractAddressService _smartContractAddressService;

        public SmartContractAddressUpdateService(ITransactionExecutingService transactionExecutingService,
            IDefaultContractZeroCodeProvider defaultContractZeroCodeProvider,
            IEnumerable<ISmartContractAddressNameProvider> smartContractAddressNameProviders,
            ISmartContractAddressService smartContractAddressService)
        {
            _transactionExecutingService = transactionExecutingService;
            _defaultContractZeroCodeProvider = defaultContractZeroCodeProvider;
            _smartContractAddressNameProviders = smartContractAddressNameProviders;
            _smartContractAddressService = smartContractAddressService;
        }

        public async Task UpdateSmartContractAddressesAsync(BlockHeader blockHeader)
        {
            foreach (var smartContractAddressNameProvider in _smartContractAddressNameProviders)
            {
                await UpdateSmartContractAddressesAsync(blockHeader, smartContractAddressNameProvider);
            }
        }

        private async Task UpdateSmartContractAddressesAsync(BlockHeader blockHeader,
            ISmartContractAddressNameProvider smartContractAddressNameProvider)
        {
            var t = new Transaction()
            {
                From = _defaultContractZeroCodeProvider.ContractZeroAddress,
                To = _defaultContractZeroCodeProvider.ContractZeroAddress,
                MethodName = nameof(ISmartContractZero.GetContractAddressByName),
                Params = ByteString.CopyFrom(ParamsPacker.Pack(smartContractAddressNameProvider.ContractName))
            };

            var transactionResult = (await _transactionExecutingService.ExecuteAsync(blockHeader,
                new List<Transaction> {t},
                CancellationToken.None)).First();

            var address = transactionResult.ReturnValue.DeserializeToPbMessage<Address>();

            _smartContractAddressService.SetAddress(smartContractAddressNameProvider.ContractName, address);
        }
    }
}