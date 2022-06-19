let createUri = "https://localhost:7195/api/ReferralLetter/createAppointment/"
let referralLetterUri = "https://localhost:7195/api/ReferralLetter/"

/*
currentTime();
let user = JSON.parse(sessionStorage.getItem("user"));
console.log(user);
setNameOnCorner(user);
*/

var letters = [];

let getReferralLettersRequest = new XMLHttpRequest();
getReferralLettersRequest.open('GET', referralLetterUri); 
getReferralLettersRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            var result = JSON.parse(getReferralLettersRequest.response);
            for (let i = 0; i < result.length; ++i) {
                if (result[i]["state"] == "created")
                    letters.push(result[i])
            }
            populateTable()
        } else {
            alert("Greska prilikom kreiranja korisnika.")
        }
    }
}

getReferralLettersRequest.send();

function populateTable() {
    let table = document.createElement("table")
    let row = table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "ID"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "PATIENT ID"
    let cell3 = row.insertCell(1);
    cell3.innerHTML = "FROM DOCTOR ID"
    let cell4 = row.insertCell(1);
    cell4.innerHTML = "TYPE"
    for (let i = 0; i < letters.length; ++i) {
        let letter = letters[i]

        let row = table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);
        let cell3 = row.insertCell(2);
        let cell4 = row.insertCell(3);

        cell1.innerHTML = letter["id"];
        cell2.innerHTML = letter["patientId"];
        cell3.innerHTML = letter["fromDoctorId"];
        if (letter["toDoctorId"] == null)
            cell4.innerHTML = "By Specialization"
        else
            cell4.innerHTML = "Specific Doctor"
    }
    document.getElementById("letter-table").appendChild(table);
}
document.getElementById("appoint").addEventListener("click", function(){
    event.preventDefault()
    appoint()
});

function appoint() {
    let id = document.getElementById("letter-id").value
    let time = document.getElementById("letter-time").value

    let dto = {
        "startTime" : time,
        "referralId" : id,
    }

    let appointRequest = new XMLHttpRequest();
    appointRequest.open('PUT', createUri); 
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


