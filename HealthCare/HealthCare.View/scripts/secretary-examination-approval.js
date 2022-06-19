let rejectUri = "https://localhost:7195/api/ExaminationApproval/reject/"
let approveUri = "https://localhost:7195/api/ExaminationApproval/approve/"
let examinationApprovalUri = "https://localhost:7195/api/ExaminationApproval/"

/*
currentTime();
let user = JSON.parse(sessionStorage.getItem("user"));
console.log(user);
setNameOnCorner(user);
*/

var created = [];

let getApprovalsRequest = new XMLHttpRequest();
getApprovalsRequest.open('GET', examinationApprovalUri); 
getApprovalsRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            var approvals = JSON.parse(getApprovalsRequest.response);
            for (let i = 0; i < approvals.length; ++i) {
                let approval = approvals[i]
                if (approval["state"] == "created")
                    created.push(approval)
            }
            populateTable()
        } else {
            alert("Greska prilikom kreiranja korisnika.")
        }
    }
}

getApprovalsRequest.send();

function populateTable() {
    let table = document.createElement("table")
    let row = table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "ID"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "TYPE"
    for (let i = 0; i < created.length; ++i) {
        let approval = created[i]

        let row = table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);

        cell1.innerHTML = approval["id"];
        if (approval["oldExaminationId"] == approval["newExaminationId"])
            cell2.innerHTML = "Deletion";
        else
            cell2.innerHTML = "Creation";

    }
    document.getElementById("created-table").appendChild(table);
}
document.getElementById("approve").addEventListener("click", function(){
    event.preventDefault()
    approve()
});

document.getElementById("reject").addEventListener("click", function(){
    event.preventDefault()
    reject()
});

function approve() {
    let id = document.getElementById("approval-id").value

    let approveRequest = new XMLHttpRequest();
    approveRequest.open('PUT', approveUri + id); 
    approveRequest.setRequestHeader('Content-Type', 'application/json');
    approveRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                alert("Approve successful!")
                location.reload();
            } else {
                alert("Greska prilikom kreiranja korisnika.")
            }
        }
    }
    approveRequest.send(id);
}

function reject() {
    let id = document.getElementById("approval-id").value

    let rejectRequest = new XMLHttpRequest();
    rejectRequest.open('PUT', approveUri + id); 
    rejectRequest.setRequestHeader('Content-Type', 'application/json');
    rejectRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                alert("Reject successful!")
                location.reload();
            } else {
                alert("Greska prilikom kreiranja korisnika.")
            }
        }
    }
    rejectRequest.send(id);
}


