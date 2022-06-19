let missingEquipmentUri = "https://localhost:7195/api/EquipmentRequest/missingEquipment"
let orderUri = "https://localhost:7195/api/EquipmentRequest/orderEquipment"

/*
currentTime();
let user = JSON.parse(sessionStorage.getItem("user"));
console.log(user);
setNameOnCorner(user);
*/

var equipment = [];

let getMissingEquipmentRequest = new XMLHttpRequest();
getMissingEquipmentRequest.open('GET', missingEquipmentUri); 
getMissingEquipmentRequest.onreadystatechange = function () {
    if (this.readyState === 4) {
        if (this.status === 200) {
            equipment = JSON.parse(getMissingEquipmentRequest.response);
            populateTable()
        } else {
            alert("Greska prilikom kreiranja korisnika.")
        }
    }
}

getMissingEquipmentRequest.send();

function populateTable() {
    let table = document.createElement("table")
    let row = table.insertRow(0);
    let cell1 = row.insertCell(0);
    cell1.innerHTML = "ID"
    let cell2 = row.insertCell(1);
    cell2.innerHTML = "NAME"

    for (let i = 0; i < equipment.length; ++i) {
        let eq = equipment[i]

        let row = table.insertRow(i+1);
        let cell1 = row.insertCell(0);
        let cell2 = row.insertCell(1);

        cell1.innerHTML = eq["id"];
        cell2.innerHTML = eq["name"];
    }
    document.getElementById("equipment-table").appendChild(table);
}

document.getElementById("order").addEventListener("click", function(){
    event.preventDefault()
    orderEquipment()
});

function orderEquipment() {
    let equipmentId = document.getElementById("equipment-id").value
    let amount = document.getElementById("amount").value

    let dto = [{
        "equipmentId" : equipmentId,
        "amount" : amount,
    } ]

    let orderEquipmentRequest = new XMLHttpRequest();
    orderEquipmentRequest.open('PUT', orderUri); 
    orderEquipmentRequest.setRequestHeader('Content-Type', 'application/json');
    orderEquipmentRequest.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                alert("Order successful!")
                location.reload();
            } else {
                alert("Appointment failed! Doctor is busy.")
            }
        }
    }
    orderEquipmentRequest.send(JSON.stringify(dto));
}


