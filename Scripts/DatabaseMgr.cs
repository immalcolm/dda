using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
public class DatabaseMgr : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputCreatePlayerName;

    [SerializeField]
    private Button btnCreatePlayer;

    [SerializeField]
    private TMP_InputField inputFindPlayerByName;
    [SerializeField]
    private Button btnFindAllPlayers;
    [SerializeField]
    private Button btnFindPlayerByNameOnly;
    [SerializeField]
    private TMP_Text txtSearchResults;

    [SerializeField]
    private TMP_InputField inputDeletePlayer;
    [SerializeField]
    private Button btnDeletePlayer;
    [SerializeField]
    private Button btnDeletePlayerByName;

 
    [SerializeField]
     private TMP_Text txtErrorMsg;

    [SerializeField]
    private GameObject panelUpdatePlayer;
    [SerializeField]
    private Button btnUpdatePlayer;

    //update panel contents
    [SerializeField]
    private TMP_InputField inputUpdatePlayerName;
    [SerializeField]
    private TMP_InputField inputUpdatePlayerHP;
    [SerializeField]
    private TMP_InputField inputUpdatePlayerMana;
    [SerializeField]
    private TMP_InputField inputUpdatePlayerLevel;
    [SerializeField]
    private TMP_Text txtPlayerKey;

    private string currentPlayerSearchKey;

    [SerializeField]
    private TMP_Text txtGameLog;

    //@TODO
    //database
    DatabaseReference mDatabaseRef;

    // Start is called before the first frame update
    void Start()
    {
        //@TODO check whether inputs are proper assigned
        //check for null
        btnCreatePlayer.onClick.AddListener(CreatePlayer);
        btnFindAllPlayers.onClick.AddListener(DisplayPlayers);
        //btnFindAllPlayers.onClick.AddListener(FindAllPlayerWithName);
        //btnFindPlayerByNameOnly.onClick.AddListener(() => FindPlayerByNameOnly());
        btnDeletePlayer.onClick.AddListener(DeletePlayerByKey);
        btnDeletePlayerByName.onClick.AddListener(DeletePlayerByPlayerName);
        btnUpdatePlayer.onClick.AddListener(UpdatePlayer);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp.Create();
            //set our database reference to the root 
            mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        });

    }
    /// <summary>
    /// Create a player based on user name and place it under players
    /// </summary>
    public void CreatePlayer()
    {
        //@TODO check for user inputs

        //@TODO check whether player id (displayname) exist first

        //Create our entry into database
        string playerName = inputCreatePlayerName.text;
        //create player with our new player name
        Player p = new Player(playerName);
        //convert to JSON using Unity JsonUtility
        string jsonEntry = JsonUtility.ToJson(p);

        //create a new node below players
        //the node playerName will be the key. <playername>:{ player details...}
        mDatabaseRef.Child("players").Child(playerName).SetRawJsonValueAsync(jsonEntry);

        //compressed version
        //mDatabaseRef.Child("players").SetRawJsonValueAsync(JsonUtility.ToJson(new Player(playerName)));
    }

    /// <summary>
    /// Function to read and display all our player data
    /// </summary>
    public void DisplayPlayers()
    {
        Debug.Log("Enter Display Players");
        mDatabaseRef.Child("players").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //@TODO
                //process error
            }
            else if (task.IsCompleted)
            {
                //this is a giant snapshot of our data
                DataSnapshot ds = task.Result;

                //READ: When you retrieve data from Firebase,
                //it often comes as a collection of children under a parent node,
                //especially when you're fetching lists or collections.

                //let's loop and display
                //we use var -> to say let c# decide the datatype
                //foreach(<data obj type> <variable name> in <DataSnapshot>.Children)
                txtSearchResults.text = string.Format("<b>Search Results: {0} Players found</b>", ds.ChildrenCount);
                foreach (var v in ds.Children)
                {
                    string data = v.ToString();
                    //Key? => playerName as the id
                    //Value? => player details

                    string playerKey = v.Key;
                    string playerDetails = v.GetRawJsonValue();
                    Debug.LogFormat("Player Key: {0} \n Player Details: {1}"
                        , playerKey, playerDetails);

                    //now let's convert the player Details nicely into a Player Obj
                    //template >> JsonUtility.FromJson<obj class>(<json value>);
                    //process >> Convert snapshot to JSON string and then
                    //deserialize to Player object

                    Player p = JsonUtility.FromJson<Player>(playerDetails);
                    //now we can use the player properties anywhere we want
                    //now let's display the player details nicely.
                    Debug.LogFormat("Player ID ({0}):\n{1}",
                        p.playerName, p.PrintPlayerDetails()); ;
                    txtSearchResults.text += string.Format("\nPlayer ID ({0}):\n{1}",
                        p.playerName, p.PrintPlayerDetails());
                }
            }
        });
    }

    /// <summary>
    /// Update some player details based on the current search result
    /// </summary>
    public void UpdatePlayer()
    {
        //@TODO
        //check whether there's a key even
        if (string.IsNullOrEmpty(currentPlayerSearchKey))
        {
            //@TODO display error
            Debug.Log("No current player assigned");
            txtGameLog.text = "\nNo current player assigned...";
        }
        else
        {
            //get input fields
            Dictionary<string, object> p = new Dictionary<string, object>();

            p["health"] = int.Parse(inputUpdatePlayerHP.text);
            p["mana"] = int.Parse(inputUpdatePlayerMana.text);
            p["level"] = int.Parse(inputUpdatePlayerLevel.text);
            p["playerName"] = inputUpdatePlayerName.text;
            //update based on the key assigned
            mDatabaseRef.Child("players").Child(currentPlayerSearchKey).UpdateChildrenAsync(p);

            //update status

            //clear fields
            inputUpdatePlayerHP.text = "";
            inputUpdatePlayerLevel.text = "";
            inputUpdatePlayerMana.text = "";
            inputUpdatePlayerName.text = "";

            //clear current key
            currentPlayerSearchKey = "";

            txtGameLog.text += "\nPlayer Updated...";


        }


    }

    /// <summary>
    /// Retrieve all players with targetted name
    /// </summary>
    public void FindAllPlayerWithName()
    {
        Debug.Log("Find All players with name entered");
        string searchKey = inputFindPlayerByName.text;
        Query query = mDatabaseRef.Child("players").OrderByChild("playerName").EqualTo(searchKey);

        //perform query
        query.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //@TODO
            }
            else if (task.IsCompleted)
            {
                DataSnapshot ds = task.Result;

                //loop it
                if (ds.HasChildren)
                {
                    Debug.LogFormat("Number of players found: {0}", ds.ChildrenCount);
                    txtGameLog.text += string.Format("\nNumber of players found: {0}", ds.ChildrenCount);
                    foreach (var v in ds.Children)
                    {
                        string json = v.GetRawJsonValue();
                        Debug.Log("Player found" + json);
                    }

                }
                else
                {
                    Debug.Log("No players found.... ");
                    txtGameLog.text += "\nNo Players found...";
                }
            }
        });

    }

    /// <summary>
    /// let's limit to only the first entry that comes back
    /// </summary>
    /// <param name="limit">default set to 1</param>
    public void FindPlayerByNameOnly(int limit = 1)
    {
        string searchKey = inputFindPlayerByName.text;
        Query query = mDatabaseRef.Child("players").OrderByChild("playerName").EqualTo(searchKey).LimitToFirst(limit);

        //perform query
        query.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //@TODO
            }
            else if (task.IsCompleted)
            {
                DataSnapshot ds = task.Result;

                //loop it
                if (ds.HasChildren)
                {
                    Debug.LogFormat("Number of players found: {0}", ds.ChildrenCount);
                    foreach (var v in ds.Children)
                    {
                        string json = v.GetRawJsonValue();
                        Debug.Log("Player found" + json);
                        //Convert to Player Object
                        Player p = JsonUtility.FromJson<Player>(json);

                        //display and update PanelUpdate
                        inputUpdatePlayerHP.text = p.health.ToString();
                        inputUpdatePlayerMana.text = p.mana.ToString();
                        inputUpdatePlayerLevel.text = p.level.ToString();
                        inputUpdatePlayerName.text = p.playerName;

                        //update our current key
                        currentPlayerSearchKey = v.Key;
                        txtPlayerKey.text = "Player Key: " + currentPlayerSearchKey;
                    }

                }
                else
                {
                    Debug.Log("No players found.... ");
                }
            }
        });
    }

    /// <summary>
    /// @TODO enhance by searching first
    /// we use setvalue to null to empty out the value
    /// we can use use RemoveValueAsync
    /// path >> root/players/<player key>
    /// if it's the last entry the parent node will be deleted
    /// </summary>
    public void DeletePlayerByKey()
    {
        //@TODO
        //check whether does the player even exist for a more meaningful system


        //get by key
        string playerKey = inputDeletePlayer.text;
        DatabaseReference mdb = FirebaseDatabase.DefaultInstance.GetReference("players/" + playerKey);
        mdb.SetValueAsync(null).ContinueWithOnMainThread(task =>
        {

            if (task.IsFaulted)
            {
                //@TODO
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Player successfully deleted");
            }
        });
    }

    public void DeletePlayerByPlayerName()
    {
        //search

        //delete

        string searchKey = inputDeletePlayer.text;
        Query query = mDatabaseRef.Child("players").OrderByChild("playerName").EqualTo(searchKey);

        //perform query
        query.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //@TODO
            }
            else if (task.IsCompleted)
            {
                DataSnapshot ds = task.Result;

                //loop it
                if (ds.HasChildren)
                {
                    Debug.LogFormat("Number of players found: {0}", ds.ChildrenCount);
                    foreach (var v in ds.Children)
                    {
                        string json = v.GetRawJsonValue();
                        Debug.Log("Player found" + json);
                        Debug.Log("deleting... player");
                        DeletePlayer(v.Key);
                    }

                }
                else
                {
                    Debug.Log("No players found.... ");
                }
            }
        });//end query
    }

    public void DeletePlayer(string key)
    {
        mDatabaseRef.Child("players").Child(key).RemoveValueAsync();
    }
}
