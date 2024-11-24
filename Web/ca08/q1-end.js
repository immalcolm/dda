const timestamp = Math.floor(Date.now() / 1000);
// literal obj of SimpleLeaderBoard
let SimpleLeaderBoard = {
  userName: "Alfred",
  score: 100,
  updatedOn: timestamp,
};

console.log(SimpleLeaderBoard);
/*
C# class
public class SimpleLeaderBoard{

    public string userName { get; set; }
    public int score { get; set; }
    public long timestamp { get; set; }

    public SimpleLeaderBoard(string userName, int score, long timestamp){
        this.userName = userName;
        this.score = score;
        this.timestamp = timestamp;
    }

}
*/
