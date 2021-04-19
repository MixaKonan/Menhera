const submitBtn = document.getElementById("submit-change");
const email = document.getElementById("email");
const login = document.getElementById("login");
const password = document.getElementById("password");
const color = document.getElementById("color");

function submitChanges() {
    let data = {email: email.value, login: login.value, password: password.value, color: color.value};

    $.ajax({
        url: "/Admin/ChangeAccountDetails",
        contentType: 'application/x-www-form-urlencoded',
        data: data,
        method: 'post',
        success: (result) => {
            if(result.redirectToUrl != null) {
                window.location.href = result.redirectToUrl;
            }
            else {
                window.location.reload();
            }
        },
        error: () => {
            alert("Произошла ошибка.")
        }
    })
}

submitBtn.onclick = submitChanges;