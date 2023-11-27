using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseDbContext : MonoBehaviour
{
    private DatabaseReference _databaseReference;

    void Start()
    {
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async Task<bool> AddUserAsync(Player player)
    {
        var user = await GetUserAsync(player.Email);
        if (user != null) { return false; }

        var json = JsonUtility.ToJson(player);
        await _databaseReference.Child(nameof(PlayersData.Players)).Child(player.Email).SetRawJsonValueAsync(json);

        return true;
    }

    public async Task<bool> ChangeUserAsync(Player player)
    {
        var user = await GetUserAsync(player.Email);
        if (user == null) { return false; }

        var json = JsonUtility.ToJson(player);
        await _databaseReference.Child(nameof(PlayersData.Players)).Child(player.Email).SetRawJsonValueAsync(json);

        return true;
    }

    public async Task<Player> GetUserAsync(string email)
    {
        DataSnapshot snapshot = await _databaseReference.Child(nameof(PlayersData.Players)).Child(email).GetValueAsync();

        if (snapshot.Exists)
        {
            return JsonUtility.FromJson<Player>(snapshot.GetRawJsonValue());
        }
        else
        {
            Debug.LogWarning("Node does not exist.");
            return null;
        }
    }

    public async Task<List<Player>> GetAllUserAsync()
    {
        DataSnapshot snapshot = await _databaseReference.Child(nameof(PlayersData.Players)).GetValueAsync();
        if (snapshot.Exists)
        {
            var players = new List<Player>();
            foreach (var childSnapshot in snapshot.Children)
            {
                Player player = JsonUtility.FromJson<Player>(childSnapshot.GetRawJsonValue());
                players.Add(player);
            }

            return players;
        }
        else
        {
            Debug.LogWarning("Node does not exist.");
            return null;
        }
    }
}
