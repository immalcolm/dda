# Firebase for the web

We use the CDN version of firebase, which takes away the SDK portion.
There is additional load to reference the files online, however it is much more portable without a need to download the SDK.

## To initialize the application
```js
import {
    //determine which services you want to use
    initializeApp
    } from 'https://www.gstatic.com/firebasejs/10.6.0/firebase-app.js';
```

