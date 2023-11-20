using System.Collections;
using UnityEngine;
using Firebase.Database;


public class RealtimeDatabaseTutorial : MonoBehaviour
{
    [SerializeField] private int coins;

    private DatabaseReference _databaseReference;

    // Start is called before the first frame update
    void Start()
    {
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveCoins()
    {
        //  _databaseReference.Child("UserData").Child("Coins").SetValueAsync(coins);

        PlayersData players = new PlayersData();
        players.Players.Add(new Player() { Email = "Email", Password = "Password", Name = "Name", Score = new Score() { Balance = 0, BestScore = 0 } });
        players.Players.Add(new Player() { Email = "Email1", Password = "Password1", Name = "Name1", Score = new Score() { Balance = 0, BestScore = 0 } });
        players.Players.Add(new Player() { Email = "Email2", Password = "Password2", Name = "Name2", Score = new Score() { Balance = 0, BestScore = 0 } });
        players.Players.Add(new Player() { Email = "Email3", Password = "Password3", Name = "Name3", Score = new Score() { Balance = 0, BestScore = 0 } });

        var json=JsonUtility.ToJson(players);

        _databaseReference.SetRawJsonValueAsync(json);
    }

    public void LoadCoins()
    {
        StartCoroutine(LoadData());
    }

    private IEnumerator LoadData()
    {
        var userMeta = _databaseReference.Child(nameof(PlayersData.Players)).GetValueAsync();

        yield return new WaitUntil(predicate: () => userMeta.IsCompleted);

        if (userMeta.Exception != null)
        {
            Debug.Log("UsersData error Exception");
        }
        else if (userMeta == null)
        {
            Debug.Log("UsersData=null");
        }
        else
        {
            DataSnapshot dataSnapshot = userMeta.Result;
            var res = dataSnapshot.Child("2").Child("Email").Value.ToString();
          // var ress = JsonUtility.FromJson<PlayersData>(res);
            Debug.Log(res);
        }
    }
}