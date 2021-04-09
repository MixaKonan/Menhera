const fileInput = document.getElementById("files");
const submit = document.getElementById("submit");
const textarea = document.getElementById("Comment")

const commentLimit = 5000;
const threadLimit = 20;

textarea.setAttribute("required", "required");
textarea.setAttribute("maxlength", commentLimit.toString());

function validateNewPostForm() {
    let limit = document.getElementById("fileLimit").value

    if (fileInput.files.length > limit) {
        window.alert("Файлов больше, чем разрешено на доске.");
        return false;
    }
    return true;
}

submit.onclick = validateNewPostForm;