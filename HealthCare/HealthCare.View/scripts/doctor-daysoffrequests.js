daysOffUri = "https://localhost:7195/api/DaysOffRequest/byDoctor"

let requestsBox = document.getElementById('request-select');

let DOrequests;

let getDORequestsRequest = new XMLHttpRequest();
getDORequestsRequest.open('GET', daysOffUri + "/" + doctorId); 

getDORequestsRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            DOrequests = JSON.parse(getDORequestsRequest.responseText);
            console.log(DOrequests);
            DOrequests.sort(sortByProperty("from"));
            for(let i = 0; i < DOrequests.length; i++)
                {
                    if(DOrequests[i].isDeleted != false)
                        populateRequest(DOrequests[i]);
                }
        } else {
            alert("Error occured while trying to fetch your days off requests.")
        }
    }
}
getDORequestsRequest.send();

function populateRequest(request)
{
    let requestBox = document.createElement("div");
    requestBox.classList.add("examination-box");
    requestBox.classList.add("request-box");
    
    let requestElem = document.createElement("div");
    requestElem.classList.add("examination");
    requestElem.classList.add("request");
    
    let examinationDatetimeBox = document.createElement("div");
    examinationDatetimeBox.classList.add("examination-datetime-box");
    
    let examinationDatetimeIcon = document.createElement("div");
    examinationDatetimeIcon.classList.add("examination-datetime-icon");
    
    let datetimeIcon = document.createElement("i");
    datetimeIcon.classList.add("fa-solid");
    datetimeIcon.classList.add("fa-calendar-days");
    
    let examinationDatetime = document.createElement("div");
    examinationDatetime.classList.add("examination-datetime");
    
    let fromDate = document.createElement("p");
    fromDate.innerText = request.from.split("T")[0];
    let toDate = document.createElement("p");
    toDate.innerText = request.to.split("T")[0];

    
    examinationDatetimeIcon.appendChild(datetimeIcon);
    
    examinationDatetime.appendChild(fromDate);
    examinationDatetime.appendChild(toDate);
    
    examinationDatetimeBox.appendChild(examinationDatetimeIcon);
    examinationDatetimeBox.appendChild(examinationDatetime);
    
    requestElem.appendChild(examinationDatetimeBox);
    
    let urgentBox = document.createElement("div");
    urgentBox.classList.add("examination-doctor-box");
    urgentBox.classList.add("urgent-box");
    
        
    let urgent = document.createElement("div");
    urgent.classList.add("examination-doctor-info");
    urgent.classList.add("text-center");


    let isUrgent = document.createElement("p");
    isUrgent.innerText = request.isUrgent ? "urgent" : "not urgent";

    
    urgent.appendChild(isUrgent);
    
    urgentBox.appendChild(urgent);
    
    requestElem.appendChild(urgentBox);
    
    requestBox.appendChild(requestElem);

    let reasonBox = document.createElement("div");
    reasonBox.classList.add("appointment-type-box");
    reasonBox.classList.add("comment-box");

    let reasonTitle = document.createElement("p");
    reasonTitle.innerText = "comment:";
    reasonTitle.classList.add("reaquest-title");

    let reason = document.createElement("p");
    reason.innerText = request.comment;
    reason.classList.add("request-text");

    reasonBox.appendChild(reasonTitle);
    reasonBox.appendChild(reason);
    requestElem.appendChild(reasonBox);
    
    let statusBox = document.createElement("div");
    statusBox.classList.add("status-box");
    statusBox.classList.add("examination-doctor-box");

    let status = document.createElement("p");
    status.innerText = parseState(request.state);
    let color = getStateColor(request.state);
    status.classList.add("examination-datetime");
    status.classList.add(color);

    statusBox.appendChild(status);
    requestElem.appendChild(statusBox);

    if (request.state == 2)
    {
        let rejectionBox = document.createElement("div");
        rejectionBox.classList.add("appointment-type-box");
        rejectionBox.classList.add("rejection-reason-box");

        let rejection = document.createElement("p");
        rejection.innerText = request.rejectionReason;
        rejection.classList.add("request-text");


        let rejectionTitle = document.createElement("p");
        rejectionTitle.innerText = "rejection reason:";
        rejectionTitle.classList.add("reaquest-title");

        rejectionBox.appendChild(rejectionTitle);
        rejectionBox.appendChild(rejection);
        requestElem.appendChild(rejectionBox);
    }

    requestsBox.appendChild(requestBox);
}

function parseState(state)
{
    switch (parseInt(state)) {
        case 0:
            return "pending";
            break;
        case 1:
            return "approved";
            break;
        case 2:
            return "rejected";
            break;
    }
}

function getStateColor(state)
{
    switch (parseInt(state)) {
        case 0:
            return "yellow";
            break;
        case 1:
            return "green";
            break;
        case 2:
            return "red";
            break;
    }
}

function sortByProperty(property){  
    return function(a,b){  
       if(a[property] > b[property])  
          return 1;  
       else if(a[property] < b[property])  
          return -1;  
   
       return 0;  
    }  
 }