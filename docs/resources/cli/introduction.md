# Introduction to the CLI


The **aelf-command** tool is a CLI tools built for interacting with an AElf node. This section will walk you through some of the most commonly used features and show you how to install the tool.

## Features

* Get or Set common configs, `endpoint`, `account`, `datadir`, `password`.
* For new learners who are not familiar with the CLI parameters, any lacked parameters will be asked in a prompting way.
* Create a new `account`.
* Load a account by a given `private key` or `mnemonic`.
* Show `wallet` details which include private key, address, public key and mnemonic.
* Encrypt account info into `keyStore` format and save into files.
* Get current `Best Height` of the chain.
* Get `block info` by a given `height` or `block hash`.
* Get `transaction result` by a given `transaction id`.
* Send a `transaction` or call a `read-only method` on a smart `contract`.
* Deploy a smart `contract`.
* Open a `REPL` for using `JavaScript` to interact with the chain.
* Friendly interact, beautify with chalk & ora.
* Get current chain status.
* Create a proposal on any contract method.

## Installing aelf-command

```bash
$ npm i aelf-command -g
```

## Using aelf-command

### First Step

You need to create a new account or load a account by a `private key` or `mnemonic` you already have.

* Create a new wallet

```bash
$ aelf-command create

Your wallet info is :
Mnemonic            : great mushroom loan crisp ... door juice embrace
Private Key         : e038eea7e151eb451ba2901f7...b08ba5b76d8f288
Public Key          : 0478903d96aa2c8c0...6a3e7d810cacd136117ea7b13d2c9337e1ec88288111955b76ea
Address             : 2Ue31YTuB5Szy7cnr3SCEGU2gtGi5uMQBYarYUR5oGin1sys6H
✔ Save account info into a file? … no / yes
✔ Enter a password … ********
✔ Confirm password … ********
✔
Account info has been saved to "/Users/young/.local/share/aelf/keys/2Ue31YTuB5Szy7cnr...Gi5uMQBYarYUR5oGin1sys6H.json"

```

* Load wallet from private key
```bash
$ aelf-command load e038eea7e151eb451ba2901f7...b08ba5b76d8f288

Your wallet info is :
Private Key         : e038eea7e151eb451ba2901f7...b08ba5b76d8f288
Public Key          : 0478903d96aa2c8c0...6a3e7d810cacd136117ea7b13d2c9337e1ec88288111955b76ea
Address             : 2Ue31YTuB5Szy7cnr3SCEGU2gtGi5uMQBYarYUR5oGin1sys6H
✔ Save account info into a file? … no / yes
✔ Enter a password … ********
✔ Confirm password … ********
✔
Account info has been saved to "/Users/young/.local/share/aelf/keys/2Ue31YTuB5Szy7cnr...Gi5uMQBYarYUR5oGin1sys6H.json"
```

* show wallet info you already have
```bash
$ aelf-command wallet -a 2Ue31YTuB5Szy7cnr3SCEGU2gtGi5uMQBYarYUR5oGin1sys6H
Your wallet info is :
Private Key         : e038eea7e151eb451ba2901f7...b08ba5b76d8f288
Public Key          : 0478903d96aa2c8c0...6a3e7d810cacd136117ea7b13d2c9337e1ec88288111955b76ea
Address             : 2Ue31YTuB5Szy7cnr3SCEGU2gtGi5uMQBYarYUR5oGin1sys6H
```

Here you can get the account info and decide whether to encrypt account info and save into a file.

Examples:
```bash
$ aelf-command console -a 2Ue31YTuB5Szy7cnr3SCEGU2gtGi5uMQBYarYUR5oGin1sys6H
✔ Enter the password you typed when creating a wallet … ********
✔ Succeed!
Welcome to aelf interactive console. Ctrl + C to terminate the program. Double tap Tab to list objects

   ╔═══════════════════════════════════════════════════════════╗
   ║                                                           ║
   ║   NAME       | DESCRIPTION                                ║
   ║   AElf       | imported from aelf-sdk                     ║
   ║   aelf       | the instance of an aelf-sdk, connect to    ║
   ║              | http://127.0.0.1:8000                  ║
   ║   _account   | the instance of an AElf wallet, address    ║
   ║              | is                                         ║
   ║              | 2Ue31YTuB5Szy7cnr3SCEGU2gtGi5uMQBYarYUR…   ║
   ║              | 5oGin1sys6H                                ║
   ║                                                           ║
   ╚═══════════════════════════════════════════════════════════╝
```

Any missed parameters you did not give in CLI parameters will be asked in a prompting way
```bash
$ aelf-command console
✔ Enter a valid wallet address, if you don't have, create one by aelf-command create … 2Ue31YTuB5Szy7cnr3SCEGU2gtGi5uMQBYarYUR5oGin1sys6H
✔ Enter the password you typed when creating a wallet … ********
✔ Succeed!
Welcome to aelf interactive console. Ctrl + C to terminate the program. Double tap Tab to list objects

   ╔═══════════════════════════════════════════════════════════╗
   ║                                                           ║
   ║   NAME       | DESCRIPTION                                ║
   ║   AElf       | imported from aelf-sdk                     ║
   ║   aelf       | the instance of an aelf-sdk, connect to    ║
   ║              | http://13.231.179.27:8000                  ║
   ║   _account   | the instance of an AElf wallet, address    ║
   ║              | is                                         ║
   ║              | 2Ue31YTuB5Szy7cnr3SCEGU2gtGi5uMQBYarYUR…   ║
   ║              | 5oGin1sys6H                                ║
   ║                                                           ║
   ╚═══════════════════════════════════════════════════════════╝

```


### Help

Type
```bash
$ aelf-command -h

Usage: aelf-command [command] [options]

Options:
  -v, --version                                            output the version number
  -e, --endpoint <URI>                                     The URI of an AElf node. Eg: http://127.0.0.1:8000
  -a, --account <account>                                  The address of AElf wallet
  -p, --password <password>                                The password of encrypted keyStore
  -d, --datadir <directory>                                The directory that contains the AElf related files. Default to be Default to be `{home}/.local/share/aelf`
  -h, --help                                               output usage information

Commands:
  call [contract-address|contract-name] [method] [params]  Call a read-only method on a contract.
  send [contract-address|contract-name] [method] [params]  Execute a method on a contract.
  get-blk-height                                           Get the current block height of specified chain
  get-chain-status                                         Get the current chain status
  get-blk-info [height|block-hash] [include-txs]           Get a block info
  get-tx-result [tx-hash]                                  Get a transaction result
  console                                                  Open a node REPL
  create [options] [save-to-file]                          Create a new account
  wallet                                                   Show wallet details which include private key, address, public key and mnemonic
  load [private-key|mnemonic] [save-to-file]               Load wallet from a private key or mnemonic
  proposal [organization] [expired-time]                   Send a proposal to an origination with a specific contract method
  deploy [category] [code-path]                            Deploy a smart contract
  config <flag> [key] [value]                              Get, set, delete or list aelf-command config

```
in your terminal and get useful information.

Any sub-commands such as `call`, you can get `help` by typing this
```bash
$ aelf-command call -h
$ aelf-command send -h
$ aelf-command console -h
...
```