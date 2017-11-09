Feature: Create blocks in the blockchain
    In order to add transactions to the ledger
    As a Blockchain user
    I want to be able to create blocks in the blockchain

    Scenario: Add a new block to an empty ledger
        Given I have an empty ledger
        When I add a new transaction
        Then The ledger have a only the genesis block
    
    Scenario: Add a new block in to a ledger
        Given I have a non empty ledger
        When I add a new "transaction"
        Then The ledger have a new block with the "transaction"
    
    