# What game?
- Title: After School
- openworld
- basically, just run or touch: 鬼ごっこ
- futher fully free game: the place you login if you are free and you want to play (or talk) with friends

# Test Briefly
## Database: 
### User Inf
| name | description        |
-|-
| UserID | user id |
| e-mail | e-mail           |
| name  | name              |
| password   | password     |
| session key|  |

## In Game 
### Player Inf in Game  
| name | description |
-|-
| name     | display name       |
| position |  |

<!--
| HP   | hp                |
| MHP  | max hp            |
| SP   | stamina point     |
| MSP  | max stamina point |
| MP   | magic point       |
| MMP  | max magic point |
| MP   | magic point       |
| ATK  | attack power      |
| DEF  | defend power      |
-->


<!--


# total structure

|Sever               |multiple? |associated |description| 
-|-|-|-
|TotalServer         |1    |HasOne(mysql_1),  | UserAccountManagement: userID, Email, etc 
|GameManagementServer|1    |HasMany(GameDedicatedServer), HasOne(mysql_1), |
|GameDedicatedServer_1|multi|BelongsTo(GameManagementServer),  | Normal Move Control
|GameDedicatedServer_2|multi|BelongsTo(GameManagementServer),  | Battle Game match

|Database            |multiple? |description| 
-|-|-
|MYSQL_1 |1    |UserAccount: user ID, email, etc 
|REDIS_1 |1    |Session Management, GameDedicatedServerManagement (where userID ** is linked to dedicated server **?

# GameDedicatedServer 
## HOW ASSIGNED? 
- Assigned by [GameManagementServer](#GameManagementServer)
- Rules

|Rule| Short | Detailed |
-|-|-
| 1  |Position| Default: maximum distance is 200? (Allow flexibility is distance 50) (given that player's velocity is about 1m/s) |
| 2  |HowMany | Default: maximum number is 30? |

# Player Status (e.g.)
| name | description |
-|-
| TYPE | what monster?         |
| HP   | hp                |
| SP   | stamina point     |
| MP   | magic point       |
| MP   | magic point       |
| ATK  | attack power      |
| DEF  | defend power      |
| MHP  | max hp            |
| MSP  | max stamina point |
| MMP  | max magic point   |
| MATK | max attack power  |
| MDEF | max defend power  |

# Action Status (e.g.)
| name | description |
-|-
|  AttackBytes | byte attack |
|  AttackTail  | tail attack |

# Communicating Data
- request
```
{
    "session": {
        "user_id": "",
        "session_id": "",
        "when": "DATETIME",
    },
    "transform": {
        "position": {
            "x": "",
            "y": "",
            "z": "",
        },
        "rotation": {
            "x": "",
            "y": "",
            "z": "",
        },
    },
    "action": {
        "type": "1",
    },

}
```

- response
```
[
    "user1": { 
        "session": {
            "user_id": "",
            "when": "DATETIME",
        },
        "status": {
            "hp": ,
            "sp": ,
            "mp": ,
        },
        "transform": {
            "position": {
                "x": "",
                "y": "",
                "z": "",
            },
            "rotation": {
                "x": "",
                "y": "",
                "z": "",
            },
        },
        "action": {
            "type": "1",
        },
    
    },
    "user2": {

    },
    "user2": {

    },
    "user2": {

    },
]
```

-->