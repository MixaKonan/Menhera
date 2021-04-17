document.querySelectorAll(".deletePost").forEach(item =>
    item.addEventListener('click', () => deletePost(item.id)))

function deletePost(stringId) {
    let id = "";

    for(let i = 11; i < stringId.length; i++) {
        id += stringId[i];
    }

    let deletionData = {postId: document.getElementById("deleteInput-".concat(id)).value};
    
    let confirmed = window.confirm("Удалить этот пост?");

    if(confirmed) {
        $.ajax({
                url: '/Admin/DeletePost',
                contentType: 'application/x-www-form-urlencoded',
                data: deletionData,
                method: 'post',
                success: () => {
                    alert("Пост удалён");
                    location.reload();
                },
                error: () => {
                    alert("Произошла ошибка")
                }
        })
    }
}