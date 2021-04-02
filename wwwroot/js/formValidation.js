const fileInput = document.getElementById("files");
const submit = document.getElementById("submit");
const textarea = document.getElementById("Comment")

const commentLimit = 5000;

textarea.setAttribute("required", "required");
textarea.setAttribute("maxlength", commentLimit.toString());

let limit = document.getElementById("fileLimit").value

function allowedFileAmount()
{
    if (fileInput.files.length > limit)
    {
        alert("Файлов больше, чем разрешено на доске.");
        return false;
    }
    return true;
}

submit.onclick = allowedFileAmount;