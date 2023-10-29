using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//list of Firebase and Unity imports
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;//for Firebase Authentication
using Firebase.Extensions;//for Firebase Tasks
using UnityEngine.UI;//for basic Unity UI
using TMPro;//for TextMeshPro UI
using System.Text.RegularExpressions;

//@TODO: Read https://firebase.google.com/docs/auth/unity/start
//Read: https://docs.unity3d.com/ScriptReference/Debug.LogFormat.html
//Read: https://firebase.google.com/docs/reference/unity/namespace/firebase/auth

//@TODO: READ
//1. Approach your code systematically and test
//2. Plan and test your UI in parts. DO NOT do it in 1 lump chump with copy/paste.
//3. Progressive testing and ensure your code works then move on.
//4. Leave error handling for later if you are afraid. Make 1 flow to work first.

//we use MyAuth like manager for authetication methods 
//Take note it is a monobehaviour. We will assign it to a game obj 
public class MyAuth : MonoBehaviour
{

    //declare our UI system
    //ensure that you have assign them appropriately in Unity Inspector
    //simply drag and drop the related UI
    [SerializeField]
    private TMP_InputField inputEmail;
    [SerializeField]
    private TMP_InputField inputPassword;

    //buttons 
    [SerializeField]
    private Button btnSignUp;
    [SerializeField]
    private Button btnSignIn;
    [SerializeField]
    private Button btnSignOut;

    //Grouping our UI
    [SerializeField]
    private GameObject panelLogin;
    [SerializeField]
    private GameObject panelProfile;

    //Logged in UI
    [SerializeField]
    private TMP_Text txtProfileContent;
    [SerializeField]
    private TMP_InputField inputUpdateDisplayName;
    [SerializeField]
    private Button btnUpdateProfile;

    //for visual error display or to use as auth errors
    [SerializeField]
    private TMP_Text txtError;

    //declare auth related items
    FirebaseAuth mAuth;
    //this is a firebase user object
    //Firebase.Auth.FirebaseUser
    FirebaseUser mUser;

    // Start is called before the first frame update
    void Start()
    {
        //1. init our auth
        //<auth variable> = FirebaseAuth.DefaultInstance;
        mAuth = FirebaseAuth.DefaultInstance;

        //2. add button handlers (you can use Unity hierachy + inspector)
        //using a common temp var, we can skip this if we have a button property
        //Button b = btnSignUp.GetComponent<Button>();

        //add our listener/handler
        //<btn variable>.onClick.AddListener(<function to call>);
        //This allows us better control and code portability. Less reliant on the Unity Inspector
        //@TODO --> check whether buttons are assigned
        btnSignUp.onClick.AddListener(SignUp);
        btnSignIn.onClick.AddListener(SignIn);

        btnSignOut.onClick.AddListener(SignOut);

        //@TODO
        //this checks whether the button is assigned properly in the inspector
        //commonly gives null reference errors
        if (btnUpdateProfile != null)
        {
            btnUpdateProfile.onClick.AddListener(() => UpdateUserDisplayName(inputUpdateDisplayName.text));
        }

        //3. hardset panel status
        panelLogin.SetActive(true);
        panelProfile.SetActive(false);
    }

    //Signup the user

