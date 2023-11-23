using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Authorization : MonoBehaviour
{
    [SerializeField] private FirebaseDbContext _firebaseDbContext;

    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;

    [SerializeField] private TMP_Text _errorMessage;

    public async void onClickLoginButton()
    {
        if (!CheckFields()) return;

        var checkResult = await CheckUserAsync(_email.text, _password.text);

        if (checkResult)
            gameObject.SetActive(false);
    }

    private async Task<bool> CheckUserAsync(string email, string password)
    {
        var user = await _firebaseDbContext.GetUserAsync(email);
        if (user == null)
        {
            _errorMessage.text = "This user does not exist";
            return false;
        }
        else if (!password.Equals(user.Password))
        {
            _errorMessage.text = "Incorrect password";
            return false;
        }
        else return true;
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
