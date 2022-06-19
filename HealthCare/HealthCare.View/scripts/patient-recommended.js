let doctorsUri = "https://localhost:7195/api/Doctor";
let recommendedUri = "https://localhost:7195/api/RecommendExamination/recommend";
let examinationUri = "https://localhost:7195/api/Examination/create"


currentTime();
let user = JSON.parse(sessionStorage.getItem("user"));
console.log(user);
setNameOnCorner(user);


let doctors; 
let getDoctorsRequest = new XMLHttpRequest();
getDoctorsRequest.open('GET', doctorsUri); 

getDoctorsRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            doctors = JSON.parse(getDoctorsRequest.responseText);
            console.log(doctors);
            fillDoctorSelect(doctors);
        } else {
            alert("Greska prilikom ucitavanja doktora.")
        }
    }
}

getDoctorsRequest.send();

let doctorSelect = document.getElementById("doctor-names");

function fillDoctorSelect(doctors)
{
    doctors.forEach(doctor => {
        const option = document.createElement('option');
        option.textContent = doctor.name + " " + doctor.surname;
        option.value = doctor.id;
        doctorSelect.appendChild(option);
    });
}


const hourSelect1 = document.getElementById("hourSelect1");
const minuteSelect1 = document.getElementById("minuteSelect1");
const hourSelect2 = document.getElementById("hourSelect2");
const minuteSelect2 = document.getElementById("minuteSelect2");

function populateHours () 
{
    for(let i = 0; i < 24; i++)
    {
        const option1 = document.createElement('option');
        option1.textContent = i;
        option1.value = i;
        const option2 = document.createElement('option');
        option2.textContent = i;
        option2.value = i;
        hourSelect1.appendChild(option1);
        hourSelect2.appendChild(option2);
    }
}

populateHours();

function populateMinutes () 
{
    for(let i = 0; i < 60; i+=15)
    {
        const option1 = document.createElement('option');
        option1.textContent = i;
        option1.value = i;
        const option2 = document.createElement('option');
        option2.textContent = i;
        option2.value = i;
        minuteSelect1.appendChild(option1);
        minuteSelect2.appendChild(option2);
    }
}

populateMinutes();

let submitBtn = document.getElementById("submitBtn");
let priority = document.getElementsByName('priority');
let dateUntil = document.getElementById("date-until")
     
let appointmentBox = document.getElementById('examination-select');


submitBtn.addEventListener("click", function(e) {
    let hours1 = formatDate(hourSelect1.value);
    let minute1 = formatDate(minuteSelect1.value);
    let hours2 = formatDate(hourSelect2.value);
    let minute2 = formatDate(minuteSelect2.value);
    let timeFrom = "1000-01-01T" + hours1 + ":" + minute1 + ":00.000Z";
    let timeTo = "1000-01-01T" + hours2 + ":" + minute2 + ":00.000Z";
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    today = yyyy + "-" + mm + "-" + dd; 
    if(today > dateUntil.value)
    {
        alert("Datum mora biti u buducnosti");
        return;
    }

    let recommendedDTO =  {
        "patientId": user.id,
        "doctorId": doctorSelect.value,
        "timeFrom": timeFrom,
        "timeTo": timeTo,
        "lastDate": dateUntil.value + "T10:00:00.000Z",
        "isDoctorPriority": priority[0].checked ? true : false
    }
    console.log(JSON.stringify(recommendedDTO));


    let recommendedRequest = new XMLHttpRequest();
    recommendedRequest.open('POST', recommendedUri); 
    recommendedRequest.setRequestHeader('Content-Type', 'application/json');
    recommendedRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let recommendedExaminations = JSON.parse(recommendedRequest.responseText);
                console.log(recommendedExaminations);
                
                appointmentBox.innerHTML = "";
                recommendedExaminations.forEach(examination => {
                    populateAppointments(examination);
                });
            } else {
                alert("Greska prilikom rezervisanja pregleda.")
            }
        }
    }
    recommendedRequest.send(JSON.stringify(recommendedDTO));
});
     
