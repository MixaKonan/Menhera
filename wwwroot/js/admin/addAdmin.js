const email = document.getElementById("admin-email");
const login = document.getElementById("admin-login");
const password = document.getElementById("admin-password");
const canBanUsers = document.getElementById("admin-canBanUsers");
const canCloseThreads = document.getElementById("admin-canCloseThreads");
const canDeletePosts = document.getElementById("admin-canDeletePosts");
const hasAccessToPanel = document.getElementById("admin-hasAccessToPanel");

const adminSubmit = document.getElementById("admin-submit");

function addAdmin() {
    let data = {
        email: email.value,
        login: login.value,
        passwordHash: password.value,
        canBanUsers: canBanUsers.value,
        canCloseThreads: canCloseThreads.value,
        canDeletePosts: canDeletePosts.value,
        hasAccessToPanel: hasAccessToPanel.value
    };

    $.ajax({
        url: "/Admin/AddAdmin",
        contentType: 'application/x-www-form-urlencoded',
        data: data,
        method: 'post',
        success: () => {
            alert("Админ добавлен.");
            location.reload();
        },
        error: () => {
            alert("Произошла ошибка.")
        }
    })
}

adminSubmit.onclick = addAdmin;