# Instructions
Create your READ data and display in HTML Based on your previous CAs, add in new functionalities to deal with READ data.

**To be taken care**
- Place your configs in another folder
- Optimise your code to show what happens when there’s no data.
- Use Bootstrap for nicer UI presentation
- You may use one main node reference e.g players or playerStats depending on your own data structure

## References
- [Bootstrap Alerts](https://getbootstrap.com/docs/5.3/components/alerts/#examples)
```
Start
 |
 |-- Step 1: Import Firebase Modules
 |     |
 |     |-- (a) Import initializeApp from Firebase App SDK
 |     |-- (b) Import Realtime Database Services (getDatabase, ref, set, etc.)
 |     `-- (c) Import Authentication Services (getAuth, createUserWithEmailAndPassword, etc.)
 |
 |-- Step 2: Define Firebase Configuration
 |     `-- Set the FIREBASE_CONFIG object with your Firebase project details
 |
 |-- Step 3: Initialize Firebase App
 |     `-- Call initializeApp() with FIREBASE_CONFIG
 |
 |-- Step 4: Initialize Firebase Services
 |     |
 |     |-- (a) Get Database Reference
 |     |     |-- Call getDatabase() to get a reference to the database
 |     |     `-- Use ref() to define specific data paths (e.g., "players")
 |     `-- (b) Initialize Authentication
 |           `-- Call getAuth() to initialize Firebase Authentication
 |- End
```

## Firebase Skeleton JS 
```js
//we are running the code in module mode
//this taps on the CDN version of the firebase SDK
import {
  initializeApp
} from 'https://www.gstatic.com/firebasejs/11.0.2/firebase-app.js';

//for realtime database
import {
  //determine which services you want to use
  getDatabase, ref, set, get, onValue, push, child, update, remove
} from 'https://www.gstatic.com/firebasejs/11.0.2/firebase-database.js';

//for auth
import {
  //determine which services you want to use
  getAuth, createUserWithEmailAndPassword, signInWithEmailAndPassword, onAuthStateChanged, signOut
} from 'https://www.gstatic.com/firebasejs/11.0.2/firebase-auth.js';

//Your web app's Firebase configuration

const FIREBASE_CONFIG = {
  apiKey: "",
  authDomain: "",
  databaseURL: "",
  projectId: "",
  storageBucket: "",
  messagingSenderId: "",
  appId: ""
};


// Initialize Firebase
const app = initializeApp(FIREBASE_CONFIG);

//get a reference to the database service
const db = getDatabase();
const playerRef = ref(db, "players");
/Working with Auth
const auth = getAuth();

```