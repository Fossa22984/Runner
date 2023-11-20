using System.Collections.Generic;


[System.Serializable]
public class PlayersData
{
    public List<Player> Players = new List<Player>();
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


[System.Serializable]
public class Score
{
    public int BestScore;
    public int Balance;

    public Score()
    {
        BestScore = 0;
        Balance = 0;
    }
    public Score(int bestScore, int balance)
    {
        BestScore = bestScore;
        Balance = balance;
    }
}