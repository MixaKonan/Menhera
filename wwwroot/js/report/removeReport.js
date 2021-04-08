const deleteReportButtons = document.querySelectorAll(".delete-report-button");

deleteReportButtons.forEach(item => item.addEventListener('click',
    () => removeReport(item.id)));

function removeReport (buttonId) {

    let id = "";

    for(let i = 3; i < buttonId.length; i++)
    {
        id += buttonId[i];
    }

    let data = {reportId: id}


    let confirmed = window.confirm("Убрать репорт?");

    if (confirmed) {
        $.ajax({
            url: "/Admin/RemoveReport",
            contentType: 'application/x-www-form-urlencoded',
            data: data,
            method: 'post',
            success: () => {
                alert("Репорт удалён.");
            },
            error: () => {
                alert("Произошла ошибка.")
            }
        })
    }
}