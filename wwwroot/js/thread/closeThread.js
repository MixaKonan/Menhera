const closeThreadButtons = document.querySelectorAll(".close-thread");

closeThreadButtons.forEach(item => item.addEventListener('click',
    () => closeThread(item.id)));

function closeThread(buttonId) {
    let id = "";

    for(let i = 3; i < buttonId.length; i++) {
        id += buttonId[i];
    }

    let data = {threadId: id};

    let confirmed = window.confirm("Закрыть тред?");

    if (confirmed) {
        $.ajax({
            url: "/Admin/CloseThread",
            contentType: 'application/x-www-form-urlencoded',
            data: data,
            method: 'post',
            success: () => {
                alert("Тред закрыт.");
                location.reload();
            },
            error: () => {
                alert("Произошла ошибка.")
            }
        })
    }
}
