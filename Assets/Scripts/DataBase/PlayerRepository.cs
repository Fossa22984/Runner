using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DataBase
{
    public class PlayerRepository : MonoBehaviour
    {
        public Player Player { get; private set; }
        [SerializeField] private FirebaseDbContext _firebaseDbContext;

        public void Initialize(Player player)
        {
            Player = player;
        }

        public void SetBestScore(int score)
        {
            if (Player.Score.BestScore < score)
                Player.Score.BestScore = score;
        }

        public void SetBalance()
        {
            Player.Score.Balance++;
        }

        public async Task ChangeUserData()
        {
            await _firebaseDbContext.ChangeUserAsync(Player);
        }

        public async Task<List<Player>> GetLeaders()
        {
            var leaders = await _firebaseDbContext.GetAllUserAsync();
            return leaders.OrderByDescending(x => x.Score.BestScore).ToList();
        }
    }
}