    public void SignUp()
    {
        //Test and debug our button click first
        Debug.Log("SignUp Button is being clicked....");

        //get input values
        //@TODO  the login details should be error handled first
        string email = inputEmail.text;
        string password = inputPassword.text;

        //tap on our auth obj and call the function
        //use ContinueOnMainThread!!
        mAuth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                string errorMsg = HandleAuthExceptions(task.Exception);
                Debug.LogFormat("CreateUserWithEmailAndPasswordAsync Error>>> {0}", errorMsg);
                txtError.text += string.Format("SignInWithEmailAndPasswordAsync Error >>> {0} ", errorMsg);

                //Debug.Log(task.Exception.GetBaseException());
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {

                // Firebase user has been created.
                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1}) {2}",
                    result.User.Email, result.User.UserId, JsonUtility.ToJson(result.User));

                //we can update user name after creation

                //for an update of user display name
                //set displayname to userid by default
                UpdateUserDisplayName(result.User.UserId);
                //Can we store into firebase DB?

                //@TODO
                //show signout button
                //clear textfields
                ClearFields();

                //without ContinueWithOnMainThread this will fail to work
                //hide our UI
                ToggleLoginPanel();
                ToggleProfilePanel();
            }

        });
    }

    //Signout the user
    //@TODO
    //check if auth is available and check if sign in
    //show sign out button
    //sign out the user only if sign in
    public void SignOut()
    {

        //how can we check the auth?
        //hint: your auth object and null?
        if (mAuth != null && mAuth.CurrentUser != null)
        {
            Debug.Log("K thanks Bye!!! " + mAuth.CurrentUser.Email);
            txtError.text += "\nK thanks Bye!!! " + mAuth.CurrentUser.Email;
            //@TODO signout
            //<auth obj>.SignOut();

            ToggleLoginPanel();
            ToggleProfilePanel();
            mAuth.SignOut();

        }
    }

    //@TODO sign in the user based on the email and password
    public void SignIn()
    {

        //1. retrieve login details
        //@TODO  the login details should be error handled first
        string email = inputEmail.text;
        string password = inputPassword.text;

        //2. attempt to sign with user
        mAuth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            //3. if there's issue do something about it
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                string errorMsg = HandleAuthExceptions(task.Exception);
                Debug.LogFormat("SignInWithEmailAndPasswordAsync Error>>> {0}", errorMsg);
                txtError.text += string.Format("\nSignInWithEmailAndPasswordAsync Error >>> {0} ", errorMsg);

                //Debug.Log(task.Exception.GetBaseException());
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }


            //4. If it's ok let's do something about it
            //you can consider load new scene, show existing panels or hide etc.
            //there are several ways to retrieve the current user

            //a) AuthResult <your variable name> = task.Result;
            //Displayname is not created by default.
            //Hence you have to enforce it after the creation process

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} {1} : {2}",
                result.User.UserId, result.User.DisplayName, result.User.Email);

            //b) let's make a better reference using FirebaseUser class
            Debug.Log("\n Working with FirebaseUser");
            //we assign Firebase.Auth.FirebaseUser to our predeclare mUser
            mUser = task.Result.User;
            Debug.LogFormat("User signed in successfully (FirebaseUser): {0} ({1}) : {2}",
               mUser.UserId, mUser.DisplayName, mUser.Email);

            //c) printout our current logged in user, using our current user class
            Debug.LogFormat("Currently logged in user: {0} {1} :  {2}",
                mAuth.CurrentUser.UserId,
                mAuth.CurrentUser.DisplayName,
                mAuth.CurrentUser.Email);

            //5. Handle our UI
            ClearFields();
            ToggleLoginPanel();
            ToggleProfilePanel();

            ShowUserProfile();

        });
    }

    //@TODO Think about where and when we should place this code
    //namespace being used here to show the full path of the library
    public void UpdateUserDisplayName(string newDisplayName)
    {
        //@TODO
        //check whether the newDisplayName makes sense or not
        //see what rules you want
        if (mAuth.CurrentUser.DisplayName != "" && !(IsValidUsername(newDisplayName)))
        {
            //Debug.LogError("Username is invalid.. has to be at least 3 characters");
            return;
        }

        //retrieve the current logged in user
        Firebase.Auth.FirebaseUser user = mAuth.CurrentUser;

        //check whether user obj is valid and user is logged in
        if (user != null)
        {
            //The Userprofile has specific properties
            //we tap on Firebase.Auth.UserProfile class to create a new UserProfile object
            //Simply type profile.  <-- the dot over here will expand.
            //profile here is our variable name. It could be anything
            //You will be shown a list of properties involved
            //profile object below
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {

                DisplayName = newDisplayName,
                // PhotoUrl = new System.Uri("https://amphibistudio.sg")
            };

            //update our userprofile by completing replacing the profile object
            user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    txtError.text += string.Format("\nUpdateUserProfileAsync encountered an error: {0}", task.Exception);
                    return;
                }
                //refresh our profile panel
                ShowUserProfile();

                Debug.Log("User profile updated successfully." + user.DisplayName + ":" + user.UserId);

            });
        }

    }

    //rules for Displayname
    //what other rules?
    public bool IsValidUsername(string username)
    {

        // Rule 1: Check Length
        if (username.Length < 3 || username.Length > 20)
        {
            txtError.text += string.Format("\nUsername {0} invalid : Needs to be > 3 characters or less than 20", username);
            return false;

        }

        // Rule 2: Character Restrictions
        Regex validUsernamePattern = new Regex(@"^[a-zA-Z0-9_]+$"); // Alphanumeric and underscores only
        if (!validUsernamePattern.IsMatch(username))
        {

            txtError.text += string.Format("\nUsername {0} invalid : Special characters not allowed", username);
            return false;
        }

        // Rule 3: Content Restrictions (simple example)
        string[] disallowedNames = { "admin", "moderator", "support" };
        foreach (string name in disallowedNames)
        {
            if (username.ToLowerInvariant().Contains(name))
            {

                txtError.text += string.Format("\nUsername {0} invalid : Disallowed name", username);
                return false;
            }
        }

        // Add other rules as needed...F

        return true;
    }

    //@TODO how can we clear the input fields ?
    private void ClearFields()
    {
        inputEmail.text = "";
        inputPassword.text = "";
    }

    public void ToggleLoginPanel()
    {
        // Checks if the panel is active in the scene
        bool isActive = panelLogin.activeSelf;

        // Sets the panel's active state to the opposite of its current state
        panelLogin.SetActive(!isActive);
        Debug.Log("Toggle login panel" + panelLogin.activeSelf);
    }

    public void ToggleProfilePanel()
    {
        // Checks if the panel is active in the scene
        bool isActive = panelProfile.activeSelf;

        // Sets the panel's active state to the opposite of its current state
        panelProfile.SetActive(!isActive);

        Debug.Log("Toggle login panel: " + panelProfile.activeSelf);
    }

    public void ShowUserProfile()
    {
        //check if signed in
        if (IsUserLoggedIn())
        {
            //display profile details
            txtProfileContent.text = GetUserProfile();
            inputUpdateDisplayName.text = mAuth.CurrentUser.DisplayName;
        }
    }


    //check whether our user is available and logged in
    public bool IsUserLoggedIn()
    {
        return mAuth.CurrentUser != null;
    }


    //custom string to display user profile
    public string GetUserProfile()
    {
        string profile = "Display Name: " + mAuth.CurrentUser.DisplayName +
            "\nEmail: " + mAuth.CurrentUser.Email +
            "\nUser ID: " + mAuth.CurrentUser.UserId;

        return profile;
    }

    //how can we handle errors?
    //only if it's a firebaseuser object
    //@TODO handle your own auth authetication
    //@TODO READ:https://firebase.google.com/docs/reference/unity/namespace/firebase/auth
    public string HandleAuthExceptions(System.AggregateException e)
    {
        string errorMsg = "";

        if (e != null)
        {
            //classify our base exception into a FirebaseException object
            FirebaseException firebaseEx = e.GetBaseException() as FirebaseException;

            //Cast our error codes into proper Firebase AuthError codes
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            Debug.LogError("Error in auth.... error code: " + errorCode);
            //care for common errors
            //@TODO there may be edge cases or other errors to handle
            //@TODO use meaningful error messages
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    errorMsg += "Missing email input";
                    break;
                case AuthError.MissingPassword:
                    errorMsg += "Missing password input";
                    break;
                case AuthError.WrongPassword:
                    errorMsg += "Wrong password";
                    break;
                case AuthError.InvalidEmail:
                    errorMsg += "Email appears to be malformed or invalid";
                    break;
                case AuthError.UserNotFound:
                    errorMsg += "Account does not appear to exist in the system";
                    break;
                case AuthError.WeakPassword:
                    errorMsg += "Password used appears to be weak...";
                    break;
                case AuthError.EmailAlreadyInUse:
                    errorMsg += "Email is already in use... ";
                    break;
                case AuthError.UserMismatch:
                    errorMsg += "User Mismatch";
                    break;
                case AuthError.Failure:
                    errorMsg += "Failed to login...";
                    break;
                default:
                    errorMsg += "Issue in authetication" + errorCode;
                    break;
            }
        }
        return errorMsg;
    }

}
