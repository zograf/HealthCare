let getPatientUri = "https://localhost:7195/api/Manager/";
let userId = sessionStorage.getItem("userId");


currentTime();


let getPatientRequest = new XMLHttpRequest();
getPatientRequest.open('GET', getPatientUri + userId);
getPatientRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            let user = JSON.parse(getPatientRequest.responseText);
            console.log(user);
            fillLabels(user);
            sessionStorage.setItem("user", JSON.stringify(user));
        } else {
            alert("Greska prilikom ucitavanja menadzera.");
        }
    }
}
getPatientRequest.send();


function fillLabels(user)
{
    setNameOnCorner(user);

    let bigName = document.getElementById("bigName");
    bigName.innerText = user.name + ' ' + user.surname;

    let welcomeName = document.getElementById("welcome-name");
    welcomeName.innerText = user.name + " !";

    let id = document.getElementById("userId");
    id.innerText = user.id;

    let dateOfBirth = document.getElementById("dateOfBirth");
    dateOfBirth.innerText = user.birthDate.split('T')[0];

    let mail = document.getElementById("mail");
    mail.innerText = user.email;

    let phone = document.getElementById("phone");
    phone.innerText = user.phone;

    let height = document.getElementById("height");
    height.innerText = user.medicalRecord.height;

    let weight = document.getElementById("weight");
    weight.innerText = user.medicalRecord.weight;

    let examinationNumber = document.getElementById("examination-number");
    examinationNumber.innerText = user.examinations.length;
    
}



  
