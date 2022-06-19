let getDoctorUri = "https://localhost:7195/api/Doctor/doctorId=";
let userId = sessionStorage.getItem("userId");
let days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"]


currentTime();


let getDoctorRequest = new XMLHttpRequest();
getDoctorRequest.open('GET', getDoctorUri + userId);
getDoctorRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            let user = JSON.parse(getDoctorRequest.responseText);
            console.log(user);
            fillLabels(user);
        } else {
            alert("Error occured while trying to get doctor's data.");
        }
    }
}
getDoctorRequest.send();


function fillLabels(user)
{
    let smallName = document.getElementById("smallName");
    smallName.innerText = user.name + ' ' + user.surname;

    let bigName = document.getElementById("bigName");
    bigName.innerText = user.name + ' ' + user.surname;

    let welcomeName = document.getElementById("welcome-name");
    welcomeName.innerText = "Dr. " + user.name + "!";

    let id = document.getElementById("userId");
    id.innerText = user.id;

    let dateOfBirth = document.getElementById("dateOfBirth");
    dateOfBirth.innerText = user.birthDate.split('T')[0];

    let mail = document.getElementById("mail");
    mail.innerText = user.email;

    let phone = document.getElementById("phone");
    phone.innerText = user.phone;

    let spec = document.getElementById("specialization");
    spec.innerText = user.specialization != null ? user.specialization.name : none;
    
    let examination = document.getElementById("examination-number");
    examination.innerText = user.examinations.length;

    let operation = document.getElementById("operation-number");
    operation.innerText = user.operations.length;
}


function currentTime() {
    let date = new Date(); 
    let hh = date.getHours();
    let mm = date.getMinutes();
    let ss = date.getSeconds();
    let session = "AM";
  
    if(hh == 0){
        hh = 12;
    }
    if(hh > 12){
        hh = hh - 12;
        session = "PM";
     }
  
     hh = (hh < 10) ? "0" + hh : hh;
     mm = (mm < 10) ? "0" + mm : mm;
     ss = (ss < 10) ? "0" + ss : ss;
      
     let time = hh + ":" + mm + ":" + ss + " " + session;
  
    document.getElementById("clock").innerText = time; 
    document.getElementById("date").innerText = date.toLocaleDateString();
    document.getElementById("year").innerText = days[date.getDay() - 1]; 
    let t = setTimeout(function(){ currentTime() }, 1000);
  }
  