function formatDate(param)
{
    if(param < 10)
    {
        param = "0" + param;
    }
    return param;
}

function populateAppointments(appointment)
{
    let examinationBox = document.createElement("div");
    examinationBox.classList.add("examination-box");
    
    let examination = document.createElement("div");
    examination.classList.add("examination");
    
    let examinationDatetimeBox = document.createElement("div");
    examinationDatetimeBox.classList.add("examination-datetime-box");
    
    let examinationDatetimeIcon = document.createElement("div");
    examinationDatetimeIcon.classList.add("examination-datetime-icon");
    
    let datetimeIcon = document.createElement("i");
    datetimeIcon.classList.add("fa-solid");
    datetimeIcon.classList.add("fa-calendar-days");
    
    
    
    let examinationDatetime = document.createElement("div");
    examinationDatetime.classList.add("examination-datetime");
    
    let date = document.createElement("p");
    date.innerText = appointment.startTime.split("T")[0];
    
    
    let time = document.createElement("p");
    timelist = appointment.startTime.split("T")[1].split("Z")[0].split(":");
    time.innerText = timelist[0] + ":" + timelist[1] + "h";
    
    examinationDatetimeIcon.appendChild(datetimeIcon);
    
    examinationDatetime.appendChild(date);
    examinationDatetime.appendChild(time);
    
    examinationDatetimeBox.appendChild(examinationDatetimeIcon);
    examinationDatetimeBox.appendChild(examinationDatetime);
    
    examination.appendChild(examinationDatetimeBox);
    
    let examinationDoctorBox = document.createElement("div");
    examinationDoctorBox.classList.add("examination-doctor-box");

    let examinationDoctorIcon = document.createElement("div");
    examinationDoctorIcon.classList.add("examination-datetime-icon");
    
    let doctorIcon = document.createElement("i");
    doctorIcon.classList.add("fa-solid");
    doctorIcon.classList.add("fa-user-doctor");
    
    examinationDoctorIcon.appendChild(doctorIcon);
    examinationDoctorBox.appendChild(examinationDoctorIcon);
        
    let examinationDoctorInfo = document.createElement("div");
    examinationDoctorInfo.classList.add("examination-doctor-info");


    let name = document.createElement("p");

    doctors.forEach(doctor => {
        if(doctor.id === appointment.doctorId)
        {
            name.innerText = "Dr " + doctor.name + " " + doctor.surname;
        }
    });

    
    examinationDoctorInfo.appendChild(name);
    
    examinationDoctorBox.appendChild(examinationDoctorInfo);
    
    examination.appendChild(examinationDoctorBox);
    
    examinationBox.appendChild(examination);
    
    let examinationButtons = document.createElement("div");
    examinationButtons.classList.add("examination-buttons");

    let reserveBtn = document.createElement("button");
    reserveBtn.classList.add("reserveBtn");
    reserveBtn.innerText += "        Reserve";

    reserveBtn.onclick = function() {reserve(appointment.doctorId, appointment.startTime); };

    examinationButtons.appendChild(reserveBtn);

    
    examinationBox.appendChild(examinationButtons);

    appointmentBox.appendChild(examinationBox);
}

function reserve(selectedDoctorId, date)
{
    console.log(selectedDoctorId);
    let examination =  {
        "doctorId": selectedDoctorId,
        "examinationId": 0,
        "patientId": user.id,
        "startTime": date,
        "isPatient": true
    }
    console.log(JSON.stringify(examination));

    
    let makeExaminationRequest = new XMLHttpRequest();
    makeExaminationRequest.open('POST', examinationUri); 
    makeExaminationRequest.setRequestHeader('Content-Type', 'application/json');
    makeExaminationRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let examination = JSON.parse(makeExaminationRequest.responseText);
            } else {
                alert("Greska prilikom rezervisanja pregleda.")
            }
        }
    }
    makeExaminationRequest.send(JSON.stringify(examination));
}