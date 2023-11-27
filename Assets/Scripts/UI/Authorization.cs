using Assets.Scripts.DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Authorization : MonoBehaviour
{
    [SerializeField] private FirebaseDbContext _firebaseDbContext;
    [SerializeField] private PlayerRepository _playerRepository;
    [SerializeField] private GameObject _menu;

    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;

    [SerializeField] private TMP_Text _errorMessage;

    public async void onClickLoginButton()
    {
        if (!CheckFields()) return;

        var checkResult = await CheckUserAsync(_email.text, _password.text);

        if (checkResult!=null)
        {
            _playerRepository.Initialize(checkResult);
            gameObject.SetActive(false);
            _menu.SetActive(true);
        }        
    }

    private async Task<Player> CheckUserAsync(string email, string password)
    {
        var user = await _firebaseDbContext.GetUserAsync(email);
        if (user == null)
        {
            _errorMessage.text = "This user does not exist";
            return null;
        }
        else if (!password.Equals(user.Password))
        {
            _errorMessage.text = "Incorrect password";
            return null;
        }
        else return user;
    }

    private bool CheckFields()
    {
        if (!_email.text.Equals("") & !_password.text.Equals(""))
        {
            _errorMessage.text = "";
            return true;
        }

        else _errorMessage.text = "Fill in all the fields";

        return false;
    }
}
