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