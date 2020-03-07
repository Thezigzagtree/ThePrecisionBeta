using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class RegistrationScript : MonoBehaviour
{
    
    public Text loggedInUser;
    public Text invalidEmailText;
    public Text invalidPWDText;
    public InputField emailField;
    public InputField passwordField;
    public InputField verifyField;

    public InputField Login_emailField;
    public InputField Login_passwordField;

    private string email;
    private string password;
    private string verifyPWD;
    
    public  const string MatchEmailPattern =
		@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
		+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
		+ @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
		+ @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public static bool validateEmail (string email)
	{
		if (email != null)
			return Regex.IsMatch (email, MatchEmailPattern);
		else
			return false;
	}

    public void confirmEmailCorrect()
    {
        email = emailField.text;
        if (validateEmail(email))
        {
            invalidEmailText.GetComponentInChildren<Image>().enabled = true;
            invalidEmailText.GetComponent<Text>().enabled = false;
        }

        else
        {
            invalidEmailText.GetComponentInChildren<Image>().enabled = false;
            invalidEmailText.GetComponent<Text>().enabled = true;
        }
    }
    
    public void confirmPWDCorrect()
    {
        password = passwordField.text;
        verifyPWD = verifyField.text;

        validatePWD(password, verifyPWD);
       
    }

public bool validatePWD(string pw1, string pw2)
{
        if(pw1 != pw2)
    {
        invalidPWDText.GetComponentInChildren<Image>().enabled = false;
        invalidPWDText.GetComponent<Text>().enabled = true;
        return false;
    }
    else
    {
        invalidPWDText.GetComponentInChildren<Image>().enabled = true;
        invalidPWDText.GetComponent<Text>().enabled = false;
        return true;
    }

}


    public void LoginCapture()
    {
        email = Login_emailField.text;
        password = Login_passwordField.text;

        FindObjectOfType<FirebaseObject>().SignUserIn(email, password);

    }


    public void Capture()
    {

        if (!validateEmail(email) || !validatePWD(password, verifyPWD))
            return;

        FindObjectOfType<FirebaseObject>().RegisterUser(email, password);
        
        
    }
    
    public void hideInvalidEmail()
    {
        invalidEmailText.GetComponent<Text>().enabled = false;
    }

    public void checkPasswordVerified()
    {

    }

    public void registrationButton()
    {

    }
}
