const timestamps = document.querySelectorAll(".post-time");

function setDate() {
    timestamps.forEach(item => getDate(item));
}

function getDate(obj) {
    let timestamp = obj.innerText;
    
    let date = new Date(timestamp*1000);
    
    obj.innerText = date.getDate()+
        "."+(date.getMonth()+1)+
        "."+date.getFullYear()+
        " "+date.getHours()+
        ":"+date.getMinutes()+
        ":"+date.getSeconds()
}

window.onload = () => setDate();