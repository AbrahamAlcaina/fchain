module CreateBlocksInBlockchain

open Blockchain
open TickSpec
open Xunit
open FsUnit.Xunit

let mutable ledger = []
let now = System.DateTime.Now
let Fail () = Assert.True(false) 
let [<BeforeScenario>] Setup () =
    ledger <- []
let [<Given>] ``I have an empty ledger`` ()=
    ledger <- []
let [<When>] ``I add a new (.*)`` (tx) = 
    ledger <- addBlock ledger now [{someData = tx}]
let [<Then>] ``The ledger only has the genesis block`` () = 
    ledger |> should haveLength 1
    match ledger.Head with
        | Genesis g -> g.genesisHash |> should equal genesisHash
        | RegularBlock _ -> Fail ()
    
let  [<Given>] ``I have a non empty ledger`` ()= 
    ledger <- addBlock ledger now [{someData = ""}]
let [<Then>] ``The ledger has a new block with the (.*)`` (tx) =     
    match ledger.Head with
        | Genesis _ -> Fail ()
        | RegularBlock 
            {
                previousBlock = previousBlock ; 
                index = index; 
                timestamp = timestamp;
                transactions = transactions;
                hash = hash
            } ->    index |> should equal 1
                    previousBlock.hash |> should equal genesisHash
                    timestamp |> should equal now
                    hash |> should be Empty
                    transactions |> should haveLength 1
                    transactions.Head.someData |> should equal tx