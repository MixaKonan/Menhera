const buttons = document.querySelectorAll(".delete-button");

buttons.forEach(item => item.addEventListener('click',
    () => removeAdmin(item.id)));

function removeAdmin (adminId) {
    
    let data = {adminId: adminId}

    $.ajax({
        url: "/Admin/RemoveAdmin",
        contentType: 'application/x-www-form-urlencoded',
        data: data,
        method: 'post',
        success: () => {
            alert("Админ удалён.");
        },
        error: () => {
            alert("Произошла ошибка.")
        }
    })
}