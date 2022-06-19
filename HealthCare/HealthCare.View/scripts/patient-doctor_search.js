let doctorsUri = "https://localhost:7195/api/Doctor/search"
let examinationUri = "https://localhost:7195/api/Examination/create"

currentTime();
let user = JSON.parse(sessionStorage.getItem("user"));
console.log(user);
setNameOnCorner(user);


let doctors;
let searchBtn = document.getElementById("search-btn");
let name = document.getElementById("name-search");
let surname = document.getElementById("surname-search");
let specialization = document.getElementById("specialization-search");
let sortParam = document.getElementById("sort-params");

searchBtn.addEventListener("click", function (e) {
    let params = "?";
    if (name.value != "") params += "Name=" + name.value + "&";
    if (surname.value != "") params += "Surname=" + surname.value + "&";
    if (specialization.value != "") params += "Specialization=" + specialization.value + "&";
    params += "SortParam=" + sortParam.value;
    console.log(doctorsUri + params);
    let getDoctorsRequest = new XMLHttpRequest();
    getDoctorsRequest.open('GET', doctorsUri + params);

    getDoctorsRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                doctors = JSON.parse(getDoctorsRequest.responseText);
                doctorSelect.innerHTML = "";
                doctors.forEach(doctor => {
                    makeDoctorCard(doctor);
                });
            } else {
                alert("Greska prilikom ucitavanja doktora.")
            }
        }
    }

    getDoctorsRequest.send();
});





let doctorSelect = document.getElementById("doctor-select");


function makeDoctorCard(doctor) {
    let card = document.createElement('div');
    card.classList.add('doctor-card');

    card.onclick = function () {
        selectedDoctorId = doctor.id
    }

    let imageDiv = document.createElement('div');
    imageDiv.classList.add('image-doctor');
    let image = document.createElement('img');
    image.src = "../img/profile_pic.png";
    image.alt = "profile_picture";
    image.classList.add("user-nav__box--img");
    imageDiv.appendChild(image);

    let name = document.createElement('div');
    card.classList.add('doctor-name');
    let nameSurname = document.createElement('p');
    nameSurname.classList.add('name-surname');
    nameSurname.innerText = doctor.name + " " + doctor.surname;
    let specialization = document.createElement('p');
    specialization.classList.add('specialization');
    specialization.innerText = doctor.specialization.name;
    let mail = document.createElement('p');
    mail.classList.add('mail');
    mail.innerText = doctor.email;
    let btn = document.createElement('button');
    btn.innerText = "Choose";
    btn.classList.add("choose-btn");
    btn.onclick = function() {openModal(doctor)};
    name.appendChild(nameSurname);
    name.appendChild(specialization);
    name.appendChild(mail);
    name.appendChild(btn);


    card.appendChild(imageDiv);
    card.appendChild(name);
    card.setAttribute("doctorId", doctor.id);
    card.setAttribute("tabindex", -1);

    doctorSelect.appendChild(card);

}
let modal = document.getElementById("modal");
let container = document.getElementById("container");
function openModal(doctor)
{
    console.log(doctor.id)
    modal.setAttribute("style", "display:block");
    container.classList.add("wrapper");
    let title = document.getElementById('modal-title');
    title.innerText = doctor.name + " " + doctor.surname;
    let submitBtn = document.getElementById('submit-btn');
    submitBtn.onclick = function() {reserve(doctor.id);}
}
let btnClose = document.getElementById("close-btn");

btnClose.onclick = (e) => {
    modal.setAttribute("style", "display:none");
    container.classList.remove("wrapper");
};

const yearSelect = document.getElementById("yearSelect");
const monthSelect = document.getElementById("monthSelect");
const daySelect = document.getElementById("daySelect");
const hourSelect = document.getElementById("hourSelect");
const minuteSelect = document.getElementById("minuteSelect");

const months = ['January', 'February', 'March', 'April', 'May', 'June',
'July', 'August', 'September', 'October', 'November', 'December'];

(function populateMonths(){
    for(let i = 0; i < months.length; i++)
    {
        const option = document.createElement('option');
        option.textContent = months[i];
        option.value = i + 1;
        monthSelect.appendChild(option);
    }
    monthSelect.value = "Month";
})();

function populateDays(month) {
    while(daySelect.firstChild)
    {
        daySelect.removeChild(daySelect.firstChild);
    }
    let dayNum;
    if(month === '1' || month === '3' || month === '5' || 
    month === '7' || month === '8' || month === '10' ||
     month === '12')
    {
        dayNum = 31;
    }   
    else if(month === '4' || month === '6' || month === '9' || 
    month === '11')
    {
        dayNum = 30;
    }   
    else {
        dayNum = 29;
    }
    for(let i = 1; i <= dayNum; i++)
    {
        const option = document.createElement('option');
        option.textContent = i;
        option.value = i;
        daySelect.appendChild(option);
    }
}

function populateYears()
{
    let year = new Date().getFullYear();
    for(let i = 0; i <= 5; i++)
    {
        const option = document.createElement('option');
        option.textContent = year + i;
        option.value = year + i;
        yearSelect.appendChild(option);
    }
}

populateDays(monthSelect.value);
populateYears();

yearSelect.onchange = () => {
    populateDays(monthSelect.value);
}

monthSelect.onchange = () => {
    populateDays(monthSelect.value);
}

function populateHours () 
{
    for(let i = 0; i < 24; i++)
    {
        const option = document.createElement('option');
        option.textContent = i;
        option.value = i;
        hourSelect.appendChild(option);
    }
}

populateHours();

function populateMinutes () 
{
    for(let i = 0; i < 60; i+=15)
    {
        const option = document.createElement('option');
        option.textContent = i;
        option.value = i;
        minuteSelect.appendChild(option);
    }
}

populateMinutes();


function reserve(doctorId) 
{
    let day = formatDate(daySelect.value);
    let month = formatDate(monthSelect.value);
    let year = formatDate(yearSelect.value);
    let hours = formatDate(hourSelect.value);
    let minutes = formatDate(minuteSelect.value);
    let date = year + "-" + month + "-" + day + "T" + hours + ":" + minutes + ":00.000Z";
    
    let examination =  {
        "doctorId": doctorId,
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
                reloadUser()
            } else {
                alert("Greska prilikom rezervisanja pregleda.")
            }
        }
    }
    makeExaminationRequest.send(JSON.stringify(examination));
    modal.setAttribute("style", "display:none");
    container.classList.remove("wrapper");
}


function formatDate(param)
{
    if(param < 10)
    {
        param = "0" + param;
    }
    return param;
}