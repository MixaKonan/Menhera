const openFormButtons = document.querySelectorAll(".open-button");
const closeFormButtons = document.querySelectorAll(".close-button");

const submitButtons = document.querySelectorAll(".report-btn");

submitButtons.forEach(item => item.addEventListener('click',
    () => banAnon(item.id)))

openFormButtons.forEach(item => item.addEventListener('click',
    () => openForm(item.id)));

closeFormButtons.forEach(item => item.addEventListener('click',
    () => closeForm(item.id)));

function banAnon(stringId) {
    let id = "";

    for (let i = 11; i < stringId.length; i++) {

        id += stringId[i];
    }

    let ipHash = document.getElementById("banIpHash-".concat(id)).value;
    let banEnd = document.getElementById("banCalendar-".concat(id)).value;
    let reason = document.getElementById("report-reason-".concat(id)).value;

    let banData = {anonIpHash: ipHash, reason: reason, banEndTime: banEnd};

    let confirmed = window.confirm("Забанить?");

    if (confirmed) {
        $.ajax({
            url: "/Admin/BanAnon",
            contentType: 'application/x-www-form-urlencoded',
            data: banData,
            method: 'post',
            success: () => {
                alert("Анон забанен");
                location.reload();
            },
            error: () => {
                alert("Произошла ошибка")
            }
        })
    }
}

function openForm(buttonId) {
    let id = "";

    for (let i = 12; i < buttonId.length; i++) {

        id += buttonId[i];
    }

    let form = document.getElementById("myForm-".concat(id));

    form.style.display = "block";
}

function closeForm(buttonId) {
    let id = "";

    for (let i = 13; i < buttonId.length; i++) {

        id += buttonId[i];
    }

    let form = document.getElementById("myForm-".concat(id));

    form.style.display = "none";
}