const fileInput = document.getElementById("files");
const submit = document.getElementById("submit");
const textarea = document.getElementById("comment");

const threadLimit = 20;
const commentLimit = 5000;

const _allowedMimeFileExtensions = ['image/jpeg', 'image/png', 'image/gif'];

textarea.setAttribute("required", "required");
textarea.setAttribute("maxlength", commentLimit.toString());

function validateNewThreadForm() {
    let threadCount = document.getElementById("threadCount").value;

    if (threadCount >= threadLimit) {
        window.alert("Достигнуто максимальное количество тредов.")
        return false;
    }

    let limit = document.getElementById("fileLimit").value
    let files = fileInput.files;

    if (files.length > limit) {
        window.alert("Файлов больше, чем разрешено на доске.");
        return false;
    }

    for (let i = 0; i < files.length; i++) {
        let file = files[i];

        if(!_allowedMimeFileExtensions.includes(file.type)) {
            window.alert("Неподдерживаемый формат файла: " + file.type + "\nПоддерживаемые форматы: " + _allowedMimeFileExtensions.join(', '));
            return false;
        }

        if(file.size > _allowedFileSize){
            window.alert("Размер файла слишком большой. Разрешённый размер -- 5 МБ");
            return false;
        }
    }

    return true;
}

submit.onclick = validateNewThreadForm;


