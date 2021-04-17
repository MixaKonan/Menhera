const submitBtn = document.getElementById("submit-change");
const email = document.getElementById("email");
const login = document.getElementById("login");
const password = document.getElementById("password");
const color = document.getElementById("color");

function submitChanges() {
    let data = {email: email.value, login: login.value, password: password.value, color: color.value};

    $.ajax({
        url: "/Admin/Account",
        contentType: 'application/x-www-form-urlencoded',
        data: data,
        method: 'post',
        success: (result) => {
                window.location.href = result.redirectToUrl;
        },
        error: () => {
            alert("Произошла ошибка.")
        }
    })
}

submitBtn.onclick = submitChanges;