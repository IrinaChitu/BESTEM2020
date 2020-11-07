using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Database;

using APIs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class FirebaseManager : MonoBehaviour
	{
	    //Firebase variables
	    [Header("Firebase")]
	    public DependencyStatus dependencyStatus;
	    public FirebaseAuth auth;
	    public FirebaseFirestore firestore;
	    public DatabaseReference database;

	    private void Awake()
	    {
	        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
	        {
	            dependencyStatus = task.Result;
	            if (dependencyStatus == DependencyStatus.Available)
	            {
	                InitializeFirebase();
	            }
	            else
	            {
	                Debug.LogError("Could not resolve all Firebase dependencies " + dependencyStatus);
	            }
	        });
	    }

	    private void InitializeFirebase()
	    {
	        Debug.Log("Setting up Firebase Auth");
	        //Set the authentication instance object
	        auth = FirebaseAuth.DefaultInstance;
	        firestore = FirebaseFirestore.DefaultInstance;
	        database = FirebaseDatabase.DefaultInstance.RootReference;

	        DatabaseAPI.InitializeDatabase();
	    }
	}
}