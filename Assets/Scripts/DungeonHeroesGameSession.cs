using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// game name variants: Skull & Cloak, Dungeon of the Red Rose, Mirroes, One Dungeon Two Heroes, Dungeon of Skull & Cloack, You undead to me.

/* A game session */
public class DungeonHeroesGameSession : GameSession
{
    public int currentOpponentIndex = 0;

    public Hero playerHero
    {
        get
        {
            return playerArmy.hero;
        }
        set
        {
            playerArmy.hero = value;
        }
    }
    public Hero opponentHero
    {
        get
        {
            return opponentArmy.hero;
        }
        set
        {
            opponentArmy.hero = value;
        }
    }

    public Army playerArmy
    {
        get
        {
            return players[0].armies[0];
        }
        set
        {
            players[0].armies[0] = value;
        }
    }
    public Army opponentArmy
    {
        get
        {
            return players[1].armies[0];
        }
        set
        {
            players[1].armies[0] = value;
        }
    }


    public override void InitMap()
    {
        map = new Map()
        {
            name = "test"
        };

        map.playerSlots.Add(new PlayerSlot()
        {

        });
        map.playerSlots.Add(new PlayerSlot()
        {
            onlyAI = true
        });
    }


    public override void InitPlayers()
    {
        players = new List<Player>(2);

        Player player = new Player()
        {
            name = "Player",
            controlled_by = Player.Controller.Player
        };
        player.armies.Add(CreateStartingPlayerArmy());
        players.Add(player);

        player = new Player()
        {
            name = "AI",
            controlled_by = Player.Controller.AI
        };
        player.armies.Add(CreateOpponentArmy(0));
        players.Add(player);

        playerRelations = new List<List<Player.Relations>>(2);
        playerRelations.Add(new List<Player.Relations>(2));
        playerRelations.Add(new List<Player.Relations>(2));
        playerRelations[0][0] = Player.Relations.Ally;
        playerRelations[1][1] = Player.Relations.Ally;
        playerRelations[0][1] = Player.Relations.Enemy;
        playerRelations[1][0] = Player.Relations.Enemy;
    }


    public Army GetCurrentOpponentArmy()
    {
        return players[currentOpponentIndex + 1].armies[0];
    }


    public Army CreateStartingPlayerArmy()
    {
        Hero hero = new Hero();
        Army army = new Army(hero);

        army.units.Add(new Unit(G.game.unitTypes[0], 20)
        {

        });
        army.units.Add(new Unit(G.game.unitTypes[1], 10)
        {

        });

        return army;
    }


    public Army CreateOpponentArmy(int index)
    {
        if (index == 0)
        {
            Hero hero = new Hero();
            Army army = new Army(hero);
            army.units.Add(new Unit(G.game.unitTypes[0], 3)
            {

            });
            army.units.Add(new Unit(G.game.unitTypes[1], 1)
            {

            });
            army.units.Add(new Unit(G.game.unitTypes[0], 3)
            {

            });

            return army;
        }
        else
        {
            throw new NotImplementedException();
        }
    }


    Army GenerateRandomArmy(float cost)
    {
        int count = 0;
        float unitCost = 0;

        Army a = new Army(opponentHero);

        for (int i = 0; i < G.game.unitTypes.Length && cost > 0; i++)
        {
            unitCost = G.game.unitTypes[i].data.cost;
            count = UnityEngine.Random.Range(0, (int)(cost / G.game.unitTypes[i].data.cost));

            if (count != 0)
            {
                a.units.Add(new Unit(G.game.unitTypes[i], count));
            }

            cost -= count * unitCost;
        }

        return a;
    }


    public int FindFirstUnitIndexOfType(UnitType ut, List<Unit> units)
    {
        return units.FindIndex(u => u.type.name == ut.name);
    }
}