using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

[Serializable]
public class Game
{
    public GameInit init;
    public List<EndGame> end;
    public List<GameTurn> turns;
}

[Serializable]
public class EndGame
{
    public int playerId;
    public int score;
}

[Serializable]
public class GameTurn
{
    public int turnNum;
    public List<TurnPlayer> playerTurnEvents;
    public List<TurnAttack> turnAttacks;

}

[Serializable]
public class TurnAttack
{
    public int attackerId;
    public int defenderId;
}

[Serializable]
public class TurnPlayer
{
    public int pId;
    public PlayerEvent turnEvent;
}

[Serializable]
public class PlayerEvent
{
    public bool isAlive;
    public int hp;
    public int ap;
    public int[] hand;
    public List<PlayerUnit> units;
    public List<PlayerMapSpell> mapSpells;
}

[Serializable]
public class PlayerUnit
{
    public int id;
    public int row;
    public int col;
    public int hp;
    public int typeId;
    public string level;
}

[Serializable]
public class PlayerMapSpell
{
    public int spellId;
    public List<int> unitIds;
    public int typeId;
}

[Serializable]
public class GameInit
{
    public InitMap graphicMap;
    public int maxAP;
    public List<InitConstants> baseUnits;
}

[Serializable]
public class InitConstants
{
    public int type;
    public int maxHp;
    public int ap;
}

[Serializable]
public class InitMap
{
    public int row;
    public int col;
    public List<InitKing> kings;
    public List<InitPath> paths;
}

[Serializable]
public class InitKing
{
    public int row;
    public int col;
    public int pId;
    public int hp;
    public string name;
}

[Serializable]
public class InitPath
{
    public int pathId;
    public List<PathCell> cells;
}

[Serializable]
public class PathCell
{
    public int row;
    public int col;
}

public class LogReader : MonoBehaviour
{

    public Game ReadLog()
    {
        Game gameLog;
        var logPath = PlayerPrefs.GetString("LogPath");
        using (var r = new StreamReader(logPath))
        {
            var json = r.ReadToEnd();
            gameLog = JsonUtility.FromJson<Game>(json);
        }
        return gameLog;
    }
}