let hospitalUri = "https://localhost:7195/api/Survey/hospital/stats"
let doctorUri = "https://localhost:7195/api/Survey/doctor/stats/"
let bestUri = "https://localhost:7195/api/Survey/best-doctors"
let worstUri = "https://localhost:7195/api/Survey/worst-doctors"
let allDoctorsUri = "https://localhost:7195/api/Doctor/"

/*
currentTime();
let user = JSON.parse(sessionStorage.getItem("user"));
console.log(user);
setNameOnCorner(user);
*/

var hospital = [];
var doctors = [];
var best = [];
var worst = [];
var allDoctors = [];

let allDoctorsRequest = new XMLHttpRequest();
allDoctorsRequest.open('GET', allDoctorsUri); 
allDoctorsRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            allDoctors = JSON.parse(allDoctorsRequest.response);
            populateTableAllDoctors()
        } else {
            alert("Greska prilikom kreiranja korisnika.")
        }
    }
}
allDoctorsRequest.send();

let hospitalRequest = new XMLHttpRequest();
hospitalRequest.open('GET', hospitalUri); 
hospitalRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            hospital = JSON.parse(hospitalRequest.response);
            populateTableHospital()
        } else {
            alert("Greska prilikom kreiranja korisnika.")
        }
    }
}
hospitalRequest.send();

function doctorRequest(id) {
    let doctorRequest = new XMLHttpRequest();
    doctorRequest.open('GET', doctorUri + id); 
    doctorRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                doctors = JSON.parse(doctorRequest.response);
                populateTableDoctors()
            } else {
                alert("Greska prilikom kreiranja korisnika.")
            }
        }
    }
    doctorRequest.send();
}

let bestRequest = new XMLHttpRequest();
bestRequest.open('GET', bestUri); 
bestRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            best = JSON.parse(bestRequest.response);
            populateTableBest()
        } else {
            alert("Greska prilikom kreiranja korisnika.")
        }
    }
}
bestRequest.send();

let worstRequest = new XMLHttpRequest();
worstRequest.open('GET', worstUri); 
worstRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            worst = JSON.parse(worstRequest.response);
            populateTableWorst()
        } else {
            alert("Greska prilikom kreiranja korisnika.")
        }
    }
}
worstRequest.send();


function populateTableAllDoctors() {
    let table = document.createElement("table")
    let row = table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "ID"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "NAME"
    let cell3 = row.insertCell(2);
    cell3.innerHTML = "SURNAME"

    for (let i = 0; i < allDoctors.length; ++i) {
        let element = allDoctors[i]

        let row = table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);
        let cell3 = row.insertCell(2);

        cell1.innerHTML = element["id"];
        cell2.innerHTML = element["name"];
        cell3.innerHTML = element["surname"];
    }
    document.getElementById("all-doctors-table").appendChild(table);
}

function populateTableHospital() {
    let table = document.createElement("table")
    let row = table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "QUESTION"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "ANSWER COUNT"
    let cell3 = row.insertCell(2);
    cell3.innerHTML = "ANSWER AVERAGE"
    let cell4 = row.insertCell(3);
    cell4.innerHTML = "COMMENTS"

    for (let i = 0; i < hospital.length; ++i) {
        let element = hospital[i]

        let row = table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);
        let cell3 = row.insertCell(2);
        let cell4 = row.insertCell(3);

        cell1.innerHTML = element["question"]["text"];
        cell2.innerHTML = element["count"];
        cell3.innerHTML = parseFloat(element["average"]).toFixed(2);
        cell4.innerHTML = parseComments(element["comments"]);
    }
    document.getElementById("hospital-table").appendChild(table);
}

function parseComments(element) {
    var ret = ""
    for (let i = 0; i < element.length; i++) {
        ret += element[i] + "<br>"
    }
    return ret
}

function populateTableDoctors() {
    document.getElementById("doctor-table").innerHTML = ""
    let table = document.createElement("table")
    let row = table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "QUESTION"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "ANSWER COUNT"
    let cell3 = row.insertCell(2);
    cell3.innerHTML = "ANSWER AVERAGE"
    let cell4 = row.insertCell(3);
    cell4.innerHTML = "COMMENTS"

    for (let i = 0; i < doctors.length; ++i) {
        let element = doctors[i]

        let row = table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);
        let cell3 = row.insertCell(2);
        let cell4 = row.insertCell(3);

        cell1.innerHTML = element["question"]["text"];
        cell2.innerHTML = element["count"];
        cell3.innerHTML = parseFloat(element["average"]).toFixed(2);
        cell4.innerHTML = parseComments(element["comments"]);
    }
    document.getElementById("doctor-table").appendChild(table);
}

function populateTableBest() {
    let table = document.createElement("table")
    let row = table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "NAME"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "SURNAME"
    let cell3 = row.insertCell(2);
    cell3.innerHTML = "AVERAGE GRADE"

    best.sort((x, y) => y["average"] - x["average"])

    for (let i = 0; i < best.length; ++i) {
        let element = best[i]

        let row = table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);
        let cell3 = row.insertCell(2);

        cell1.innerHTML = element["doctor"]["name"];
        cell2.innerHTML = element["doctor"]["surname"];
        cell3.innerHTML = parseFloat(element["average"]).toFixed(2);
    }
    document.getElementById("top-rated-table").appendChild(table);
}

function populateTableWorst() {
    let table = document.createElement("table")
    let row = table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "NAME"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "SURNAME"
    let cell3 = row.insertCell(2);
    cell3.innerHTML = "AVERAGE GRADE"
    
    worst.sort((x, y) => x["average"] - y["average"])

    for (let i = 0; i < worst.length; ++i) {
        let element = worst[i]

        let row = table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);
        let cell3 = row.insertCell(2);

        cell1.innerHTML = element["doctor"]["name"];
        cell2.innerHTML = element["doctor"]["surname"];
        cell3.innerHTML = parseFloat(element["average"]).toFixed(2);
    }
    document.getElementById("worst-rated-table").appendChild(table);
}

document.getElementById("find").addEventListener("click", function(){
    event.preventDefault()
    find()
});

function find() {
    let id = document.getElementById("doctor-id").value
    if (id == "")
    {
        alert("Please provide the doctor's ID.")
        return
    }
    doctorRequest(id)
}
