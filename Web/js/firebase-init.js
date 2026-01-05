import { FIREBASE_CONFIG } from '../config.js';

//we are running the code in module mode
//this taps on the CDN version of the firebase SDK
//for firebase app
import {
    //determine which services you want to use
    initializeApp
} from 'https://www.gstatic.com/firebasejs/10.6.0/firebase-app.js';

//for realtime database
import {
    //determine which services you want to use
    getDatabase, ref, set, get, onValue, push, child, update, remove
} from 'https://www.gstatic.com/firebasejs/10.6.0/firebase-database.js';

//for auth
import {
    //determine which services you want to use
    getAuth, createUserWithEmailAndPassword, signInWithEmailAndPassword, onAuthStateChanged, signOut
} from 'https://www.gstatic.com/firebasejs/10.6.0/firebase-auth.js';


const app = initializeApp(FIREBASE_CONFIG);

//get a reference to the database service
const db = getDatabase();
//Working with Auth
const auth = getAuth();