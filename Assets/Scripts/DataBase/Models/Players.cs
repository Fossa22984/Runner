using System.Collections.Generic;


[System.Serializable]
public class PlayersData
{
    public List<Player> Players;

    public PlayersData() { Players = new List<Player>(); }
}


[System.Serializable]
public class Player
{
    public string Name;
    public string Email;
    public string Password;
    public Score Score;

    public Player() { }
    public Player(string name, string email, string password, Score score)
    {
        Name = name;
        Email = email;
        Password = password;
        Score = score;
    }
}