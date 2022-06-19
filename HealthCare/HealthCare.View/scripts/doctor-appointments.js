let scheduleUrl = "https://localhost:7195/api/Appointment/schedule"
let patientsUri = "https://localhost:7195/api/Patient"
let roomsUri = "https://localhost:7195/api/Room"
let createDaysOffRequestUri = "https://localhost:7195/api/DaysOffRequest/create"

let scheduleRequest = new XMLHttpRequest();
let scheduleTable = document.getElementById("schedule-tbody");
let appointmentBox = document.getElementById('examination-select');

let doctorId = sessionStorage.getItem("userId");

let rooms;

let getRoomsRequest = new XMLHttpRequest();
getRoomsRequest.open('GET', roomsUri); 

getRoomsRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            rooms = JSON.parse(getRoomsRequest.responseText);
            

        } else {
            alert("Greska prilikom ucitavanja soba.")
        }
    }
}
getRoomsRequest.send();

let patients; 

let getPatientsRequest = new XMLHttpRequest();
getPatientsRequest.open('GET', patientsUri); 

getPatientsRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            patients = JSON.parse(getPatientsRequest.responseText);
        } else {
            alert("Greska prilikom ucitavanja doktora.")
        }
    }
}

getPatientsRequest.send();

scheduleRequest.onreadystatechange = function (e) {
    if (this.readyState == 4) {
        if (this.status == 200) {
            let appointments = JSON.parse(this.responseText);
            appointments.sort(sortByProperty("startTime"));
            console.log(appointments);
            for(let i = 0; i < appointments.length; i++)
            {
                if(appointments[i].isDeleted == false)
                    populateAppointments(appointments[i]);
            }
        } else {
            alert("Can't get your schedule at the moment :(")
        }
    }
}

scheduleRequest.open("GET", scheduleUrl.concat("?DoctorId=" + doctorId + "&Date=" + getCurrentDate() + "&ThreeDays=true"));
scheduleRequest.send();


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

    let appointmentTypeBox = document.createElement("div");
    appointmentTypeBox.classList.add("appointment-type-box");

    let type = document.createElement("p");
    type.innerText = appointment.type == "1" ? "examination" : "operation";
    type.classList.add("examination-datetime");

    appointmentTypeBox.appendChild(type);
    examination.appendChild(appointmentTypeBox);
    
    let examinationPatientBox = document.createElement("div");
    examinationPatientBox.classList.add("examination-doctor-box");

    let examinationPatientIcon = document.createElement("div");
    examinationPatientIcon.classList.add("examination-datetime-icon");
    
    let patientIcon = document.createElement("i");
    patientIcon.classList.add("fa-solid");
    patientIcon.classList.add("fa-user");
    
    examinationPatientIcon.appendChild(patientIcon);
    examinationPatientBox.appendChild(examinationPatientIcon);
        
    let examinationPatientInfo = document.createElement("div");
    examinationPatientInfo.classList.add("examination-doctor-info");


    let name = document.createElement("p");
    name.classList.add("examination-doctor-name");
    let currentPatient;
    patients.forEach(patient => {
        if(patient.id === appointment.patientId)
        {
            name.innerText = patient.name + " " + patient.surname;
            currentPatient = patient;
        }
    });

    
    examinationPatientInfo.appendChild(name);
    examinationPatientBox.onclick = function() {showPatientsMedicalRecord(currentPatient)};
    
    let room = document.createElement("p");

    rooms.forEach(r => {
        if(r.id === appointment.roomId)
        {
            room.innerText = "Room " + r.roomName;
        }
    });
    
    examinationPatientInfo.appendChild(room);
    
    examinationPatientBox.appendChild(examinationPatientInfo);
    
    examination.appendChild(examinationPatientBox);
    
    examinationBox.appendChild(examination);
    
    let examinationButtons = document.createElement("div");
    examinationButtons.classList.add("examination-buttons");

    let editBtn = document.createElement("button");
    editBtn.classList.add("editBtn");
    editBtn.innerText += "        Edit";

    let editIcon = document.createElement("i");
    editIcon.classList.add("fa-solid");
    editIcon.classList.add("fa-pencil");
    editIcon.classList.add("edit-icon");
    //editBtn.innerHTML = editIcon;
    editIcon.onclick = function() {}
    examinationButtons.appendChild(editIcon);
    
    let deleteBtn = document.createElement("button");
    deleteBtn.classList.add("deleteBtn");
    //deleteBtn.innerHTML = <i class="fa-solid fa-trash"></i>;
    deleteBtn.innerText += "        Delete";

    let deleteIcon = document.createElement("i");
    deleteIcon.classList.add("fa-solid");
    deleteIcon.classList.add("fa-trash");
    deleteIcon.classList.add("edit-icon")
    deleteIcon.onclick = function() {deleteAppointment(appointment.id)};

    examinationButtons.appendChild(deleteIcon);
    
    examinationBox.appendChild(examinationButtons);

    
    

    let startButtonBox = document.createElement("div");
    startButtonBox.classList.add("start-button-box");
    let startBtn = document.createElement("button");
    startBtn.classList.add("startBtn");
    startBtn.innerText += "START";
    
    startBtn.onclick = function() {startAppointment(appointment.id)};

    startButtonBox.appendChild(startBtn);
    examinationBox.appendChild(startButtonBox);

    appointmentBox.appendChild(examinationBox);
    
}


function getCurrentDate() {
    let today = new Date();

    let dd = today.getDate();
    let mm = today.getMonth() + 1;
    let yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    
    return yyyy + '-' + mm + '-' + dd + "T00:00:00";
}

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

    let welcomeName = document.getElementById("welcome-name");
    welcomeName.innerText = "Dr. " + user.name + "!";
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

function startAppointment(id) {

}

function showPatientsMedicalRecord(patient) {
    console.log(patient.id);
}

function requestFreeDays() 
{
    let fromDate = document.getElementById("from-date").value;
    let toDate = document.getElementById("to-date").value;

    if (Date.parse(toDate) < Date.parse(fromDate))  
    {
        alert("Please enter valid date range.");
        return;
    }
        

    if (Date.parse(fromDate) < new Date())
    {
        alert("Dates must be in the future.");
        return;
    }
        

    let urgent = document.getElementById("urgent").checked;
    let reason = document.getElementById("reason").value;

    if (reason == "" || fromDate == "" || toDate == "")
    {
        alert("You must fill all input fields.");
        return;
    }
        

    let daysOffRequest = {
        "comment": reason,
        "isUrgent": urgent,
        "doctorId": doctorId,
        "from": fromDate,
        "to": toDate
    }

    let createDaysOffRequest = new XMLHttpRequest();
    createDaysOffRequest.open('POST', createDaysOffRequestUri); 
    createDaysOffRequest.setRequestHeader('Content-Type', 'application/json');
    createDaysOffRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                let approved = urgent ? " and approved" : "";
                alert("Request successfully sent" + approved + ".");
                clearDaysOffForm();
                location.reload();
            } else {
                alert(this.responseText)
            }
        }
    }
    createDaysOffRequest.send(JSON.stringify(daysOffRequest));
    
}

function clearDaysOffForm()
{
    document.getElementById("from-date").value = "";
    document.getElementById("to-date").value = "";
    document.getElementById("urgent").checked = false;
    document.getElementById("reason").value = "";
}

