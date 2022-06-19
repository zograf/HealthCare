let createUri = "https://localhost:7195/api/Patient/create"

currentTime();
let user = JSON.parse(sessionStorage.getItem("user"));
console.log(user);
setNameOnCorner(user);

function createPatient() {
    birthDate = "2000-01-01T00:00:00Z"

    let medicalRecordDTO = {
        "height": document.getElementById("height").value,
        "weight": document.getElementById("weight").value,
        "bedriddenDiseases": document.getElementById("disease").value,
        "patientId": 0,
        "isDeleted": false
    }

    let loginDTO = {
        "username" : document.getElementById("username").value,
        "password" : document.getElementById("password").value,
    }

    let patient =  {
        "id": 0,
        "name": document.getElementById("name").value,
        "surname": document.getElementById("surname").value,
        "email": document.getElementById("email").value,
        "birthDate": birthDate,
        "phone": document.getElementById("phone").value,
        "medicalRecordDTO": medicalRecordDTO,
        "loginDTO": loginDTO,
    }


    let createPatientRequest = new XMLHttpRequest();
    createPatientRequest.open('POST', createUri); 
    createPatientRequest.setRequestHeader('Content-Type', 'application/json');
    createPatientRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                alert("Patient successfully created!");
            } else {
                alert("Greska prilikom kreiranja korisnika.")
            }
        }
    }
    createPatientRequest.send(JSON.stringify(patient));
}

