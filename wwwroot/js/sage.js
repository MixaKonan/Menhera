const checkbox =  document.getElementById("sage");
const email =  document.getElementById("email");

function sageThread()
{
    if(checkbox.checked)
    {
        email.value = "sage";
    }
    if(!checkbox.checked)
    {
        email.value = "";
    }
}

checkbox.onchange = sageThread;