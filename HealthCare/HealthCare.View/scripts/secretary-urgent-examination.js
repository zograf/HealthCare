let specializationUri = "https://localhost:7195/api/Specialization/"
let patientUri = "https://localhost:7195/api/Patient/"
let urgentAppointmentUri = "https://localhost:7195/api/UrgentAppointment/urgentList"

/*
currentTime();
let user = JSON.parse(sessionStorage.getItem("user"));
console.log(user);
setNameOnCorner(user);
*/

var specializations = [];
var patients = [];

let getSpecializationsRequest = new XMLHttpRequest();
getSpecializationsRequest.open('GET', specializationUri); 
getSpecializationsRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            let specs = JSON.parse(getSpecializationsRequest.response);
            for (let i = 0; i < specs.length; ++i) {
                specializations.push(specs[i])
            }
            populateTableSpecialization()
        } else {
            alert("Greska prilikom kreiranja korisnika.")
        }
    }
}

getSpecializationsRequest.send();

let getPatientsRequest = new XMLHttpRequest();
getPatientsRequest.open('GET', patientUri); 
getPatientsRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            let result = JSON.parse(getPatientsRequest.response);
            for (let i = 0; i < result.length; ++i) {
                if ((result[i]["blockedBy"] == "" || 
                    result[i]["blockedBy"] == null) && result[i]["isDeleted"] == false)
                patients.push(result[i])
            }
            populateTablePatient()
        } else {
            alert("Greska prilikom kreiranja korisnika.")
        }
    }
}

getPatientsRequest.send();

function populateTableSpecialization() {
    let table = document.createElement("table")
    let row = table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "ID"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "NAME"

    for (let i = 0; i < specializations.length; ++i) {
        let spec = specializations[i]

        let row = table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);

        cell1.innerHTML = spec["id"];
        cell2.innerHTML = spec["name"];
    }
    document.getElementById("specialization-table").appendChild(table);
}

function populateTablePatient() {
    let table = document.createElement("table")
    let row = table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "ID"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "NAME"
    let cell3 = row.insertCell(1);
    cell3.innerHTML = "SURNAME"

    for (let i = 0; i < patients.length; ++i) {
        let patient = patients[i]

        let row = table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);
        let cell3 = row.insertCell(1);

        cell1.innerHTML = patient["id"];
        cell2.innerHTML = patient["name"];
        cell3.innerHTML = patient["surname"];
    }
    document.getElementById("patient-table").appendChild(table);
}

document.getElementById("examination").addEventListener("click", function(){
    event.preventDefault()
    appointExamination()
});

document.getElementById("operation").addEventListener("click", function(){
    event.preventDefault()
    appointOperation()
});

function appointOperation() {
    let patientId = document.getElementById("patient-id").value
    let specId = document.getElementById("spec-id").value
    let duration = document.getElementById("duration").value

    let dto = {
        "isExamination" : false,
        "patientId" : patientId,
        "specializationId" : specId,
        "duration" : duration,
    }

    let appointRequest = new XMLHttpRequest();
    appointRequest.open('PUT', urgentAppointmentUri); 
    appointRequest.setRequestHeader('Content-Type', 'application/json');
    appointRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                alert("Appointment successful!")
                location.reload();
            } else {
                alert("Appointment failed! Doctor is busy.")
            }
        }
    }
    appointRequest.send(JSON.stringify(dto));
}

function appointExamination() {
    let patientId = document.getElementById("patient-id").value
    let specId = document.getElementById("spec-id").value

    let dto = {
        "isExamination" : true,
        "duration" : 15,
        "patientId" : patientId,
        "specializationId" : specId,
    }

    let appointRequest = new XMLHttpRequest();
    appointRequest.open('PUT', urgentAppointmentUri); 
    appointRequest.setRequestHeader('Content-Type', 'application/json');
    appointRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                alert("Appointment successful!")
                location.reload();
            } else {
                alert("Appointment failed! Doctor is busy.")
            }
        }
    }
    appointRequest.send(JSON.stringify(dto));
}


