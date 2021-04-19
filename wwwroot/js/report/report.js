const submit = document.getElementById("submit");
const textarea = document.getElementById("reason");

const commentLimit = 5000;

textarea.setAttribute("required", "required");
textarea.setAttribute("maxlength", commentLimit.toString());

function sendReport() {
    let postId = document.getElementById("post-id").value;
    let reason = textarea.value;
    
    let data = {postId: postId, reason: reason};
    
    $.ajax({
        url: "/Report/Report",
        contentType: 'application/x-www-form-urlencoded',
        data: data,
        method: 'post',
        success: () => {
            window.alert("Репорт отправлен.");
            window.history.go(-1);
        },
        error: () => {
            window.alert("Произошла ошибка.");
        }
    })
}

submit.onclick = sendReport;