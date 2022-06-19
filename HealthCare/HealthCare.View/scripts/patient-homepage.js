let getPatientUri = "https://localhost:7195/api/Patient/patientId=";
let rateHosptialUri = "https://localhost:7195/api/Answer/rateHospital"

let userId = sessionStorage.getItem("userId");


currentTime();


let getDoctorRequest = new XMLHttpRequest();
getDoctorRequest.open('GET', getPatientUri + userId);
getDoctorRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            let user = JSON.parse(getDoctorRequest.responseText);
            console.log(user);
            fillLabels(user);
            sessionStorage.setItem("user", JSON.stringify(user));
        } else {
            alert("Greska prilikom ucitavanja korisnika.");
        }
    }
}
getDoctorRequest.send();


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

let rateHospitalBtn = document.getElementById("rate-hospital-btn");
let container = document.getElementById("container");


let modalQuestion = document.getElementById("modalQuestion");
rateHospitalBtn.onclick = function rateQuestion()
{
    modalQuestion.setAttribute("style", "display:block");
    container.classList.add("wrapper");
}
let btnCloseQuestion = document.getElementById("close-btn-question");

btnCloseQuestion.onclick = (e) => {
    modalQuestion.setAttribute("style", "display:none");
    container.classList.remove("wrapper");
};

let submitBtnQuestion = document.getElementById("submit-btn-question");

submitBtnQuestion.addEventListener("click", function(e) {
    let answer1Mark = document.getElementsByName("question1");
    let answer2Mark = document.getElementsByName("question2"); 
    let answer3Mark = document.getElementsByName("question3"); 
    let answer4Mark = document.getElementsByName("question4"); 
    let text1 = document.getElementById("text1"); 
    let text2 = document.getElementById("text2"); 
    let text3 = document.getElementById("text3"); 
    let text4 = document.getElementById("text4"); 
    
    let answers =  {
        "answer1": {
          "id": 0,
          "answerText": text1.value,
          "evaluation": answer1Mark[0].checked ? 5 : (answer1Mark[1].checked ? 4 : (answer1Mark[2].checked ? 3 : (answer1Mark[3].checked ? 2 : 1))),
          "doctorId": null,
          "patientId": userId,
          "questionId": 1
        },
        "answer2": {
          "id": 0,
          "answerText": text2.value,
          "evaluation": answer2Mark[0].checked ? 5 : answer2Mark[1].checked ? 4 : answer2Mark[2].checked ? 3 : answer2Mark[3].checked ? 2 : 1,
          "doctorId": null,
          "patientId": userId,
          "questionId": 2
        },
        "answer3": {
            "id": 0,
            "answerText": text3.value,
            "evaluation": answer3Mark[0].checked ? 5 : answer3Mark[1].checked ? 4 : answer3Mark[2].checked ? 3 : answer3Mark[3].checked ? 2 : 1,
            "doctorId": null,
            "patientId": userId,
            "questionId": 3
          },
          "answer4": {
            "id": 0,
            "answerText": text4.value,
            "evaluation": answer4Mark[0].checked ? 5 : answer4Mark[1].checked ? 4 : answer4Mark[2].checked ? 3 : answer4Mark[3].checked ? 2 : 1,
            "doctorId": null,
            "patientId": userId,
            "questionId": 4
          }
      }
    console.log(JSON.stringify(answers));
    let rateHospitalRequest = new XMLHttpRequest();
    rateHospitalRequest.open('POST', rateHosptialUri); 
    rateHospitalRequest.setRequestHeader('Content-Type', 'application/json');
    rateHospitalRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                
            } else {
                alert("Greska prilikom ocenjivanja doktora.")
            }
            modal.setAttribute("style", "display:none");
            container.classList.remove("wrapper");
        }
    }
    rateHospitalRequest.send(JSON.stringify(answers));
});
  
