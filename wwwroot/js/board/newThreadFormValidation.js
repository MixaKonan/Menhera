const fileInput = document.getElementById("files");
const submit = document.getElementById("submit");
const textarea = document.getElementById("Comment")

const threadLimit = 20;
const commentLimit = 5000;

textarea.setAttribute("required", "required");
textarea.setAttribute("maxlength", commentLimit.toString());

function validateNewThreadForm() {
    let threadCount = document.getElementById("threadCount").value;

    if (threadCount >= threadLimit) {
        window.alert("Достигнуто максимальное количество тредов.")
        return false;
    }

    let limit = document.getElementById("fileLimit").value

    if (fileInput.files.length > limit) {
        window.alert("Файлов больше, чем разрешено на доске.");
        return false;
    }
    return true;
}

submit.onclick = validateNewThreadForm;


