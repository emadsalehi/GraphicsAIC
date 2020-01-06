using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class Game
{
    public GameInit Init { get; set; }
    public List<GameTurn> Turns { get; set; }
}

public class GameTurn
{
    public int TurnNum { get; set; }
    public List<TurnPlayer> PlayerTurnEvents { get; set; }
    public List<TurnAttack> TurnAttacks { get; set; }

}

public class TurnAttack
{
    public int AttackerId { get; set; }
    public int DefenderId { get; set; }
}

public class TurnPlayer
{
    public int PId { get; set; }
    public PlayerEvent TurnEvent { get; set; }
}

public class PlayerEvent
{
    public bool IsAlive { get; set; }
    public int Hp { get; set; }
    public int Ap { get; set; }
    public int[] Hand { get; set; }
    public List<PlayerUnit> Units { get; set; }
    public List<PlayerMapSpell> MapSpells { get; set; }
}

public class PlayerUnit
{
    public int Id { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public int Hp { get; set; }
    public int TypeId { get; set; }
    public string Level { get; set; }
}

public class PlayerMapSpell
{
    public int SpellId { get; set; }
    public MapSpellCenter Center { get; set; }
    public int Range { get; set; }
    public int TypeId { get; set; }
}

public class MapSpellCenter
{
    public int Row { get; set; }
    public int Col { get; set; }
}

public class GameInit
{
    public InitMap GraphicMap { get; set; }
    public int MaxAP { get; set; }
    public InitConstants Constants { get; set; }
}

public class InitConstants
{
    public int MaxHp { get; set; }
    public int ApThreshold { get; set; }
}

public class InitMap
{
    public int Row { get; set; }
    public int Col { get; set; }
    public List<InitKing> Kings { get; set; }
    public List<InitPath> Paths { get; set; }
}

public class InitKing
{
    public int Row { get; set; }
    public int Col { get; set; }
    public int PId { get; set; }
    public string Name { get; set; }
}

public class InitPath
{
    public int PathId { get; set; }
    public List<PathCell> Cells { get; set; }
}

public class PathCell
{
    public int Row { get; set; }
    public int Col { get; set; }
}

public class LogReader : MonoBehaviour
{

    public Game ReadLog()
    {
        Game gameLog;
        using (StreamReader r = new StreamReader("Assets/Scripts/Log/log.json"))
        {
            string json = r.ReadToEnd();
            gameLog = JsonConvert.DeserializeObject<Game>(json);
        }
        return gameLog;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
