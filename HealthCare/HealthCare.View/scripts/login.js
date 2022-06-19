let loginUri = "https://localhost:7195/api/Credentials/login?"

let loggedCreds;

let loginForm = document.getElementById("login-form");

// window.onload = function() {
//     if (sessionStorage.getItem("loggedCreds") != null) {
//         loggedCreds = sessionStorage.getItem("loggedCreds");
//         redirectUser();
//     } 
//   };


loginForm.addEventListener("submit", function(e) {
    e.preventDefault();

    let username = document.getElementById("log-username").value.trim();
    let password = document.getElementById("log-password").value.trim();
    

    if (username=="" || password=="") {
        alert("Please fill all input fields.")
    } else {
        processLogin(username, password);
    }

    clearLogin();
});

function clearLogin() {
    document.getElementById("log-username").value = null;
    document.getElementById("log-password").value = null;
}

function processLogin(username, password) {
    
    let getRequest = new XMLHttpRequest();

    getRequest.onreadystatechange = function () {
        if (this.readyState == 4) {
            if (this.status == 200) {
                // remember user
                sessionStorage.setItem("loggedCreds", this.responseText)
                loggedCreds = JSON.parse(this.responseText);
                console.log(loggedCreds);
                
                setUserId();
                redirectUser();
    
            } else {
                // let erroBanner = document.getElementById("error-banner");
                // erroBanner.innerText = "Nepostojeci korisnik. Proverite da li ste uneli tacne podatke.";
                // erroBanner.style.display = "block";
                alert(this.responseText);
                
            }
        }
    }

    getRequest.open("GET", loginUri.concat("Username=" + username + "&Password=" + password), true);
    getRequest.send();

}

function setUserId() {
    let userId;
    switch (loggedCreds.userRole.roleName) {
        case "manager":
            userId = loggedCreds.managerId;
            break;
        case "doctor":
            userId = loggedCreds.doctorId;
            break;
        case "patient":
            userId = loggedCreds.patientId;
            break;
        case "secretary":
            userId = loggedCreds.secretaryId;
            break;
    }
    sessionStorage.setItem("userId", userId);
}

function redirectUser() {
    switch (JSON.parse(sessionStorage.getItem("loggedCreds")).userRole.roleName) {
        case "manager":
            window.location.href = "manager-homepage.html";
            break;
        case "doctor":
            window.location.href = "doctor-homepage.html";
            break;
        case "patient":
            window.location.href = "patient-homepage.html";
            break;
        case "secretary":
            window.location.href = "secretary-homepage.html";
            break;
    }
}