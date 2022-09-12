# BlockChainMaxFinder
Blockchain Maximum Reward Finder Using Merkle Root

Question : 

Under the Bitcoin protocol, a block can contain up to 1000000 bytes of transactions.
Miners can freely choose transactions to include in a block, and once they mine a block, they
earn the transaction fees for each transaction in the block and an additional 12.5 BTC as a
reward (as of May 2017).
Please find the maximum possible reward for creating a block using the following 12
transactions:
ID Size (byte) Fee (BTC)
1 57247 0.0887
2 98732 0.1856
3 134928 0.2307
4 77275 0.1522
5 29240 0.0532
6 15440 0.0250
7 70820 0.1409
8 139603 0.2541
9 63718 0.1147
10 143807 0.2660
11 190457 0.2933
12 40572 0.0686

Notes
A block cannot contain the same transaction more than once.
For simplicity, there are no dependencies between transactions, and any combination of
transactions can be included in a block.
Also for the sake of simplicity you may ignore the fact that blocks must always contain a
coinbase transaction.
Hint
Assuming the total size of a transaction that can be included in a block to be 500000 bytes
instead of 1000000 bytes, the answer is 13.4376 BTC.
