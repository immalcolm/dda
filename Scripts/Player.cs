using System;
public class Player
{
    //define attributes
    //we use public access to allow our JSON utility to convert nicely
    //base attributes
    public string playerName;

    //stats attribute
    public int level;
    public int health;
    public int maxHealth;
    public int mana;
    public int agility;

    //misc attributes
    public string timeCreated;

    //empty constructor
    public Player(){}

    //constructor with parameters
    public Player(string playerName,
        int level = 1, int health = 10, int maxHealth = 10,
        int mana = 5, int agility = 5)
    {
        this.playerName = playerName;
        this.level = level;
        this.health = health;
        this.maxHealth = maxHealth;
        this.mana = mana;
        this.agility = agility;
        this.timeCreated = ConvertNowToTimeStamp();
    }

    //fancy string function to get the details nicely
    public string PrintPlayerDetails()
    {
        return "Player Name: " + this.playerName +
            "\tLevel: " + this.level +
            "\tHealth: " + this.health;
    }

    //define methods

    /// <summary>
    /// https://briancaos.wordpress.com/2022/02/24/c-datetime-to-unix-timestamps/
    /// </summary>
    /// <returns></returns>
    public string ConvertNowToTimeStamp()
    {
        DateTimeOffset dto = new DateTimeOffset(DateTime.UtcNow);
        // Get the unix timestamp in seconds
        return dto.ToUnixTimeSeconds().ToString();

        
    }


    public Boolean IsAlive()
    {
        if (health > 0)
            return true;

        return false;

    }
}
