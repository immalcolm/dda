//declare variables
//var
var x = 10;
//let
let y = 20; //most preferrred way 
//const
const PI = 3.14; // also a good way if we dont change the values

//create simple function
//pie here has a default value of 3.14
function calculateArea(radius, pie = 3.14) {
    return pie * radius * radius;
}
//calling the function 
calculateArea(5);
calculateArea(5,PI);

//simple arr
let simpleArr = [1,2,3,4,5];
//in javascript can store anything inside an array
let rojak = [1,"98ewrf", function(){console.log("hello")}, {name: "John"}];
//simple for loop
for(let i = 0; i < simpleArr.length; i++) {
    console.log(simpleArr[i]);
}
simpleArr.push(6);
simpleArr.pop();//remove the last element

//simple literal object 
let alfred={
    "name": "Kim Jung Woo Alfred",
    "age": 21, 
    "country": "West Korea"
}

console.log(alfred.name);
console.log(alfred["age"])

//function based object
function WestKorean(name, age, country) {
    this.name = name; //c# string name = .... 
    this.age = age;
    this.country = country;
}

//create a WestKorean object 
let alfred2 = new WestKorean("Kim Jung Wee Alfredo", 21,"West Korea");
console.log(alfred2.name);

let armyOfWestKorean = [];
armyOfWestKorean.push(alfred2);
armyOfWestKorean.push(new WestKorean("Ah juma",99,"West Korean"));

//simple for each loop
for(let korean of armyOfWestKorean){
    console.log(`
        Name: ${korean.name}, 
        Age: ${korean.age}, 
        Country: ${korean.country}`
    );
}


