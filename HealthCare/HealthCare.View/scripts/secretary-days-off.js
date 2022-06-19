let requestsUri = "https://localhost:7195/api/DaysOffRequest/"
let approveUri = "https://localhost:7195/api/DaysOffRequest/approve/"
let rejectUri = "https://localhost:7195/api/DaysOffRequest/reject/"
let doctorUri = "https://localhost:7195/api/Doctor/doctorId="

/*
currentTime();
let user = JSON.parse(sessionStorage.getItem("user"));
console.log(user);
setNameOnCorner(user);
*/

var requests = [];

let getRequests = new XMLHttpRequest();
getRequests.open('GET', requestsUri); 
getRequests.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            let result = JSON.parse(getRequests.response);
            for (let i = 0; i < result.length; ++i) {
                if (result[i]["state"] == 0)
                    requests.push(result[i])
            }
            populateTable()
        } else {
            alert("Invalid request ID.")
        }
    }
}

getRequests.send();

async function populateTable() {
    let table = document.createElement("table")
    let row = table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "ID"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "FROM"
    let cell3 = row.insertCell(2);
    cell3.innerHTML = "TO"
    let cell4 = row.insertCell(3);
    cell4.innerHTML = "DOCTOR NAME"

    for (let i = 0; i < requests.length; ++i) {
        let request = requests[i]

        let row = table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);
        let cell3 = row.insertCell(2);
        let cell4 = row.insertCell(3);

        cell1.innerHTML = request["id"];
        cell2.innerHTML = request["from"].split("T")[0];
        cell3.innerHTML = request["to"].split("T")[0];
        cell4.innerHTML = await getDoctorById(request["doctorId"]);
    }
    document.getElementById("request-table").appendChild(table);
}

async function getDoctorById(id) {
    let doctor = await fetch(doctorUri + id)
    .then(response => response.json())
    return doctor["name"] + " " + doctor["surname"]
}

document.getElementById("approve").addEventListener("click", function(){
    event.preventDefault()
    approve()
});

document.getElementById("reject").addEventListener("click", function(){
    event.preventDefault()
    reject()
});

function reject() {
    let requestId = document.getElementById("request-id").value
    let message = document.getElementById("message").value
    if (message.length < 5) {
        alert("You must provide a message.")
        return;
    }

    let dto = {
        "id" : requestId,
        "comment" : message,
    }

    let request = new XMLHttpRequest();
    request.open('PUT', rejectUri); 
    request.setRequestHeader('Content-Type', 'application/json');
    request.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                alert("Reject successful!")
                location.reload();
            } else {
                alert("Reject failed!")
            }
        }
    }
    request.send(JSON.stringify(dto));
}

function approve() {
    let requestId = document.getElementById("request-id").value

    let request = new XMLHttpRequest();
    request.open('PUT', approveUri + requestId); 
    request.setRequestHeader('Content-Type', 'application/json');
    request.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                alert("Approve successful!")
                location.reload();
            } else {
                alert("Approve failed!")
            }
        }
    }
    request.send();
}
