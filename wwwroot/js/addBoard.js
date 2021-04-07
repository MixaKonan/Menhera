const prefix = document.getElementById("board-prefix");
const postfix = document.getElementById("board-postfix");
const title = document.getElementById("board-title");
const description = document.getElementById("board-description");
const isHidden = document.getElementById("board-isHidden");
const anonHasNoName = document.getElementById("board-anonHasNoName");
const hasSubject = document.getElementById("board-hasSubject");
const filesAreAllowed = document.getElementById("board-filesAreAllowed");
const fileLimit = document.getElementById("board-fileLimit");
const anonName = document.getElementById("board-anonName");

const boardSubmit = document.getElementById("board-submit");

function addBoard() {
    
    let data = {
        prefix: prefix.value,
        postfix: postfix.value,
        title: title.value,
        description: description.value,
        isHidden: isHidden.value,
        anonHasNoName: anonHasNoName.value,
        hasSubject: hasSubject.value,
        filesAreAllowed: filesAreAllowed.value,
        fileLimit: fileLimit.value,
        anonName: anonName.value
    };

    $.ajax({
        url: "/Admin/AddBoard",
        contentType: 'application/x-www-form-urlencoded',
        data: data,
        method: 'post',
        success: () => {
            alert("Доска добавлена.");
        },
        error: () => {
            alert("Произошла ошибка.")
        }
    })
}

boardSubmit.onclick = addBoard;