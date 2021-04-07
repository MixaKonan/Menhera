const buttons = document.querySelectorAll(".delete-button");

buttons.forEach(item => item.addEventListener('click',
    () => removeBoard(item.id)));

function removeBoard (boardId) {

    let data = {boardId: boardId}

    $.ajax({
        url: "/Admin/RemoveBoard",
        contentType: 'application/x-www-form-urlencoded',
        data: data,
        method: 'post',
        success: () => {
            alert("Доска удалена.");
        },
        error: () => {
            alert("Произошла ошибка.")
        }
    })
}