const deleteBoardButtons = document.querySelectorAll(".delete-board-button");

deleteBoardButtons.forEach(item => item.addEventListener('click',
    () => removeBoard(item.id)));

function removeBoard(buttonId) {

    let id = "";

    for (let i = 3; i < buttonId.length; i++) {
        id += buttonId[i];
    }

    let data = {boardId: id}

    let confirmed = window.confirm("Удалить доску?");

    if (confirmed) {
        $.ajax({
            url: "/Admin/RemoveBoard",
            contentType: 'application/x-www-form-urlencoded',
            data: data,
            method: 'post',
            success: () => {
                alert("Доска удалена.");
                location.reload();
            },
            error: () => {
                alert("Произошла ошибка.")
            }
        })
    }
}