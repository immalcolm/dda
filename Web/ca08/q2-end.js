//function based object
function SimpleLeaderBoard(userName, score, timestamp) {
    this.userName = userName;
    this.score = score;
    this.timestamp = timestamp;
}
//simple array
let leaderboard = []


const timestamp = Math.floor(Date.now() / 1000);
//create our simpleLeaderBoard object
let simpleLeaderBoard1 = new SimpleLeaderBoard("Alfred", 100, timestamp);
let simpleLeaderBoard2 = new SimpleLeaderBoard("Bob", 200, timestamp);

//add stuff into the array
leaderboard.push(simpleLeaderBoard1);
leaderboard.push(simpleLeaderBoard2);

console.log(leaderboard);