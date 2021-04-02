const buttons = document.querySelectorAll(".optionsButton")

buttons.forEach(item => item.addEventListener('click',
    () => openCloseOptions(item.id)));

function openCloseOptions(buttonId) {
    let id = "";

    for (let i = 14; i < buttonId.length; i++) {

        id += buttonId[i];
    }

    let options = document.getElementById("optionsList-".concat(id));

    if (options.style.display === "none" || options.style.display === "") {

        options.style.display = "inline-block";
    }
    
    else if (options.style.display === "inline-block") {

        options.style.display = "none";
    }    
}