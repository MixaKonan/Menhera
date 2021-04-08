const deleteButtons = document.querySelectorAll(".delete-thread");

deleteButtons.forEach(item => item.addEventListener('click',
    () => removeThread(item.id)));

function removeThread(buttonId) {
    let id = "";

    for (let i = 3; i < buttonId.length; i++) {
        id += buttonId[i];
    }

    let data = {threadId: id};

    let confirmed = window.confirm("Удалить тред?");

    if (confirmed) {
        $.ajax({
            url: "/Admin/RemoveThread",
            contentType: 'application/x-www-form-urlencoded',
            data: data,
            method: 'post',
            success: () => {
                alert("Тред удалён.");
            },
            error: () => {
                alert("Произошла ошибка.")
            }
        })
    }
}

