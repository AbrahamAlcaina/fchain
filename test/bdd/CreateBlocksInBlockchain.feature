Feature: Create blocks in the blockchain
    In order to add transactions to the ledger
    As a Blockchain user
    I want to be able to create blocks in the blockchain

    Scenario: Add a new block to an empty ledger
        Given   I have an empty ledger
        When    I add a new <transaction>
        Then    The ledger only has the genesis block
    Examples:
        | transaction   |
        | tx1           |
        | tx2           |
    
    Scenario: Add a new block into a ledger
        Given   I have a ledger with the genesis block
        When    I add a new <transaction>
        Then    The ledger has a new block with the <transaction>
    Examples:
        | transaction   |
        | tx1           |
        | tx2           |

    Scenario: Add a new block into a ledger that already have one
        Given   I have a ledger with a one block
        When    I add a new <transaction>
        Then    The ledger has a new block with the <transaction>
    Examples:
        | transaction   |
        | tx1           |
        | tx2           |
    
    
    
    