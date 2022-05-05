"use strict";
console.log("1");
const ship = document.getElementById("ship");
const trackingNumber = document.getElementById("trackingNumber");
const carrier = document.getElementById("carrier");

ship.addEventListener("onClick", function (event) {
    console.log("2");
    event.preventDefault();
    alert("Fill out the tracking and carrier input!");
});