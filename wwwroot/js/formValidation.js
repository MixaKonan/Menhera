const fileInput = document.getElementById("files");
const submit = document.getElementById("submit");
const textarea = document.getElementById("Comment")

const commentLimit = 5000;
const threadLimit = 20;

textarea.setAttribute("required", "required");
textarea.setAttribute("maxlength", commentLimit.toString());


function allowedFileAmount() {
    let limit = document.getElementById("fileLimit").value

    if (fileInput.files.length > limit) {
        alert("Файлов больше, чем разрешено на доске.");
        return false;
    }
    return true;
}

function validateThreadCount() {
    let threadCount = document.getElementById("threadCount").value;
    
    if(threadCount >= threadLimit) {
        window.alert("Достигнуто максимальное количество тредов.")
        return false;
    }
    return true;
}

submit.onclick = allowedFileAmount;
submit.onclick = validateThreadCount;


