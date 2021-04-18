const pinPostButtons = document.querySelectorAll(".pinPost");

pinPostButtons.forEach(item => item.addEventListener('click',
    () => pinPost(item.id)));

function pinPost(btnId) {
    let postId = "";
    
    for(let i = 9; i < btnId.length; i++) {
        postId += btnId[i];
    }

    let data = {postId: postId};

    let confirmed = window.confirm("Закрепить сообщение?");

    if (confirmed) {
        $.ajax({
            url: "/Admin/PinPost",
            contentType: 'application/x-www-form-urlencoded',
            data: data,
            method: 'post',
            success: () => {
                alert("Успешно.")
                window.location.reload();
            },
            error: () => {
                alert("Произошла ошибка.")
            }
        })
    }
}