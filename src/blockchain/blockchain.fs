module Blockchain
open System


type HashPoint = {
    index:int;
    hash:string
}

type Transaction =  {
    someData:string
}

type Block= 
    | RegularBlock of BasicBlock
    | Genesis of GenesisBlock 
and GenesisBlock = {
    genesisHash:string
}
and BasicBlock = {
    previousBlock:HashPoint; 
    index:int; 
    timestamp:DateTime;
    transactions:Transaction list;
    hash:string
} 

type Ledger = Ledger of Block list

let genesisBlock = Genesis {genesisHash = "75e13da2e9a446e01594ee3fda021abb1d8cfc11d8bda49735b692c5ef632285c3c937eb159e68cee47c9e53f6f721f0a4cf2099c4c01509f84de5aa38fdba79"}
let nextBlock previous time txs = 
    match previous with
        | Genesis { genesisHash = _genesisHash } -> RegularBlock {
               previousBlock = { index=0; hash=_genesisHash;};
               index = 1;
               timestamp = time;
               transactions = txs;
               hash = "";
            }
        | RegularBlock {hash=previousHash}   -> RegularBlock {
               previousBlock = { index=0; hash=previousHash;};
               index = 1;
               timestamp = time;
               transactions = txs;
               hash = "";
            }  
let addBlock ledger time txs = 
    match ledger with 
        | [] -> [genesisBlock]
        | previous::_ -> nextBlock previous time txs :: ledger