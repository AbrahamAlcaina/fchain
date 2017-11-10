module CreateBlocksInBlockchain

open Blockchain
open TickSpec
open Xunit
open Blockchain

let mutable ledger = []
let now = System.DateTime.Now

let [<BeforeScenario>] Setup () =
    ledger <- []
let [<Given>] ``I have an empty ledger`` ()=
    ledger <- []
let [<When>] ``I add a new (.*)`` (tx) = 
    ledger <- addBlock ledger now [{someData = tx}]
let [<Then>] ``The ledger only has the genesis block`` () = 
    Assert.True((ledger.Length) = 1)
    match ledger.Head with
        | Genesis g -> Assert.True (g.genesisHash = genesisHash)
        | RegularBlock _ -> Assert.True(false)
    
let  [<Given>] ``I have a non empty ledger`` ()= 
    ledger <- addBlock ledger now [{someData = ""}]

let [<Then>] ``The ledger has a new block with the (.*)`` (tx) =     
    match ledger.Head with
        | Genesis _ -> Assert.True(false) 
        | RegularBlock 
            {
                previousBlock = previousBlock ; 
                index = index; 
                timestamp = timestamp;
                transactions = transactions;
                hash = hash
            } ->    Assert.Equal(index, 1)
                    Assert.Equal(previousBlock.hash, genesisHash)
                    Assert.Equal(timestamp, now)
                    Assert.Empty(hash)
                    Assert.Equal(transactions.Length, 1)
                    Assert.Equal(transactions.Head.someData, tx)
