using Firebase;
using Firebase.Analytics;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseObject : MonoBehaviour
{
    private FirebaseDatabase database;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    
    //FindObjectOfType<FirebaseObject>().GetAuth()

    void Awake()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;  
    
    }

    public Firebase.Auth.FirebaseAuth GetAuth()
    {
        return auth;
    }

    public Firebase.Auth.FirebaseUser GetUser()
    {
        return user;
    }


    private IEnumerator checkDependencies()
    {
        var checkTask = FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith( task =>
                {
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                });

        yield return new WaitUntil(() => checkTask.IsCompleted);
       
        database = FirebaseDatabase.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://thumber-3497786.firebaseio.com/");
        user = auth.CurrentUser;
		if (user != null)
        {
            FindObjectOfType<RegistrationScript>().loggedInUser.text = auth.CurrentUser.Email;
            //FindObjectOfType<MainscreenManager>().userLoggedIn();
            getPlayerInfo();

        }
			
		else
			Debug.Log("Nobody is signed In");
        
		
    }

    void Start()
    {
    StartCoroutine (checkDependencies());    
        
	}

    public void SignUserIn(string email, string password)
    {
        StartCoroutine(LoginUser(email, password));
    }

    public void RegisterUser(string email, string password)
    {
        StartCoroutine(RegisterNewUser(email, password));
    }


    private IEnumerator InitRegisterUser(string email, string password)
    {
    var signTask = auth.SignInWithEmailAndPasswordAsync(email, password);
    yield return new WaitUntil(() => signTask.IsCompleted);

    if(signTask.Exception != null)
    {
        Debug.Log("Failed To Sign In");
    }
        else
        {
        
        user = auth.CurrentUser;
        FindObjectOfType<RegistrationScript>().loggedInUser.text = auth.CurrentUser.Email;
        //FindObjectOfType<playerObj>().setupNewUser();
        InitsaveGame();
    //    FindObjectOfType<MainscreenManager>().userLoggedIn(); 
        // if(FindObjectOfType<MainscreenManager>().regFormAnimator.GetBool("ExpandRegForm"))
        // FindObjectOfType<MainscreenManager>().shrinkRegForm();
        // if(FindObjectOfType<MainscreenManager>().regFormAnimator.GetBool("ExpandLoginForm"))
        //     FindObjectOfType<MainscreenManager>().shrinkLoginForm();
        }

}

    private IEnumerator LoginUser(string email, string password)
{
   var signTask = auth.SignInWithEmailAndPasswordAsync(email, password);
   yield return new WaitUntil(() => signTask.IsCompleted);

   if(signTask.Exception != null)
   {
       Debug.Log("Failed To Sign In");
       
   }
    else
    {
            
    user = auth.CurrentUser;
    FindObjectOfType<RegistrationScript>().loggedInUser.text = auth.CurrentUser.Email;
    //FindObjectOfType<MainscreenManager>().userLoggedIn(); 
    getPlayerInfo();
    // if(FindObjectOfType<MainscreenManager>().regFormAnimator.GetBool("ExpandRegForm"))
    //     FindObjectOfType<MainscreenManager>().shrinkRegForm();
    // if(FindObjectOfType<MainscreenManager>().regFormAnimator.GetBool("ExpandLoginForm"))
    //     FindObjectOfType<MainscreenManager>().shrinkLoginForm();
        
    
    }
}
    
    private IEnumerator RegisterNewUser(string email, string password)
    {

        var RegTask = auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
             auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task2 => {
                Debug.Log("Signed In");
                Firebase.Auth.FirebaseUser newUser = task2.Result;
                
                //Debug.Log(newUser.UserId);
             });
             
        });
        yield return new WaitUntil(()=> RegTask.IsCompleted);

        if (RegTask.Exception != null)
        {
            Debug.Log("Failed To Register");
        }
        else
        {
            StartCoroutine(InitRegisterUser(email, password));
        }
    }
   
    

    
    
    
    public void getPlayerInfo()
    {

         database.RootReference.Child("users").Child(user.UserId).GetValueAsync().ContinueWith(task => {
             if(task.IsFaulted) {

             }
             else if(task.IsCompleted) 
             {
                 DataSnapshot snapshot = task.Result;

                 int sCredits = int.Parse(task.Result.Child("silverCredits").Value.ToString());
                 SaveSystem.SetInt("silverCredits", sCredits);
                 Debug.Log("SilverCredits are " + SaveSystem.GetInt("silverCredits"));
                 int credits = int.Parse(task.Result.Child("credit").Value.ToString());
                 SaveSystem.SetInt("credits", credits);
                 Debug.Log("Credits are " + SaveSystem.GetInt("credits"));
                 //int position = int.Parse(task.Result.Child("position").Value.ToString());
                 //SaveSystem.SetInt("position", position);
                 //Debug.Log("Position are " + SaveSystem.GetInt("position"));
                 string shape = task.Result.Child("currentAgentShape").Value.ToString();
                 SaveSystem.SetString("currentAgentShape", shape);
                 Debug.Log("Shape is  " + SaveSystem.GetString("currentAgentShape"));
                //  SaveSystem.obtainStageToRank(task.Result.Child("stages").GetRawJsonValue());
                //  SaveSystem.obtainItems(task.Result.Child("Items").GetRawJsonValue());
                // if(FindObjectOfType<MainscreenManager>().regFormAnimator.GetBool("ExpandRegForm"))
                // FindObjectOfType<MainscreenManager>().shrinkRegForm();
                // if(FindObjectOfType<MainscreenManager>().regFormAnimator.GetBool("ExpandLoginForm"))
                //     FindObjectOfType<MainscreenManager>().shrinkLoginForm();
            
                 //Debug.Log();

                 
                 //Debug.Log(task.Result.Value == 100);
//                 Debug.Log((int)task.Result.Value);
                 //Debug.Log(task.value);
                 //Debug.Log(snapshot.Key);
                 //Debug.Log(snapshot.Value);
  //               SaveSystem.SetInt("silverCredits", (int)snapshot.Value);
                 
             }
         });
    }

    public void saveSilverCredits()
    {
        database.RootReference.Child("users").Child(user.UserId).Child("silverCredits").SetValueAsync(SaveSystem.GetInt("silverCredits"));
    }

    public void saveCredits()
    {
        database.RootReference.Child("users").Child(user.UserId).Child("credit").SetValueAsync(SaveSystem.GetInt("credits"));

    }
    public void saveAgentShape()
    {
          database.RootReference.Child("users").Child(user.UserId).Child("currentAgentShape").SetValueAsync(SaveSystem.GetString("currentAgentShape"));
    }

    public void saveStageToRank(string stagename, int value)
    {
     //   var obj = SaveSystem.stageToRank;
        // Debug.Log(obj);
        // Debug.Log("STAGERANK");
        database.RootReference.Child("users").Child(user.UserId).Child("stages").Child(stagename).SetValueAsync(value);
    }
    
    public void saveItems(string itemName, int value)
    {
     //   var obj = SaveSystem.inventoryItems;
        database.RootReference.Child("users").Child(user.UserId).Child("Items").Child(itemName).SetValueAsync(value);
    }

    public void InitsaveGame()
    {
        saveCredits();
        saveSilverCredits();
        saveAgentShape();       
      
    }

  

//	public Dictionary <string, int> stageToRank = new Dictionary<string, int>();
//	public Dictionary <string, int> agentShapes = new Dictionary<string, int> ();
//	public Dictionary <string, int> inventoryItems = new Dictionary<string, int> ();


}
