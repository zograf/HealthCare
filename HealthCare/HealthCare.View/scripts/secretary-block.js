let blockUri = "https://localhost:7195/api/Patient/block/"
let unblockUri = "https://localhost:7195/api/Patient/unblock/"
let patientUri = "https://localhost:7195/api/Patient/"

/*
currentTime();
let user = JSON.parse(sessionStorage.getItem("user"));
console.log(user);
setNameOnCorner(user);
*/

var unblocked = [];
var blocked = [];

let getPatientsRequest = new XMLHttpRequest();
getPatientsRequest.open('GET', patientUri); 
getPatientsRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            var patients = JSON.parse(getPatientsRequest.response);
            for (let i = 0; i < patients.length; ++i) {
                let patient = patients[i]
                if (patient["blockedBy"] == "Secretary" || patient["blockedBy"] == "System")
                    blocked.push(patient)
                else
                    unblocked.push(patient)
            }
            populateTable()
        } else {
            alert("Greska prilikom kreiranja korisnika.")
        }
    }
}

getPatientsRequest.send();

function populateTable() {
    let unblocked_table = document.createElement("table")
    let row = unblocked_table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "ID"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "NAME"
    let cell3 = row.insertCell(2);
    cell3.innerHTML = "SURNAME"
    for (let i = 0; i < unblocked.length; ++i) {
        let patient = unblocked[i]

        let row = unblocked_table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);
        let cell3 = row.insertCell(2);

        cell1.innerHTML = patient["id"];
        cell2.innerHTML = patient["name"];
        cell3.innerHTML = patient["surname"];
    }
    document.getElementById("unblocked-table").appendChild(unblocked_table);

    let blocked_table = document.createElement("table")
    row = blocked_table.insertRow(0);
    cell1 = row.insertCell(0);
    cell1.innerHTML = "ID"
    cell2 = row.insertCell(1);
    cell2.innerHTML = "NAME"
    cell3 = row.insertCell(2);
    cell3.innerHTML = "SURNAME"
    for (let i = 0; i < blocked.length; ++i) {
        let patient = blocked[i]

        let row = blocked_table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);
        let cell3 = row.insertCell(2);

        cell1.innerHTML = patient["id"];
        cell2.innerHTML = patient["name"];
        cell3.innerHTML = patient["surname"];
    }
    document.getElementById("blocked-table").appendChild(blocked_table);
}
document.getElementById("block").addEventListener("click", function(){
    event.preventDefault()
    block()
});

document.getElementById("unblock").addEventListener("click", function(){
    event.preventDefault()
    unblock()
});

function block() {
    let id = document.getElementById("patient-id").value
    console.log(id)

    let blockPatientRequest = new XMLHttpRequest();
    blockPatientRequest.open('PUT', blockUri + id); 
    blockPatientRequest.setRequestHeader('Content-Type', 'application/json');
    blockPatientRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                alert("Block successful!")
                location.reload();
            } else {
                alert("Greska prilikom kreiranja korisnika.")
            }
        }
    }
    blockPatientRequest.send(id);
}

function unblock() {
    let id = document.getElementById("patient-id").value

    let unblockPatientRequest = new XMLHttpRequest();
    unblockPatientRequest.open('PUT', unblockUri + id); 
    unblockPatientRequest.setRequestHeader('Content-Type', 'application/json');
    unblockPatientRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                alert("Unblock successful!")
                location.reload();
            } else {
                alert("Greska prilikom kreiranja korisnika.")
            }
        }
    }
    unblockPatientRequest.send(id);
}


