//literal object

//curly brace means it's an object
//literal object can only hold data within
//key:value pairs
const player = {
    playerName: "Malcolm",
    level: 5,
    health: 100, 
    maxHealth: 500,
    mana: 38, 
    agility: 12,
    timeCreated: Date.now()
} 

const gameSettings = {
    sound: true,
    music: true,
    difficulty: "normal",
    resolution: "1920x1080"
}

//function based objects
function Player(name, level, health, maxHealth, mana, agility) {
    this.playerName = name;
    this.level = level;
    this.health = health;
    this.maxHealth = maxHealth;
    this.mana = mana;
    this.agility = agility;
    this.timeCreated = Date.now();
}

function GameLevel(enemyCount, bossCount, levelTime, levelDifficulty) {
    this.enemyCount = enemyCount;
    this.bossCount = bossCount;
    this.levelTime = levelTime;
    this.levelDifficulty = levelDifficulty;
    this.levelCreated = Date.now();
}

const player2 = new Player("Alice", 3, 80, 300, 50, 15);
const player3 = new Player("Bob", 7, 200, 600, 20, 10);